using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FilmAPI.DTOs;
using FilmAPI.Reposiotories;
using FilmAPI.Models;
using AutoMapper;
using System.Security.Claims;

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
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int filmId, [FromBody] CreateReviewDTO reviewDTO)
        {
            var userId = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (!await _filmRepo.Check(filmId))
                return BadRequest("Film doesn't exist");

            var review = _mapper.Map<Review>(reviewDTO);

            review.UserId = userId;
            review.FilmId = filmId;

            await _reviewRepo.CreateAsync(review);

            var result = _mapper.Map<ReviewDTO>(review);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var review = await _reviewRepo.DeleteAsync(id);

            if (review == null)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(
    [FromRoute] int id,
    [FromBody] UpdateReviewDTO updateDto)
        {
            // 1️⃣ Fetch the existing review
            var existingReview = await _reviewRepo.GetByIdAsync(id);
            if (existingReview == null)
                return NotFound();

            // 2️⃣ Check ownership
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || existingReview.UserId != userId)
                return Forbid();

            // 3️⃣ Apply update
            var updated = await _reviewRepo.UpdateAsync(id, updateDto);
            if (updated == null)
                return StatusCode(500, "Failed to update review");

            // 4️⃣ Return the updated DTO
            var resultDto = _mapper.Map<ReviewDTO>(updated);
            return Ok(resultDto);
        }
    }
}
