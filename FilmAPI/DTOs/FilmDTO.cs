using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmAPI.DTOs
{
    public class FilmDTO
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Director { get; set; } = string.Empty;
        public List<ReviewDTO>? Reviews { get; set; }
    }
    public class CreateFilmRequestDTO
    {
        public int? Year { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Director { get; set; } = string.Empty;
    }
    public class UpdateFilmRequestDTO
    {
        public int? Year { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Director { get; set; } = string.Empty;
    }
}
