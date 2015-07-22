using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Entities;
using VideoLib.Domian.Abstract;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Security.Claims;
using VideoLib.Domian.Entities.AuthEntities;

namespace VideoLib.Domian.Concrete
{
    public class VideoLibRepository : IVideoLibRepository
    {
        private readonly VideoLibMainContext _context = new VideoLibMainContext();

        public static IVideoLibRepository GetRepository()
        {
            return new VideoLibRepository(); 
        }
        public void Dispose()
        {
        }

                                       #region Tables
        public IEnumerable<Film> Films 
        {
            get { return _context.Films as IEnumerable<Film>; }
        }
        public IEnumerable<users> Users
        {
            get { return _context.users as IEnumerable<users>; }
        }
        public IEnumerable<Download> Downloads 
        {
            get { return _context.Downloads as IEnumerable<Download>; }
        }
        public IEnumerable<Genre> Genres
        {
            get { return _context.Genres as IEnumerable<Genre>; }
        }
        public IEnumerable<ProducerStaff> ProducerStaff
            {
                get { return _context.ProducerStaff as IEnumerable<ProducerStaff>; }
        }
        public IEnumerable<Desctiption> Desctiption 
        {
            get { return _context.Desctiption as IEnumerable<Desctiption>; }
        }
        public IEnumerable<Country> Countries
        {
            get { return _context.Countries as IEnumerable<Country>; }
        }
        public IEnumerable<Company> Companies
        {
            get { return _context.Companies as IEnumerable<Company>; }
        }
        public IEnumerable<FavoriteFilm> FavoriteFilms
        {
            get { return _context.FavoriteFilms as IEnumerable<FavoriteFilm>; }
        }

        public IEnumerable<AdditionData> AdditionData
        {
            get { return _context.AdditionData as IEnumerable<AdditionData>; }
        }
        public IEnumerable<Comment> Comments
        {
            get { return _context.Comments as IEnumerable<Comment>; }
        }
        public IEnumerable<Rating> Rating
        {
            get { return _context.Rating as IEnumerable<Rating>; }
        }
        public IEnumerable<userclaims> UserClaims
        {
            get { return _context.userclaims as IEnumerable<userclaims>; }
        }

                                        #endregion 

                               #region Favorite Films Operations

