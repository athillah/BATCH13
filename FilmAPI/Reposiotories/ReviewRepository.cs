using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilmAPI.Data;
using FilmAPI.Models;
using FilmAPI.DTOs;

namespace FilmAPI.Reposiotories
{
    public interface IReviewRepository
    {
        Task<Review> CreateAsync(Review review);
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetAllAsync();
        Task<Review?> DeleteAsync(int id);
        Task<Review?> UpdateAsync(int id, UpdateReviewDTO DTO);
    }

    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDBContext _context;

        public ReviewRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return review;
        }

        public async Task<Review?> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null)
                return null;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return review;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<Review?> UpdateAsync(int id, UpdateReviewDTO DTO)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return null;

            review.Content = DTO.Content ?? review.Content;
            review.Rate = DTO.Rate ?? review.Rate;

            await _context.SaveChangesAsync();

            return review;
        }
    }
}
