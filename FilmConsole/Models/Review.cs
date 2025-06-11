using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmConsole.Interfaces;

namespace FilmConsole.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Release { get; set; } = DateTime.Now;
        public float Rate { get; set; }

        public IFilm? Film { get; set; }
        public ICinephile? Author { get; set; }
    }
}