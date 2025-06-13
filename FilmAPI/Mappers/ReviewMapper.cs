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
        public static ReviewDTO ToReviewDTO(this Review reviewModel)
        {
            return new ReviewDTO
            {
                Id = reviewModel.Id,
                Content = reviewModel.Content,
                Rate = reviewModel.Rate,
                CreatedOn = reviewModel.CreatedOn,
                FilmId = reviewModel.FilmId
            };
        }
    }
}