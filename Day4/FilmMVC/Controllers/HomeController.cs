using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FilmMVC.Models;

namespace FilmMVC.Controllers;

public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly FilmDB _context;

    public HomeController(ILogger<HomeController> logger, FilmDB context) {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult Film() {
        var films = _context.Films.ToList();
        return View(films);
    }
    
    public IActionResult CreateEdit() {
        return View();
    }
    
    public IActionResult SubmitForm (Film model) {
        _context.Films.Add(model);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
