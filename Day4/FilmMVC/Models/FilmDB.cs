using Microsoft.EntityFrameworkCore;
namespace FilmMVC.Models;

public class FilmDB : DbContext {
    public DbSet<Film> Films { get; set; }
    public FilmDB(DbContextOptions<FilmDB> options) : base(options) {}
}
