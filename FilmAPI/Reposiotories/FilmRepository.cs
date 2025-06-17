using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using FilmAPI.Models;
using FilmAPI.Data;
using Microsoft.EntityFrameworkCore;
using FilmAPI.DTOs;
using AutoMapper;

namespace FilmAPI.Reposiotories
{
    public interface IFilmRepository
    {
        Task<bool> Check(int id);
        Task<List<Film>> GetAllAsync();
        Task<Film?> GetByIdAsync(int id);
        Task<Film?> DeleteAsync(int id);
        Task<Film> CreateAsync(CreateFilmDTO filmDTO);
        Task<Film?> UpdateAsync(int id, UpdateFilmDTO filmDTO);
        Task<Film?> UpdateLikeAsync(Film film);
        Task<bool> AnyAsync(Expression<Func<Film, bool>> predicate, CancellationToken ct = default);
    }

    public class FilmRepository : IFilmRepository
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public FilmRepository(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Film> CreateAsync(CreateFilmDTO filmDTO)
        {
            var film = _mapper.Map<Film>(filmDTO);
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();

            return film;
        }

        public async Task<Film?> DeleteAsync(int id)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == id);

            if (film == null)
                return null;

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return film;
        }

        public async Task<List<Film>> GetAllAsync()
        {
            return await _context.Films
                .Include(f => f.LikedByUsers)
                .Include(f => f.Reviews)
                .ToListAsync();
        }

        public async Task<Film?> GetByIdAsync(int id)
        {
            return await _context.Films
                .Include(f => f.LikedByUsers)
                .Include(f => f.Reviews)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Film?> UpdateAsync(int id, UpdateFilmDTO filmDTO)
        {
            var film = await _context.Films.FirstOrDefaultAsync(f => f.Id == id);

            if (film == null)
                return null;

            // Apply partial updates
            film.Title = filmDTO.Title ?? film.Title;
            film.Year = filmDTO.Year ?? film.Year;
            film.Director = filmDTO.Director ?? film.Director;

            await _context.SaveChangesAsync();

            return film;
        }

        public Task<bool> Check(int id)
        {
            return _context.Films.AnyAsync(f => f.Id == id);
        }

        public Task<bool> AnyAsync(Expression<Func<Film, bool>> predicate, CancellationToken ct = default)
        {
            return _context.Films.AnyAsync(predicate, ct);
        }

        public async Task<Film?> UpdateLikeAsync(Film film)
        {
            _context.Films.Update(film);
            await _context.SaveChangesAsync();

            return film;
        }
    }
}
