using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using FilmAPI.Data;
using Microsoft.EntityFrameworkCore;
using FilmAPI.DTOs;


namespace FilmAPI.Reposiotories
{
    public class FilmRepository : IFilmRepository
    {
        private AppDBContext _context;

        public FilmRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Film> CreateAsync(Film filmModel)
        {
            await _context.Films.AddAsync(filmModel);
            await _context.SaveChangesAsync();

            return filmModel;
        }

        public async Task<Film> DeleteAsync(int id)
        {
            var filmModel = await _context.Films.FirstOrDefaultAsync(
                f => f.Id == id);

            if (filmModel == null)
                return null;

            _context.Films.Remove(filmModel);
            await _context.SaveChangesAsync();

            return filmModel;
        }

        public async Task<List<Film>> GetAllAsync()
        {
            return await _context.Films.Include(
                r => r.Reviews).ToListAsync();
        }

        public async Task<Film?> GetByIdAsync(int id)
        {
            return await _context.Films.Include(
                r => r.Reviews).FirstOrDefaultAsync(
                    i => i.Id == id);
        }

        public async Task<Film?> UpdateAsync(int id, UpdateFilmRequestDTO filmDTO)
        {
            var filmModel = await _context.Films.FirstOrDefaultAsync(
                f => f.Id == id);

            if (filmModel == null)
                return null;

            filmModel.Title = filmDTO.Title
                           ?? filmModel.Title;
            filmModel.Year = filmDTO.Year
                          ?? filmModel.Year;
            filmModel.Director = filmDTO.Director
                              ?? filmModel.Director;

            await _context.SaveChangesAsync();

            return filmModel;
        }
    }
}