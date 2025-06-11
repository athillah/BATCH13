using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FilmAPI.Models;
using FilmAPI.Data;
using FilmAPI.Dtos;
using AutoMapper;
using FilmAPI.Dtos.Film;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("filmapi/film")]
    public class FilmController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        public FilmController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var films = _context.Films.ToList();
            var filmDtos = _mapper.Map<List<FilmDto>>(films);
            return Ok(films);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var film = _context.Films.Find(id);
            return film != null ? Ok(film) : NotFound();
        }

    }
}