using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmConsole.Interfaces;

namespace FilmConsole.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string Director { get; set; } = string.Empty;

        public List<IReview> Reviews { get; set; } = new();
        public float? AvgRate { get; set; }
    }
}