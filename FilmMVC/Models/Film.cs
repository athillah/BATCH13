using System;
using System.ComponentModel.DataAnnotations;

namespace FilmMVC.Models {
    public class Film {
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
    [Display(Name = "Release Year")]
    public int Year { get; set; }
    public string Director { get; set; }
    [Range(0, 10, ErrorMessage = "Rate must be between 0 and 10")]
    [Display(Name = "Rate (0-10)")]
    public float? Rate { get; set; }
    }
}