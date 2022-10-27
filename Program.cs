using Image_Colour_Swap.Interfaces;
using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IImageLoader, ImageSharpImageLoader>();
builder.Services.AddScoped<IImageSaver, S3ImageSaver>();

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