using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FilmAPI.DTOs;

namespace FilmAPI.Mappers
{
    public static class FilmMappers
    {
        public static FilmDTO ToFilmDTO(this Film filmModel)
        {
            return new FilmDTO
            {
                Id = filmModel.Id,
                Year = filmModel.Year,
                Title = filmModel.Title,
                Director = filmModel.Director,
                Reviews = filmModel.Reviews.Select(
                    r => r.ToReviewDTO()).ToList()
            };
        }
        public static Film ToFilmFromCreateDTO(this CreateFilmRequestDTO filmDTO)
        {
            return new Film
            {
                Year = filmDTO.Year,
                Title = filmDTO.Title,
                Director = filmDTO.Director
            };
        }
    }
}