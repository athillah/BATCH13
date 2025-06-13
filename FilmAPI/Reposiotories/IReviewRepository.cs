using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;

namespace FilmAPI.Reposiotories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        // Task<Review> CreateAsync(Review ReviewModel);
        // // Task<Review?> UpdateAsync(int i, UpdateReviewRequestDTO ReviewDTO);
        // Task<Review> DeleteAsync(int id);
    }
}