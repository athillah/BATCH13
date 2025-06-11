using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}