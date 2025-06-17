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
            // MODEL
            CreateMap<Film, FilmDTO>()
                // Likes property uses int; set it from the count of users who liked the film
                .ForMember(dest => dest.Likes,
                           opt => opt.MapFrom(src => src.LikedByUsers.Count))
                // Map list of user IDs from users who liked the film
                .ForMember(dest => dest.LikedByUsers,
                           opt => opt.MapFrom(src => src.LikedByUsers.Select(u => u.Id)))
                // Map reviews collection
                .ForMember(dest => dest.Reviews,
                           opt => opt.MapFrom(src => src.Reviews));

            CreateMap<Review, ReviewOnFilmDTO>();


            CreateMap<CreateFilmDTO, Film>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ensure ID is not mapped
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.LikedByUsers, opt => opt.Ignore());


            // REVIEW
            // Entity → DTO
            CreateMap<Review, ReviewDTO>();

            CreateMap<Review, ReviewOnFilmDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(
                    src => src.UserId));

            CreateMap<Review, ReviewOnUserDTO>()
                .ForMember(dest => dest.FilmId, opt => opt.MapFrom(
                    src => src.FilmId));

            // DTO → Entity
            CreateMap<CreateReviewDTO, Review>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(
                    src => src.CreatedOn));

            CreateMap<UpdateReviewDTO, Review>()
                .ForAllMembers(opt => opt.Condition((
                    src, dest, srcMember) => srcMember != null));
            // only map non-null values
        }
    }
}
