﻿using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Image_Colour_Swap;
using Image_Colour_Swap.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Net;
using System.Text.Json;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IImageLoader _imageLoader;
    private readonly IImageSaver _imageSaver;
    private readonly ILogger<HomeController> _logger;
    private readonly IImageResultsRepository<ResultsModel> _resultsRepository;

    public HomeController(IConfiguration configuration, 
        IImageSaver imageSaver,
        IImageLoader imageLoader,
        ILogger<HomeController> logger,
        IImageResultsRepository<ResultsModel> resultsSaver)
    {
        _configuration = configuration;
        _imageLoader = imageLoader;
        _imageSaver = imageSaver;
        _logger = logger;
        _resultsRepository = resultsSaver;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public async Task<IActionResult> Save(string sourceFile, string palletteFile)
    {
        try
        {
            _logger.LogInformation("Starting Save action.")
            _logger.LogInformation($"Source is {sourceFile");
            _logger.LogInformation($"Pallette is {palletteFile});

            var imageStream = _imageLoader.GenerateStream(sourceFile);
            var sourceImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
            var result = await _imageSaver.SaveAsync(sourceImageFilename, imageStream);
            _logger.LogInformation($"Source saved as {sourceImageFilename});

            imageStream = _imageLoader.GenerateStream(palletteFile);
            var palletteImageFilename = $"{Guid.NewGuid().ToString()}.jpg";
            result = await _imageSaver.SaveAsync(palletteImageFilename, imageStream);
            _logger.LogInformation($"Pallette saved as {palletteImageFilename});

            var httpClient = new HttpClient();
            var processingUri = _configuration["Settings:ProcessingUri"];
            var url = $"https://{processingUri}.execute-api.eu-west-2.amazonaws.com";
            httpClient.BaseAddress = new Uri(url);
            _logger.LogInformation("Calling Lambda function");

            var response = await httpClient.GetAsync($"/Integration?palletteImage={palletteImageFilename}&sourceImage={sourceImageFilename}");
            var responseString = await response.Content.ReadAsStringAsync();
            var resultsModel = JsonSerializer.Deserialize<ResultsModel>(responseString);

            _logger.LogInformation("Back from Lambda");
            _logger.LogInformation(responseString);

            if(resultsModel != null)
            {
                resultsModel.ResultsId = Guid.NewGuid().ToString();
                var saveResult = await _resultsRepository.SaveResults(resultsModel);
                _logger.LogInformation($"Returning: {resultsModel.ResultsId}");

                return StatusCode((int)HttpStatusCode.OK, resultsModel.ResultsId);
            }

            return RedirectToAction("Index");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError); 
        }
    }
}
