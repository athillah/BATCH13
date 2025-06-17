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
            public int Likes;
            public List<ReviewOnFilmDTO> Reviews { get; set; } = new();
            public List<string> LikedByUsers { get; set; } = new List<string>();
        }
        public class CreateFilmDTO
        {
            public int? Year { get; set; }
            public string Title { get; set; } = string.Empty;
            public string? Director { get; set; } = string.Empty;
        }
        public class UpdateFilmDTO
        {
            public int? Year { get; set; }
            public string? Title { get; set; } = string.Empty;
            public string? Director { get; set; } = string.Empty;
        }

        public class ToggleLikeFilmDTO
        {
            public int Likes;
            public List<string> LikedByUsers { get; set; } = new List<string>();
        }
    }
