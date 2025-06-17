using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Reposiotories;
using FilmAPI.Models;
using FluentValidation;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("filmapi/film")]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository _filmRepo;
        private readonly IMapper _mapper;
        IValidator<CreateFilmDTO> _validator;

        public FilmController(
            IFilmRepository filmRepo, IMapper mapper, IValidator<CreateFilmDTO> validator)
        {
            _validator = validator;
            _filmRepo = filmRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var films = await _filmRepo.GetAllAsync();
            var filmDTOs = films.Select(f => _mapper.Map<FilmDTO>(f));
            return Ok(filmDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var film = await _filmRepo.GetByIdAsync(id);
            if (film == null)
                return NotFound();

            return Ok(_mapper.Map<FilmDTO>(film));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateFilmDTO filmDTO)
        {
            var result = await _validator.ValidateAsync(filmDTO);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            var film = await _filmRepo.CreateAsync(filmDTO);
            var filmResult = _mapper.Map<FilmDTO>(film);

            return CreatedAtAction(
                nameof(GetById),
                new { id = filmResult.Id },
                filmResult);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFilmDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var film = await _filmRepo.UpdateAsync(id, updateDTO);
            if (film == null)
                return NotFound();

            return Ok(_mapper.Map<FilmDTO>(film));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var film = await _filmRepo.DeleteAsync(id);

            if (film == null)
                return NotFound();

            return NoContent(); // deletion already handled in repo
        }

        [HttpPost("toggle-like-by-user")]
        [Authorize]
        public async Task<IActionResult> ToggleLike([FromRoute] int filmId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var film = await _filmRepo.GetByIdAsync(filmId);
            if (film == null)
                return NotFound();

            var existing = film.LikedByUsers.FirstOrDefault(u => u.Id == userId);
            if (existing != null)
                film.LikedByUsers.RemoveAll(u => u.Id == userId);
            else
                film.LikedByUsers.Add(new User { Id = userId });

            var updatedFilm = await _filmRepo.UpdateLikeAsync(film);
            if (updatedFilm == null)
                return StatusCode(500, "Failed to update film.");

            var dto = _mapper.Map<ToggleLikeFilmDTO>(updatedFilm);
            return Ok(dto);
        }
    }
}
