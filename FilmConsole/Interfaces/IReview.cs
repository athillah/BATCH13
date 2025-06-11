using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmConsole.Interfaces
{
    public interface IReview
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Release { get; set; }
        public float Rate { get; set; }

        public IFilm Film { get; set; }
        public ICinephile Author { get; set; }
    }
}