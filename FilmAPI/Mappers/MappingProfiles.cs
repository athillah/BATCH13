using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FilmAPI.DTOs;
using FilmAPI.Models;

namespace FilmAPI.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // <FILM>
            // Film → FilmDTO
            CreateMap<Film, FilmDTO>();

            // Review → ReviewOnFilmDTO
            CreateMap<Review, ReviewOnFilmDTO>();

            // CreateFilmDTO → Film
            CreateMap<CreateFilmDTO, Film>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ensure ID is not mapped
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.LikedByUsers, opt => opt.Ignore());


            // <REVIEW>
            // Review ↔ DTO
            CreateMap<Review, ReviewDTO>();
            CreateMap<Review, ReviewOnFilmDTO>();

            // CreateReviewDTO → Review
            CreateMap<CreateReviewDTO, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FilmId, opt => opt.Ignore()) // you still need to set this manually
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // unless handled from auth context
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
