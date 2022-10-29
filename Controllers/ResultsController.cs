using Image_Colour_Swap.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Web.Models;

namespace Web.Controllers;

public class ResultsController : Controller
{
    private readonly IImageResultsRepository<ResultsModel> _imageResultsRepository;
    private readonly ILogger<HomeController> _logger;

    public ResultsController(
        IImageResultsRepository<ResultsModel> imageResultsRepository,
        ILogger<HomeController> logger)
    {
        _imageResultsRepository = imageResultsRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string id)
    {
        _logger.LogInformation("In results method...");
        
        var resultsModel = await _imageResultsRepository.LoadResults(id);

        if(resultsModel != null)
        {
            return View(resultsModel);
        }

        return RedirectToAction("Index", "Home");
    }
}