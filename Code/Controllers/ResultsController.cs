using ImageHelpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers;

public class ResultsController : Controller
{
    private readonly IImageResultsRepository<ResultsModel> _imageResultsRepository;
    private readonly ILogger<HomeController> _logger;
    private readonly IUrlGenerator _urlGenerator;

    public ResultsController(
        IImageResultsRepository<ResultsModel> imageResultsRepository,
        ILogger<HomeController> logger,
        IUrlGenerator urlGenerator)
    {
        _imageResultsRepository = imageResultsRepository;
        _logger = logger;
        _urlGenerator = urlGenerator;
    }

    public async Task<IActionResult> Index(string id)
    {
        _logger.LogInformation($"In results method, Id is {id}");
        
        var resultsModel = await _imageResultsRepository.LoadResults(id);

        if(resultsModel != null)
        {
            resultsModel.OutputImage = _urlGenerator.GetUrl(resultsModel.OutputImage);
            resultsModel.PalletteImage = _urlGenerator.GetUrl(resultsModel.PalletteImage);
            resultsModel.SourceImage = _urlGenerator.GetUrl(resultsModel.SourceImage);

            return View(resultsModel);
        }

        _logger.LogInformation("ResultsModel was null.");
        return RedirectToAction("Index", "Home");
    }
}