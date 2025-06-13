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
        private IReviewRepository _repo;
        public ReviewController(IReviewRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _repo.GetAllAsync();
            var reviewDTO = reviews.Select(
                r => r.ToReviewDTO());

            return Ok(reviewDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _repo.GetByIdAsync(id);

            if (review == null)
                return NotFound();

            return Ok(review.ToReviewDTO());
        }

        
    }
}