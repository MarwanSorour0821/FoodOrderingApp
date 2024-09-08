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
        if (HttpContext.Session.GetString("EMAIL") == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return View();
    }

    public IActionResult Admin()
    {
        if (HttpContext.Session.GetString("EMAIL") == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return View();
    }

    public IActionResult searchOrder()
    {
        if (HttpContext.Session.GetString("EMAIL") == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return RedirectToAction("SearchOrder", "searchOrder");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        if (HttpContext.Session.GetString("EMAIL") == null)
        {
            return RedirectToAction("Index", "Login");
        }

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

