using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLib.Domian.Concrete;
using VideoLib.Domian.Entities;
using VideoLib.Domian.Abstract;
using VideoLib.WebUI.Models.VideoLib;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Newtonsoft.Json;

namespace VideoLib.WebUI.Controllers
{
    public class VideoLibController : Controller
    {
        private IVideoLibRepository _repository;
        private IVideoLibRepository Repository
        {
            get { return _repository ?? HttpContext.GetOwinContext().Get<IVideoLibRepository>();}
        }        

        //GET: films
        public JsonResult AllFilmCollection()
        {
            var Films = GetAllFilms(); 
            if(Films != null)
            {
                var orderedFilms = Films.OrderBy(film => film.AdditionDate);
                return Json(orderedFilms, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult { 
                Data = new { ErrorId = 1, ErrorMessege = "Film collection is empty" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //POST:VideoLib/UserDownloadFilmCollection
        public JsonResult UserDownloadFilmCollection(int user_id)
        {

            if (user_id > 0)
            {
                var userDownloads = Repository.Users.First(user => user.Id == user_id).download;
                if(userDownloads != null)
                {
                    var userFilms = (from film in userDownloads
                                                    .Select(download => download.film)
                                     join descr in Repository.Desctiption
                                         on film.Id equals descr.Film_Id
                                     join genre in Repository.Genres
                                         on descr.Genre_Id equals genre.Id
                                     join company in Repository.Companies
                                         on descr.Company_Id equals company.Id

                                     select new FilmViewModel
                                     {
                                         Id = film.Id,
                                         Name = film.Name,
                                         ImageSmallUrl = film.ImageSmallUrl,
                                         ImageBigUrl = film.ImageBigUrl,
                                         IsFavorite = false,
                                         AdditionDate = film.AdditionDate.Value.Date.ToShortDateString(),
                                         DownloadUrl = film.DownloadUrl,
                                         Description = descr.Text,
                                         genreId = genre.Id,
                                         genreName = genre.Name,
                                         companyId = company.Id,
                                         companyName = company.Name,
                                     }).OrderBy(film => film.AdditionDate).ToList();

                    return Json(userFilms, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return new JsonResult
                    { 
                        Data = new { ErrorId = 2, ErrorMessege = "User doesn't have downloads" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet                    
                    };
                }
            }
            return new JsonResult
            { 
                Data = new { ErrorId = 1, ErrorMessege = string.Format("User with id {0} doesn't exist", user_id) },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };                        
        }

        //GET: films/popular       
        public JsonResult PopularFilmCollection()
        {
            var films = GetAllFilms();
            if(films != null)
            {
                var popularFilms = films.Select(film => new
                {
                    Film = film,
                    Downloads = Repository.Downloads.Count(d => d.Film_Id == film.Id)
                }).OrderByDescending(popular => popular.Downloads).ToList();
                
                return Json(popularFilms.Select(p => p.Film), JsonRequestBehavior.AllowGet);
            }
                           
             return new JsonResult
             {
                 Data = new { ErrorId = 1, ErrorMessege = "Popular film collection is empty" },
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
        }

        //POST: films/favorites        
        public JsonResult UserFavoriteFilmCollection(UserId properties)
        {
            {
                if (properties.user_id > 0)
                {
                    var userFavorite = Repository.Users.First(user => user.Id == properties.user_id).favoritefilms;
                    if (userFavorite != null)
                    {
                        var FavoriteFilms = (from film in userFavorite
                                                        .Select(favorite => favorite.film)
                                         join descr in Repository.Desctiption
                                             on film.Id equals descr.Film_Id
                                         join genre in Repository.Genres
                                             on descr.Genre_Id equals genre.Id
                                         join company in Repository.Companies
                                             on descr.Company_Id equals company.Id

                                         select new FilmViewModel
                                         {
                                             Id = film.Id,
                                             Name = film.Name,
                                             ImageSmallUrl = film.ImageSmallUrl,
                                             ImageBigUrl = film.ImageBigUrl,
                                             IsFavorite = false,
                                             AdditionDate = film.AdditionDate.Value.Date.ToShortDateString(),
                                             DownloadUrl = film.DownloadUrl,
                                             Description = descr.Text,
                                             genreId = genre.Id,
                                             genreName = genre.Name,
                                             companyId = company.Id,
                                             companyName = company.Name,
                                         }).OrderBy(film => film.AdditionDate).ToList();

                        return Json(FavoriteFilms, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return new JsonResult { Data = new { ErrorId = 2, ErrorMessege = "User doesn't have favorite films" } };
                    }
                }
                return new JsonResult { Data = new { ErrorId = 1, ErrorMessege = string.Format("User with id {0} doesn't exist", properties.user_id) } };
            }
        }


        //GET:genres
        public JsonResult GetGenres()
        {
            var genres = Repository.Genres.Select(genre => new
                {
                    Id = genre.Id,
                    Name = genre.Name
                }).ToList();
            if(genres != null)
            {
                return Json(genres, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult 
            {
                Data = new { ErrorId = 1, ErrorMessege = "Genres collection is empty" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //GET:VideoLib/GetCompanies
        public JsonResult GetCompanies()
        {
            var companies = Repository.Companies.Select(company => new
            {
                Id = company.Id,
                Name = company.Name
            }).ToList();
            return Json(companies, JsonRequestBehavior.AllowGet);
        }

        //POST:films/add_to_favorites
        public JsonResult AddFavoriteFilm(UserFilmProperties properties)
        {

            bool success = false;
            if (properties.film_id > 0 && properties.user_id > 0)
            {
                var favoritFilmsIds = Repository.FavoriteFilms
                    .Where(f => f.users_Id == properties.user_id)
                    .Select(f => f.Film_Id).ToList();
                if(!favoritFilmsIds.Contains(properties.film_id))
                {
                    success = Repository.AddFavoriteFilm(properties.user_id, properties.film_id);
                    return new JsonResult { Data = new { OK = success } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = string.Format("User with id {0} have already add film with id {1}", properties.user_id, properties.film_id)
                    }
                };
            }
            return new JsonResult {
                Data = new 
                { 
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User with id {0} or film with id {1} does not exist", properties.user_id, properties.film_id)
                },                
            };
        }

        //POST:films/remove_from_favorites
        public JsonResult RemoveFavoriteFilm(UserFilmProperties properties)
        {

            bool success = false;
            if (properties.film_id > 0 && properties.user_id > 0)
            {
                var favoritFilmsIds = Repository.FavoriteFilms
                    .Where(f => f.users_Id == properties.user_id)
                    .Select(f => f.Film_Id).ToList();
                if (favoritFilmsIds.Contains(properties.film_id))
                {
                    success = Repository.RemoveFavoriteFilm(properties.user_id, properties.film_id);
                    return new JsonResult { Data = new { OK = success } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = string.Format("User with id {0} doen't have film with id {1}", properties.user_id, properties.film_id)
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User with id {0} or film with id {1} does not exist", properties.user_id, properties.film_id)
                },
            };
        }

        //POST:films/add_download
        public JsonResult AddDownload(UserFilmProperties properties)
        {
         

            bool success = false;
            if (properties.film_id > 0 && properties.user_id > 0)
            {
                success = Repository.AddDownload(properties.user_id, properties.film_id);
            }
            return new JsonResult
            {
                Data = new { OK = success },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //POST:films/by_genre
        public JsonResult FilmCollectionByGenre(GenreJson properties)
        {
            
            
            if (properties.genre_id > 0 )
            {
                var filmsByGenre = GetFilmsByGenre(properties.genre_id);
                if(filmsByGenre != null)
                {
                    return Json(filmsByGenre, JsonRequestBehavior.AllowGet);
                }
                return new JsonResult
                {
                    Data = new
                        {
                            OK = false,
                            ErrorId = 1,
                            ErrorMessege = string.Format("Films collection by genre with id {0} is empty", properties.genre_id)
                        }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("Genre with id {0} doesn't exist ", properties.genre_id)
                }
            };
        }
        

        //POST:films/is_favorite
        [HttpPost]
        public JsonResult IsFavorite(UserFilmProperties properties)
        {
            bool contains = false;
            if (properties.film_id > 0 && properties.user_id > 0)
            {
                var userFavoriteFilms = Repository.FavoriteFilms.Where(f => f.users_Id == properties.user_id).ToList();
                if(userFavoriteFilms != null)
                {
                    contains = userFavoriteFilms.Select(film => film.Film_Id).Contains(properties.film_id);
                    return Json(new { OK = true, Contains = contains });
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = string.Format("User with id {0} doesn't have favorites films ", properties.user_id)
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User with id {0} or film with id {1} does not exist", properties.user_id, properties.film_id)
                }
            };
        }

        private IEnumerable<FilmViewModel> GetAllFilms()
        {
            var allFilms = (from film in Repository.Films
                            join descr in Repository.Desctiption
                                on film.Id equals descr.Film_Id
                            join genre in Repository.Genres
                                on descr.Genre_Id equals genre.Id
                            join company in Repository.Companies
                                on descr.Company_Id equals company.Id
                            select new FilmViewModel
                            {
                                Id = film.Id,
                                Name = film.Name,
                                ImageSmallUrl = film.ImageSmallUrl,
                                ImageBigUrl = film.ImageBigUrl,
                                IsFavorite = false,
                                AdditionDate = film.AdditionDate.Value.Date.ToShortDateString(),
                                DownloadUrl = film.DownloadUrl,
                                Description = descr.Text,
                                genreId = genre.Id,
                                genreName = genre.Name,
                                companyId = company.Id,
                                companyName = company.Name,
                            }).ToList();
            return allFilms;
        }
        private IEnumerable<FilmViewModel> GetFilmsByGenre(int genre_id)
        {
            return (from film in Repository.Films
                    join descr in Repository.Desctiption
                         on film.Id equals descr.Film_Id
                    join genre in Repository.Genres
                         on descr.Genre_Id equals genre.Id
                    join company in Repository.Companies
                         on descr.Company_Id equals company.Id
                    where (descr.Genre_Id == genre_id)
                    select new FilmViewModel
                    {
                        Id = film.Id,
                        Name = film.Name,
                        ImageSmallUrl = film.ImageSmallUrl,
                        ImageBigUrl = film.ImageBigUrl,
                        IsFavorite = false,
                        AdditionDate = film.AdditionDate.Value.Date.ToShortDateString(),
                        DownloadUrl = film.DownloadUrl,
                        Description = descr.Text,
                        genreId = genre.Id,
                        genreName = genre.Name,
                        companyId = company.Id,
                        companyName = company.Name,
                    }).ToList();
        }
    }
}