using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;

namespace FilmAPI.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public float? Rate { get; set; } = null;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int FilmId { get; set; }
    }
}