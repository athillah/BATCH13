using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmConsole
{
    public class FilmService
    {
        private IRepository<IFilm> _repo;
        public FilmService(IRepository<IFilm> repo) => _repo = repo;


    }
}