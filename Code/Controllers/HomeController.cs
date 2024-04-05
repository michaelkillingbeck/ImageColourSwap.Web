using System.Net;
using System.Text.Json;
using ImageHelpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Bootstrapping;
using Web.Models;

namespace Web.Controllers;

public class HomeController(
    IImageSaver imageSaver,
    IImageHandler imageLoader,
    ILogger<HomeController> logger,
    IImageResultsRepository<ResultsModel> resultsSaver,
    IOptions<SettingsModel> settings) : Controller
{
    private readonly IImageHandler _imageHandler = imageLoader;
    private readonly IImageSaver _imageSaver = imageSaver;
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IImageResultsRepository<ResultsModel> _resultsRepository = resultsSaver;
    private readonly SettingsModel _settings = settings.Value;

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    [Authorize]
    public async Task<IActionResult> Save(string sourceFile, string palletteFile)
    {
        try
        {
            _logger.LogInformationMessage("Starting Save action.");
            _logger.LogInformationMessage($"Length of Source is {sourceFile.Length}");
            _logger.LogInformationMessage($"Length of Pallette is {palletteFile.Length}");

            Stream imageStream = _imageHandler.GenerateStream(sourceFile);
            string sourceImageFilename = $"{Guid.NewGuid()}.jpg";
            _ = await _imageSaver.SaveAsync(sourceImageFilename, imageStream);
            _logger.LogInformationMessage($"Source saved as {sourceImageFilename}");

            imageStream = _imageHandler.GenerateStream(palletteFile);
            string palletteImageFilename = $"{Guid.NewGuid()}.jpg";
            _ = await _imageSaver.SaveAsync(palletteImageFilename, imageStream);
            _logger.LogInformationMessage($"Pallette saved as {palletteImageFilename}");

            HttpClient httpClient = new();
            string url = $"https://{_settings.ProcessingUri}.execute-api.eu-west-2.amazonaws.com";
            httpClient.BaseAddress = new Uri(url);
            _logger.LogInformationMessage("Calling Lambda function");

            HttpResponseMessage response = await httpClient.GetAsync($"/Integration?palletteImage={palletteImageFilename}&sourceImage={sourceImageFilename}");
            string responseString = await response.Content.ReadAsStringAsync();
            ResultsModel? resultsModel = JsonSerializer.Deserialize<ResultsModel>(responseString);

            _logger.LogInformationMessage("Back from Lambda");
            _logger.LogInformationMessage(responseString);

            if (resultsModel != null)
            {
                resultsModel.ResultsId = Guid.NewGuid().ToString();
                _ = await _resultsRepository.SaveResults(resultsModel);
                _logger.LogInformationMessage($"Returning: {resultsModel.ResultsId}");

                return StatusCode((int)HttpStatusCode.OK, resultsModel.ResultsId);
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
