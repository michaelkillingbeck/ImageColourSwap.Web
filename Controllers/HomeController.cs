using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Results(string id)
    {
        _logger.LogInformation("In results method...");

        var tempDataString = TempData[id]?.ToString();
        
        if(String.IsNullOrEmpty(tempDataString) == false)
        {
            var resultsModel = JsonSerializer.Deserialize<ResultsModel>(tempDataString);
            return View(resultsModel);
        }

        return RedirectToAction("Index");
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
            var url = "https://${_configuration["Settings:ProcessingUri"]}.execute-api.eu-west-2.amazonaws.com"
            httpClient.BaseAddress = new Uri(url);
            _logger.LogInformation($"Pallette Image:{palletteImageFilename}");
            _logger.LogInformation($"Source Image:{sourceImageFilename}");
            var response = await httpClient.GetAsync($"/integration?palletteImage={palletteImageFilename}&sourceImage={sourceImageFilename}");
            var responseString = await response.Content.ReadAsStringAsync();
            var resultsModel = JsonSerializer.Deserialize<ResultsModel>(responseString);

            _logger.LogInformation(responseString);
            _logger.LogInformation("Back from Lambda");

            var id = Guid.NewGuid();
            TempData[id.ToString()] = JsonSerializer.Serialize(resultsModel);

            _logger.LogInformation($"Returning: {id}");
            return StatusCode((int)HttpStatusCode.OK, id);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError); 
        }
    }
}
