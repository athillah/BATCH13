using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FilmAPI.Models;
using FilmAPI.Data;
using FilmAPI.DTOs;
using FilmAPI.Mappers;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("filmapi/film")]
    public class FilmController : ControllerBase
    {
        private readonly AppDBContext _context;
        public FilmController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var films = _context.Films.ToList()
                .Select(f => f.ToFilmDTO());

            return Ok(films);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var film = _context.Films.Find(id);

            return film != null
                ? Ok(film.ToFilmDTO())
                : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateFilmRequestDTO filmDTO)
        {
            var filmModel = filmDTO.ToFilmFromCreateDTO();

            _context.Films.Add(filmModel);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById), new { id = filmModel.Id }, filmModel.ToFilmDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateFilmRequestDTO updateDTO)
        {
            var filmModel = _context.Films.FirstOrDefault(
                f => f.Id == id);

            if (filmModel == null)
                return NotFound();

            filmModel.Title = updateDTO.Title ?? filmModel.Title;
            filmModel.Year = updateDTO.Year ?? filmModel.Year;
            filmModel.Director = updateDTO.Director ?? filmModel.Director;

            _context.SaveChanges();
            return Ok(
                filmModel.ToFilmDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var filmModel = _context.Films.FirstOrDefault(
                f => f.Id == id);

            if (filmModel == null)
                return NotFound();

            _context.Films.Remove(filmModel);
            _context.SaveChanges();

            return NoContent();
        }
    }
}