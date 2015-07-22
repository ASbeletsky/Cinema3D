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
        IEnumerable<userclaims> UserClaims { get; }
        IEnumerable<Download> Downloads { get; }
        IEnumerable<Genre> Genres { get; }
        IEnumerable<ProducerStaff> ProducerStaff { get; }
        IEnumerable<Desctiption> Desctiption { get; }
        IEnumerable<Country> Countries { get; }
        IEnumerable<Company> Companies { get; }
        IEnumerable<FavoriteFilm> FavoriteFilms { get; }
        IEnumerable<AdditionData> AdditionData { get; }
        IEnumerable<Comment> Comments { get; }
        IEnumerable<Rating> Rating { get; }
        bool AddFavoriteFilm(string user_id, int film_id);
        bool RemoveFavoriteFilm(string user_id, int film_id);
        void AddNewFilm(Film film, Desctiption description, ProducerStaff staff);
        void EditFilm(Film film, Entities.Desctiption description, Entities.ProducerStaff staff);
        void RemoveFilm(int id);
        void AddNewGenre(string name);
        bool UpdateGenre(int id, string name);
        bool AddDownload(string user_id, int film_id);
        bool RemoveGenre(int id);
        void AddNewCompany(string name);
        bool UpdateCompany(int id, string name);
        bool RemoveCompany(int id);
        void AddNewCountry(string name);
        bool UpdateCountry(int id, string name);
        bool RemoveCountry(int id);
        void UpdateClaim(string claimType, string claimValue, string user_id);
        bool AddComment(string user_id, int film_id, string message);
        bool EditComment(string user_id, int film_id, string message);
        bool RemoveComment(string user_id, int film_id);
        bool LikeComment(int comment_id);
        bool UnlikeComment(int comment_id);
        bool AddFilmRate(string user_id, int film_id, sbyte rate);
        bool EditFilmRate(string user_id, int film_id, sbyte rate);
    }
}
