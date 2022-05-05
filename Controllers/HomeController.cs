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
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public async Task<IActionResult> Save(IEnumerable<IFormFile> file)
    {
        try
        {
            if(file.Count() != 2)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            if(file.ToList()[0].Length <= 0 || file.ToList()[1].Length <= 0)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            var imageSaver = new S3ImageSaver();
            var stream = new MemoryStream();

            byte[] imageBytes;
            
            using (MemoryStream ms = new MemoryStream())
            {
                file.ToList()[0].CopyTo(ms);
                imageBytes = ms.ToArray();
            }

            using (var image = Image.Load(imageBytes))
            {
                image.Save(stream, new JpegEncoder());
            }

            var sourceImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
            var result = await imageSaver.SaveAsync(sourceImageFilename, stream);

            stream = new MemoryStream();
            
            using (MemoryStream ms = new MemoryStream())
            {
                file.ToList()[1].CopyTo(ms);
                imageBytes = ms.ToArray();
            }

            using (var image = Image.Load(imageBytes))
            {
                image.Save(stream, new JpegEncoder());
            }

            var palletteImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
            result = await imageSaver.SaveAsync(palletteImageFilename, stream);

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://9g8n5f9ggi.execute-api.eu-west-2.amazonaws.com");
            var response = await httpClient.GetAsync($"/Test?palletteImage={palletteImageFilename}&sourceImage={sourceImageFilename}");
            var responseString = await response.Content.ReadAsStringAsync();

            return new OkObjectResult(responseString);
        }
        catch(Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError); 
        }
    }
}
