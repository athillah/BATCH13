using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilmAPI.Data;
using FilmAPI.Models;

namespace FilmAPI.Reposiotories
{
    public interface IReviewRepository
    {
        Task<Review> CreateAsync(Review reviewModel);
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetAllAsync();
        Task<Review?> DeleteAsync(int id);
    }

    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDBContext _context;

        public ReviewRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateAsync(Review reviewModel)
        {
            await _context.Reviews.AddAsync(reviewModel);
            await _context.SaveChangesAsync();
            return reviewModel;
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
    }
}
