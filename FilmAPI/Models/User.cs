using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string UserName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string FullName => $"{FirstName} {LastName}";

        public List<Film>? FavoriteFilms { get; set; } = new List<Film>();
        public List<Review>? Reviews { get; set; } = new List<Review>();

    }
}