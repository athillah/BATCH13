using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FilmAPI.DTOs;

namespace FilmAPI.Reposiotories
{
    public interface IFilmRepository
    {
        Task<List<Film>> GetAllAsync();
        Task<Film?> GetByIdAsync(int id);
        Task<Film> CreateAsync(Film filmModel);
        Task<Film?> UpdateAsync(int i, UpdateFilmRequestDTO filmDTO);
        Task<Film> DeleteAsync(int id);
    }
}