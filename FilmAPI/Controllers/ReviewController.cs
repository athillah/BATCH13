using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FilmAPI.Models;
using FilmAPI.DTOs;
using FilmAPI.Data;
using FilmAPI.Reposiotories;
using FilmAPI.Mappers;

namespace FilmAPI.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private IReviewRepository _reviewRepo;
        private IFilmRepository _filmRepo;
        public ReviewController(
            IReviewRepository reviewRepo, IFilmRepository filmRepo)
        {
            _reviewRepo = reviewRepo;
            _filmRepo = filmRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewRepo.GetAllAsync();
            var reviewDTO = reviews.Select(
                r => r.ToReviewDTO());

            return Ok(reviewDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewRepo.GetByIdAsync(id);

            if (review == null)
                return NotFound();

            return Ok(review.ToReviewDTO());
        }

        [HttpPost("{filmId}")]
        public async Task<IActionResult> Create([FromRoute] int filmId, CreateReviewDTO reviewDTO)
        {
            if (!await _filmRepo.Check(filmId))
                return BadRequest("Film does'nt exist");

            var review = reviewDTO
        }
    }
}