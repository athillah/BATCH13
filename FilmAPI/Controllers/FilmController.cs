using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Reposiotories;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("filmapi/film")]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository _repo;
        private readonly IMapper _mapper;

        public FilmController(IFilmRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var films = await _repo.GetAllAsync();
            var filmDTOs = films.Select(f => _mapper.Map<FilmDTO>(f));
            return Ok(filmDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var film = await _repo.GetByIdAsync(id);
            if (film == null)
                return NotFound();

            return Ok(_mapper.Map<FilmDTO>(film));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFilmDTO filmDTO)
        {
            var film = await _repo.CreateAsync(filmDTO);
            var filmResult = _mapper.Map<FilmDTO>(film);

            return CreatedAtAction(
                nameof(GetById),
                new { id = filmResult.Id },
                filmResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFilmDTO updateDTO)
        {
            var film = await _repo.UpdateAsync(id, updateDTO);
            if (film == null)
                return NotFound();

            return Ok(_mapper.Map<FilmDTO>(film));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var film = await _repo.DeleteAsync(id);
            if (film == null)
                return NotFound();

            return NoContent(); // deletion already handled in repo
        }
    }
}
