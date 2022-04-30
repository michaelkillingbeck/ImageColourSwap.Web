using System.Net;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(string sourceImage, string palletteImage)
    {
        var imageSaver = new S3ImageSaver();
        var stream = new MemoryStream();

        sourceImage = sourceImage.Substring(22);
        palletteImage = palletteImage.Substring(22);

        byte[] imageBytes = Convert.FromBase64String(sourceImage);
        using (var image = Image.Load(imageBytes))
        {
            image.Save(stream, new JpegEncoder());
        }

        var sourceImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
        var result = await imageSaver.SaveAsync(sourceImageFilename, stream);

        stream = new MemoryStream();
        imageBytes = Convert.FromBase64String(palletteImage);
        using (var image = Image.Load(imageBytes))
        {
            image.Save(stream, new JpegEncoder());
        }

        var palletteImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
        result = await imageSaver.SaveAsync(palletteImageFilename, stream);

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://9g8n5f9ggi.execute-api.eu-west-2.amazonaws.com");
        var response = await httpClient.GetAsync($"/Test?palletteImage={palletteImageFilename}&sourceImage={sourceImageFilename}");
        var s = response.Content.ReadAsStringAsync();

        return new OkResult();
    }
}
