using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Web.Models;

namespace Web.Controllers;

public class ResultsController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ResultsController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string id)
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
}