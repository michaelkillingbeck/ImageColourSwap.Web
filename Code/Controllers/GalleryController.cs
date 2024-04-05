using Microsoft.AspNetCore.Mvc;
using Web.Bootstrapping;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers;

public class GalleryController(
    IGalleryResultsService galleryService,
    ILogger<HomeController> logger) : Controller
{
    private readonly IGalleryResultsService _galleryService = galleryService;
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index(PagedSearchRequest request)
    {
        _logger.LogInformationMessage($"In Index, there are {request.PageMarkers.Count} Page Markers.");
        PagedResultsModel model = await _galleryService.GetPage(request);

        if (model.Results.ToList().Count > 0)
        {
            return View(model);
        }

        _logger.LogInformationMessage("No results found.");
        return RedirectToAction("Index", "Home");
    }
}