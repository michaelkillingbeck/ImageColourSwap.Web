using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Web.Interfaces;
using Web.Models;
using Web.Services;

namespace Web.Controllers;

public class GalleryController : Controller
{
    private readonly IGalleryResultsService _galleryService;
    private readonly ILogger<HomeController> _logger;

    public GalleryController(
        IGalleryResultsService galleryService,
        ILogger<HomeController> logger)
    {
        _galleryService = galleryService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(PagedSearchRequest request)
    {    
        var model = await _galleryService.GetPage(request);

        if(model.Results.ToList().Count > 0)
        {
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }
}