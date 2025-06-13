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
        public static FilmDTO ToFilmDTO(this Film film)
        {
            return new FilmDTO
            {
                Id = film.Id,
                Year = film.Year,
                Title = film.Title,
                Director = film.Director,
                Reviews = film.Reviews.Select(
                    r => new ReviewOnFilmDTO
                    {
                        Id = r.Id,
                        Content = r.Content,
                        Rate = r.Rate,
                        CreatedOn = r.CreatedOn
                    }).ToList()
            };
        }
        public static Film ToFilmFromCreate(this CreateFilmRequestDTO filmDTO)
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