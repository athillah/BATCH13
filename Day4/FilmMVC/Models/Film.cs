using System;
using System.ComponentModel.DataAnnotations;

namespace FilmMVC.Models {
    public class Film {
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Director { get; set; }
    [Required]
    public float? Rate { get; set; }
    }
}