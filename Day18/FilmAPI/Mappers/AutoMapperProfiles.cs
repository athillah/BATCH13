using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FilmAPI.Dtos.Film;
using FilmAPI.Models;

namespace FilmAPI.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Film, FilmDto>().ReverseMap();
        }
    }
}