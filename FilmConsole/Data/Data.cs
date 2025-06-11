using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmConsole.Interfaces;

namespace FilmConsole.Data
{
    public class Database
    {
        public List<ICinephile> Cinephiles { get; set; } = new();
        public List<IReview> Reviews { get; set; } = new();
        public List<IFilm> Films { get; set; } = new();
    }
}