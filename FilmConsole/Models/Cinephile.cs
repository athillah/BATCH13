using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmConsole.Interfaces;

namespace FilmConsole.Models
{
    public class Cinephile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<IFilm> Favorites { get; set; } = new();
    }
}