using Image_Colour_Swap;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Net;
using System.Text.Json;
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
    public async Task<IActionResult> Save(string sourceFile, string palletteFile)
    {
        try
        {
            var imageSaver = new S3ImageSaver();
            var imageLoader = new ImageSharpImageLoader();

            var imageStream = imageLoader.GenerateStream(sourceFile);
            var sourceImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
            var result = await imageSaver.SaveAsync(sourceImageFilename, imageStream);

            imageStream = imageLoader.GenerateStream(palletteFile);
            var palletteImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
            result = await imageSaver.SaveAsync(palletteImageFilename, imageStream);

            var httpClient = new HttpClient();
            var processingUri = _configuration["Settings:ProcessingUri"];
            var url = $"https://{processingUri}.execute-api.eu-west-2.amazonaws.com";
            httpClient.BaseAddress = new Uri(url);

            _logger.LogInformation($"Pallette Image:{palletteImageFilename}");
            _logger.LogInformation($"Source Image:{sourceImageFilename}");

            var response = await httpClient.GetAsync($"/Integration?palletteImage={palletteImageFilename}&sourceImage={sourceImageFilename}");
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
