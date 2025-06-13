using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmAPI.Data;
using FilmAPI.DTOs;
using FilmAPI.Mappers;
using FilmAPI.Reposiotories;
using FilmAPI.Models;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("filmapi/film")]
    public class FilmController : ControllerBase
    {
        private AppDBContext _context;
        private IFilmRepository _repo;
        public FilmController(AppDBContext context, IFilmRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var films = await _repo.GetAllAsync();
            var filmDTO = films.Select(f => f.ToFilmDTO());

            return Ok(films);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var film = await _repo.GetByIdAsync(id);

            return film != null
                ? Ok(film.ToFilmDTO())
                : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFilmRequestDTO filmDTO)
        {
            var filmModel = filmDTO.ToFilmFromCreateDTO();

            await _repo.CreateAsync(filmModel);

            return CreatedAtAction(
                nameof(GetById),
                new { id = filmModel.Id }, filmModel.ToFilmDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFilmRequestDTO updateDTO)
        {
            var film = await _repo.UpdateAsync(id, updateDTO);

            if (film == null)
                return NotFound();

            return Ok(film.ToFilmDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var filmModel = await _repo.DeleteAsync(id);

            if (filmModel == null)
                return NotFound();

            _context.Films.Remove(filmModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}