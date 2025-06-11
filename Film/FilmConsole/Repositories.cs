using System.Collections.Generic;

namespace FilmConsole
{
    public class FilmRepository : IRepository<IFilm>
    {
        private Database _db;
        public FilmRepository(Database db) => _db = db;
        
        public void Post(IFilm item)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IFilm> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IFilm GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Put(IFilm item)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(IFilm item)
        {
            throw new System.NotImplementedException();
        }
    }
}