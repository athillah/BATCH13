using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FilmMVC.Models;

namespace FilmMVC.Controllers;

public class HomeController : Controller
{
    private static List<Film> films = new List<Film>();
    public IActionResult Index() {
        return View(films);
    }
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Film film)
    {
        film.Id = films.Count + 1;
        films.Add(film);
        return RedirectToAction("Index");
    }
}
