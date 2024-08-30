using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FoodApplication.Models;

namespace FoodApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Admin1()
    {
        return View();
    }

    public IActionResult Admin()
    {
        return View();
    }

    public IActionResult searchOrder()
    {
        return RedirectToAction("SearchOrder", "searchOrder");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

