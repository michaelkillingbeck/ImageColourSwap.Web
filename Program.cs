using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Extensions.Configuration.SystemsManager;
using Amazon.Extensions.NETCore.Setup;
using Image_Colour_Swap.Interfaces;
using Microsoft.Extensions.Configuration;
using Web;
using Web.Helpers;
using Web.Models;
using Web.Services;
using Web.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSystemsManager("/ICS/", new AWSOptions
{
    Region = RegionEndpoint.EUWest2
});

builder.Services.AddScoped<IImageLoader, ImageSharpImageLoader>();
builder.Services.AddScoped<IImageSaver, S3ImageSaver>();
builder.Services.AddScoped<AmazonDynamoDBClient>(provider => {
    return new AmazonDynamoDBClient(RegionEndpoint.EUWest2);
});
builder.Services.AddScoped<IResultsModelMapper<Document>, DynamoDocumentResultsModelMapper>();
builder.Services.AddScoped<IImageResultsRepository<ResultsModel>, DynamoDbResultsRepository>();
builder.Services.AddScoped<IImageResultsRepository<PagedResultsModel>, DynamoDbPagedResultsRepository>();
builder.Services.AddScoped<IUrlGenerator, S3UrlGenerator>();
builder.Services.AddScoped<IGalleryResultsService, GalleryResultsService>();
builder.Services.AddLogging(
    config => 
    { 
        config.AddAWSProvider(builder.Configuration.GetAWSLoggingConfigSection()); 
        config.SetMinimumLevel(LogLevel.Debug); 
    });
builder.Services.AddControllersWithViews();

var cognitoSection = builder.Configuration.GetSection("Cognito");
var cognitoProvider = new AmazonCognitoIdentityProviderClient();
var cognitoUserPool = new CognitoUserPool(
    cognitoSection["UserPoolId"],
    cognitoSection["UserPoolClientid"],
    cognitoProvider,
    cognitoSection["UserPoolClientSecret"]);

builder.Services.Configure<SettingsModel>(
    builder.Configuration.GetSection("Settings")
);

builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(cognitoProvider);
builder.Services.AddSingleton<CognitoUserPool>(cognitoUserPool);
builder.Services.AddCognitoIdentity();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();