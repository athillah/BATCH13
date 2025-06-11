using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmConsole.Interfaces
{
    public interface ICinephile
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<IFilm> Favorites { get; set; }
    }
}