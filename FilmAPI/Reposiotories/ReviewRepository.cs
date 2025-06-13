using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilmAPI.Data;
using FilmAPI.Models;

namespace FilmAPI.Reposiotories
{
    public class ReviewRepository : IReviewRepository
    {
        private AppDBContext _context;
        public ReviewRepository(AppDBContext context)
        {
            _context = context;
        }

        public Task<Review> CreateAsync(Review ReviewModel)
        {
            throw new NotImplementedException();
        }

        public Task<Review> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        // public Task<Review?> UpdateAsync(int i, UpdateReviewRequestDTO ReviewDTO)
        // {
        //     throw new NotImplementedException();
        // }
    }
}