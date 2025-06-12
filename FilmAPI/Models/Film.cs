using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmAPI.Models
{
    public class Film
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Director { get; set; } = string.Empty;

        public List<Review> Reviews = new List<Review>();
    }
}