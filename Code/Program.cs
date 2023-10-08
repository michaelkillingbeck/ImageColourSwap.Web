using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Web.Bootstrapping;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSystemsManager("/ICS/", new AWSOptions
{
    Region = RegionEndpoint.EUWest2
});

ServicesBootstrapping.AddServices(builder);
LoggingBootstrapping.AddLogging(builder);
builder.Services.AddControllersWithViews();
IdentityBootstrapping.AddIdentityProvider(builder);

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