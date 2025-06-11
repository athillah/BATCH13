using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmConsole
{
    public interface IRepository<T>
    {
        void Post(T item);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Put(T item);
        void Delete(T item);
    }

    public interface IFilm
    {
        int Id { get; set; }
        string Title { get; set; }
    }
}