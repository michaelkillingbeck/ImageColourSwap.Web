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

        var result = await imageSaver.SaveAsync($"{Guid.NewGuid().ToString()}.jpg", stream);

        stream = new MemoryStream();
        imageBytes = Convert.FromBase64String(palletteImage);
        using (var image = Image.Load(imageBytes))
        {
            image.Save(stream, new JpegEncoder());
        }

        result = await imageSaver.SaveAsync($"{Guid.NewGuid().ToString()}.jpg", stream);

        return new OkResult();
    }
}
