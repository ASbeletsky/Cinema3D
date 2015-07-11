using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Entities;

namespace VideoLib.Domian.Abstract 
{
    public interface IVideoLibRepository : IDisposable
    {
        IEnumerable<Film> Films { get; }
        IEnumerable<users> Users { get; }
        IEnumerable<Download> Downloads { get; }
        IEnumerable<Genre> Genres { get; }
        IEnumerable<ProducerStaff> ProducerStaff { get; }
        IEnumerable<Desctiption> Desctiption { get; }
        IEnumerable<Country> Countries { get; }
        IEnumerable<Company> Companies { get; }
        IEnumerable<FavoriteFilm> FavoriteFilms { get; }
        IEnumerable<AdditionData> AdditionData { get; }
        bool AddFavoriteFilm(int user_id, int film_id);
        bool RemoveFavoriteFilm(int user_id, int film_id);
        void AddNewFilm(Film film, Desctiption description, ProducerStaff staff);

        void EditFilm(Film film, Entities.Desctiption description, Entities.ProducerStaff staff);

        void RemoveFilm(int id);
        void AddNewGenre(string name);

        bool UpdateGenre(int id, string name);

        bool AddDownload(int user_id, int film_id);

        bool RemoveGenre(int id);

        void AddNewCompany(string name);

        bool UpdateCompany(int id, string name);

        bool RemoveCompany(int id);



        void AddNewCountry(string name);

        bool UpdateCountry(int id, string name);

        bool RemoveCountry(int id);

        void UpdateClaim(string claimType,string claimValue, int user_id);
    
        int GetIdByParseId(string object_id);}
}
