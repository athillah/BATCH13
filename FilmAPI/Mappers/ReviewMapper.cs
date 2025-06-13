using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FilmAPI.DTOs;

namespace FilmAPI.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewDTO ToReviewDTO(this Review review)
        {
            return new ReviewDTO
            {
                Id = review.Id,
                Content = review.Content,
                Rate = review.Rate,
                CreatedOn = review.CreatedOn,
                FilmId = review.FilmId
            };
        }
        public static ReviewOnFilmDTO ToReviewOnFilmDTO(this Review review)
        {
            return new ReviewOnFilmDTO
            {
                Id = review.Id,
                Content = review.Content,
                Rate = review.Rate,
                CreatedOn = review.CreatedOn,
            };
        }

        public static Review ToReviewFromCreate (this CreateReviewDTO review, int filmId)
        {
            return new Review
            {
                Content = review.Content,
                Rate = review.Rate,
                FilmId = filmId
            };
        }
    }
}