        public bool AddFavoriteFilm(string user_id, int film_id)
        {            
            var user = _context.users.Find(user_id);
            if(user != null)
            {
                FavoriteFilm newFavorite = new FavoriteFilm()
                {
                    Film_Id = film_id,
                    User_Id = user_id,
                    AdditionTime = DateTime.Now
                };
                _context.FavoriteFilms.Add(newFavorite);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        bool IVideoLibRepository.RemoveFavoriteFilm(string user_id, int film_id)
        {
            FavoriteFilm favoriteToRomove = _context.FavoriteFilms.Find(user_id, film_id);
            if (favoriteToRomove != null)
            {

                _context.FavoriteFilms.Remove(favoriteToRomove);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
                                        #endregion

        public bool AddDownload(string user_id, int film_id)
        {
            var download = _context.Downloads.Find(user_id, film_id);
            if (download == null)
            {
                Download newDownload = new Download()
                {
                    Film_Id = film_id,
                    User_Id = user_id,
                    DownloadTime = DateTime.Now
                };
                _context.Downloads.Add(newDownload);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

                                       #region Film CRUD
        public void AddNewFilm(Film film, Desctiption description, ProducerStaff staff)
        {
            Film filmToAdd = new Film();
            filmToAdd = film;
            filmToAdd.desctiption = description;
            filmToAdd.producerstaff = staff;
            _context.Films.Add(filmToAdd);
            _context.Entry<Film>(filmToAdd).State = EntityState.Added;
            _context.SaveChanges();
        }

        public void EditFilm(Film film, Desctiption description, ProducerStaff staff)
        {
            Film editFilm = _context.Films.Find(film.Id);
            _context.Entry<Film>(editFilm).CurrentValues.SetValues(film);
            Desctiption editDescr = _context.Desctiption.Where(d => d.Film_Id == film.Id).FirstOrDefault();
            description.Film_Id = editDescr.Film_Id;
            _context.Entry<Desctiption>(editDescr).CurrentValues.SetValues(description);
            ProducerStaff editStaff = _context.ProducerStaff.Where(s => s.Film_Id == film.Id).FirstOrDefault();
            staff.Film_Id = editStaff.Film_Id;
            _context.Entry<ProducerStaff>(editStaff).CurrentValues.SetValues(staff);
            _context.SaveChanges();
        }

        public void RemoveFilm(int id)
        {
            Film filmToRemove = _context.Films.FirstOrDefault(f => f.Id == id);
            if (filmToRemove != null)
            {
                _context.Films.Remove(filmToRemove);
                _context.Entry<Film>(filmToRemove).State = EntityState.Deleted;
                try
                {
                    _context.SaveChanges();
                }
                catch(OptimisticConcurrencyException)
                {
                    ((IObjectContextAdapter) _context).ObjectContext.Refresh(RefreshMode.ClientWins, _context.Films);

                    _context.SaveChanges();
                }

            }
        }

                                                #endregion

                                      #region Genre CRUD

        public void AddNewGenre(string name)
        {
            Genre newGenre = new Genre {Name = name};
            _context.Genres.Add(newGenre);
            _context.SaveChanges();
        }

        public bool UpdateGenre(int id, string name)
        {
            Genre updateGenre = _context.Genres.Find(id);
            if(updateGenre != null)
            {
                updateGenre.Name = name;
                _context.Entry<Genre>(updateGenre).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveGenre(int id)
        {
            Genre genreToRemove = _context.Genres.Find(id);
            if(genreToRemove != null)
            {
                _context.Genres.Remove(genreToRemove);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

                                                #endregion

                                     #region Company CRUD
        public void AddNewCompany(string name)
        {
            Company newCompany = new Company { Name = name };
            _context.Companies.Add(newCompany);
            _context.SaveChanges();
        }

        public bool UpdateCompany(int id, string name)
        {
            Company updateCompany = _context.Companies.Find(id);
            if (updateCompany != null)
            {
                updateCompany.Name = name;
                _context.Entry<Company>(updateCompany).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveCompany(int id)
        {
            Company companyToRemove = _context.Companies.Find(id);
            if (companyToRemove != null)
            {
                _context.Companies.Remove(companyToRemove);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

                                                #endregion

                                      #region County CRUD

        public void AddNewCountry(string name)
        {
            Country newCountry = new Country { Name = name };
            _context.Countries.Add(newCountry);
            _context.SaveChanges();
        }

        public bool UpdateCountry(int id, string name)
        {
            Country updateCountry = _context.Countries.Find(id);
            if (updateCountry != null)
            {
                updateCountry.Name = name;
                _context.Entry<Country>(updateCountry).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveCountry(int id)
        {
            Country countryToRemove = _context.Countries.Find(id);
            if (countryToRemove != null)
            {
                _context.Countries.Remove(countryToRemove);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

                                            #endregion
        public void UpdateClaim(string claimType,string claimValue, string user_id)
        {
            var claimToUpdate = _context.userclaims.FirstOrDefault(claim=> claim.ClaimType == claimType && claim.UserId == user_id);
            claimToUpdate.ClaimValue = claimValue;
            _context.Entry<userclaims>(claimToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }

                                     #region Comment CRUD
        public bool AddComment(string user_id, int film_id, string message)
        {
            var user = _context.users.Find(user_id);
            try
            {
                Comment newComment = new Comment()
                {
                    Film_Id = film_id,
                    User_Id = user_id,
                    Text = message,
                    AdditionTime = DateTime.Now
                };
                _context.Comments.Add(newComment);
                _context.SaveChanges();
                return true;
            }
            catch { return false; }            
        }

        public bool EditComment(string user_id, int film_id, string message)
        {
            try
            {
                Comment currentComment = _context.Comments.FirstOrDefault(comment => comment.User_Id == user_id
                                                                 && comment.Film_Id== film_id);
                currentComment.Text = message;
                currentComment.AdditionTime = DateTime.Now;

                _context.Entry<Comment>(currentComment).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch { return false; }           
        }
        public bool RemoveComment(string user_id, int film_id)
        {
            try
            {
                var removingComment = _context.Comments.FirstOrDefault(comment => comment.User_Id == user_id
                                                                 && comment.Film_Id== film_id);
                _context.Comments.Remove(removingComment);
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

                                     #endregion

                                 #region Comments Like / Unlike

        public bool LikeComment(int comment_id)
        {
            try
            {
                Comment currentComment = _context.Comments.Find(comment_id);
                currentComment.Rating++;
                _context.Entry<Comment>(currentComment).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool UnlikeComment(int comment_id)
        {
            try
            {
                Comment currentComment = _context.Comments.Find(comment_id);
                currentComment.Rating--;
                _context.Entry<Comment>(currentComment).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
        }
                                                        #endregion

                                   #region Rating Operations
        public bool AddFilmRate(string user_id, int film_id, sbyte rate)
        {
            try
            {
                Rating newRate = new Rating
                {
                    Film_Id = film_id,
                    User_Id = user_id,
                    RatingValue = rate
                };
                _context.Rating.Add(newRate);
                _context.SaveChanges();
                SetNewRatingToFilm(2, film_id);
                return true;
            }
            catch { return false; }
        }

        public bool EditFilmRate(string user_id, int film_id, sbyte rate)
        {
            try
            {
                Rating currentRate;
                try { currentRate = _context.Rating.Find(user_id, film_id); }
                catch { currentRate = _context.Rating.FirstOrDefault(comment => comment.User_Id == user_id && comment.Film_Id == film_id); }
                    
                currentRate.RatingValue = rate;
                _context.Entry<Rating>(currentRate).State = EntityState.Modified;
                _context.SaveChanges();
                SetNewRatingToFilm(2, film_id);
                return true;
            }
            catch { return false; }
        }

        public void SetNewRatingToFilm(int n, int film_id)
        {
            int all_votes = _context.Rating.Where(r => r.Film_Id == film_id && r.RatingValue > 0).Count();
            int rating_votes_sum = _context.Rating.Where(r => r.Film_Id == film_id && r.RatingValue > 0)
                                                  .Sum(r => r.RatingValue);
            float new_rating;
            if(all_votes < 10)
                new_rating = (float) rating_votes_sum / all_votes;
            else
                new_rating = (rating_votes_sum + 3 * n) / (all_votes + n);

            Film currect = _context.Films.Find(film_id);
            currect.RatingValue = new_rating;
            _context.Entry<Film>(currect).State = EntityState.Modified;
            _context.SaveChanges();
        }
            
                                                #endregion



    }
}
