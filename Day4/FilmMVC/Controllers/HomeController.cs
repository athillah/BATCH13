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
    
    public IActionResult CreateEdit(int? id) {
        if (id != null) {
            var aFilm = _context.Films.SingleOrDefault(film => film.Id == id);
            return View(aFilm);
        }
        return View();
    }
    public IActionResult Delete(int id) {
        var aFilm = _context.Films.SingleOrDefault(film => film.Id == id);
        if (aFilm == null) {
            return NotFound();
        }
        _context.Films.Remove(aFilm);
        _context.SaveChanges();
        return RedirectToAction("Film");
    }
    
    public IActionResult SubmitForm(Film model) {
        if (model.Id == 0) {
            _context.Films.Add(model);
        } else {
            _context.Films.Update(model);
        }
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
