using ImageHelpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Bootstrapping;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers;

public class ResultsController(
    IImageResultsRepository<ResultsModel> imageResultsRepository,
    ILogger<HomeController> logger,
    IUrlGenerator urlGenerator) : Controller
{
    private readonly IImageResultsRepository<ResultsModel> _imageResultsRepository = imageResultsRepository;
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IUrlGenerator _urlGenerator = urlGenerator;

    public async Task<IActionResult> Index(string id)
    {
        _logger.LogInformationMessage($"In results method, Id is {id}");

        ResultsModel resultsModel = await _imageResultsRepository.LoadResults(id);

        if (resultsModel != null)
        {
            resultsModel.OutputImage = _urlGenerator.GetUrl(resultsModel.OutputImage);
            resultsModel.PalletteImage = _urlGenerator.GetUrl(resultsModel.PalletteImage);
            resultsModel.SourceImage = _urlGenerator.GetUrl(resultsModel.SourceImage);

            return View(resultsModel);
        }

        _logger.LogInformationMessage("ResultsModel was null.");
        return RedirectToAction("Index", "Home");
    }
}