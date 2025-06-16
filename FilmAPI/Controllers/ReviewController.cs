using Microsoft.AspNetCore.Mvc;
using FilmAPI.DTOs;
using FilmAPI.Reposiotories;
using FilmAPI.Models;
using AutoMapper;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IFilmRepository _filmRepo;
        private readonly IMapper _mapper;

        public ReviewController(
            IReviewRepository reviewRepo,
            IFilmRepository filmRepo,
            IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _filmRepo = filmRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewRepo.GetAllAsync();
            var reviewDTOs = reviews.Select(r => _mapper.Map<ReviewDTO>(r));
            return Ok(reviewDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewRepo.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(_mapper.Map<ReviewDTO>(review));
        }

        [HttpPost("{filmId}")]
        public async Task<IActionResult> Create([FromRoute] int filmId, [FromBody] CreateReviewDTO reviewDTO)
        {
            if (!await _filmRepo.Check(filmId))
                return BadRequest("Film doesn't exist");

            var review = _mapper.Map<Review>(reviewDTO);
            review.FilmId = filmId;

            await _reviewRepo.CreateAsync(review);

            var result = _mapper.Map<ReviewDTO>(review);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}
