using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmConsole.Interfaces
{
    public interface IFilm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }

        public List<IReview> Reviews { get; set; }
        public float AvgRate { get; set; }
    }
}