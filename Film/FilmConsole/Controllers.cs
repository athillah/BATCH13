using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmConsole
{
    public class FilmController
    {
        private FilmService _service;
        public FilmController(FilmService service) => _service = service;
    }
}