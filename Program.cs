using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Image_Colour_Swap.Interfaces;
using Microsoft.Extensions.Configuration;
using Web;
using Web.Helpers;
using Web.Models;
using Web.Services;
using Web.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();