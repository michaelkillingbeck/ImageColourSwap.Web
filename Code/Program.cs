using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Web.Bootstrapping;
using Web.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddSystemsManager("/ICS/", new AWSOptions
{
    Region = RegionEndpoint.EUWest2,
});

ServicesBootstrapping.AddServices(builder);
LoggingBootstrapping.AddLogging(builder);
builder.Services.AddControllersWithViews();
IdentityBootstrapping.AddIdentityProvider(builder);

builder.Services.Configure<SettingsModel>(
    builder.Configuration.GetSection("Settings"));

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    _ = app.UseHsts();
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