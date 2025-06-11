using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmConsole
{
    public class Database
    {
        public List<IFilm> Films { get; set; } = new();
    }
}