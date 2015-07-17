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
using VideoLib.WebUI.Models.Comment;

namespace VideoLib.WebUI.Controllers
{
    public class VideoLibController : BaseController
    {
       
        //GET: films
        public JsonResult AllFilmCollection()
        {
            var Films = GetFilms<Film>(x => x.Id > 0); 
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

        

        //GET: films/popular       
        public JsonResult PopularFilmCollection()
        {
            var films = GetFilms<Film>( x => x.Id > 0);
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
        public JsonResult UserFavoriteFilmCollection(string object_id)
        {
            {
                if (!string.IsNullOrEmpty(object_id))
                {
                    var userFavorite = Repository.Users.First(user => user.Id == object_id).favoritefilms;
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
                return new JsonResult { Data = new { ErrorId = 1, ErrorMessege = string.Format("User with id {0} doesn't exist", object_id) } };
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
            if (properties.film_id > 0 && !string.IsNullOrEmpty(properties.object_id))
            {
                bool success = false;                            
                var favoritFilmsIds = Repository.FavoriteFilms
                    .Where(f => f.User_Id == properties.object_id)
                    .Select(f => f.Film_Id).ToList();
                if(!favoritFilmsIds.Contains(properties.film_id))
                {
                    success = Repository.AddFavoriteFilm(properties.object_id, properties.film_id);
                    return new JsonResult { Data = new { OK = success } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = string.Format("User with id {0} have already add film with id {1}", properties.object_id, properties.film_id)
                    }
                };
            }
            return new JsonResult {
                Data = new 
                { 
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User with id {0} or film with id {1} does not exist", properties.object_id, properties.film_id)
                },                
            };
        }

        //POST:films/remove_from_favorites
        public JsonResult RemoveFavoriteFilm(UserFilmProperties properties)
        {
            if (properties.film_id > 0 && !string.IsNullOrEmpty(properties.object_id))
            {
                bool success = false;            
                var favoritFilmsIds = Repository.FavoriteFilms
                    .Where(f => f.User_Id == properties.object_id)
                    .Select(f => f.Film_Id).ToList();
                if (favoritFilmsIds.Contains(properties.film_id))
                {
                    success = Repository.RemoveFavoriteFilm(properties.object_id, properties.film_id);
                    return new JsonResult { Data = new { OK = success } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = string.Format("User with id {0} doen't have film with id {1}", properties.object_id, properties.film_id)
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User with id {0} or film with id {1} does not exist", properties.object_id, properties.film_id)
                },
            };
        }

        //POST:films/add_download
        public JsonResult AddDownload(UserFilmProperties properties)
        {
            bool success = false;
            if (properties.film_id > 0 && !string.IsNullOrEmpty(properties.object_id))
            {
                success = Repository.AddDownload(properties.object_id, properties.film_id);
            }
            return new JsonResult
            {
                Data = new { OK = success },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //GET:films/genre_id={genre_id}
        public JsonResult FilmCollectionByGenre(int genre_id)
        {                        
            if (genre_id > 0 )
            {
                var filmsByGenre = GetFilms<Desctiption>(descr => descr.Genre_Id == genre_id);
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
                            ErrorMessege = string.Format("Films collection by genre with id {0} is empty", genre_id)
                        },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("Genre with id {0} doesn't exist ", genre_id)
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        

        //POST:films/is_favorite
        [HttpPost]
        public JsonResult IsFavorite(UserFilmProperties properties)
        {
            if (properties.film_id > 0 && !string.IsNullOrEmpty(properties.object_id))
            {
                bool contains = false;               
                    var userFavoriteFilms = Repository.FavoriteFilms.Where(f => f.User_Id == properties.object_id).ToList();
                    if (userFavoriteFilms != null)
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
                            ErrorMessege = string.Format("User with id {0} doesn't have favorites films ", properties.object_id)
                        }
                    };                
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User with id {0} or film with id {1} does not exist", properties.object_id, properties.film_id)
                }
            };
        }
        //GET:films/id={film_id} 
        public JsonResult FilmById(int film_id)
        {
            var Films = GetFilms<Film>(x => x.Id == film_id);
            if (Films.First() != null)
            {
                return Json(Films.First(), JsonRequestBehavior.AllowGet);
            }
            return new JsonResult
            {
                Data = new {OK = false, ErrorId = 1, ErrorMessege = string.Format("Film with id {0} does not exist", film_id) },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //GET:films/id={film_id}/comments
        public ActionResult GetAllComents(int film_id)
        {
            if(film_id > 0)
            {
                var comments = (from comment in Repository.Comments
                                join rating in Repository.Rating
                                    on new { comment.Film_Id, comment.User_Id } equals new {rating.Film_Id, rating.User_Id }
                                                                                            into FullRating
                                from fullRating in FullRating.DefaultIfEmpty()
                                    where (comment.Film_Id == film_id)
                                join user in Repository.Users
                                    on comment.User_Id equals user.Id
                                select new CommentViewModel
                                {
                                    Id = comment.Id,
                                    Author = user.Name ?? user.Login,
                                    AuthorId = user.Id,
                                    Message = comment.Text,
                                    AdditionTime = comment.AdditionData.Value.ToString("dd.MM.yyyy"),
                                    CommentRating = comment.Rating,
                                    FilmRating = (fullRating == null) ? (sbyte)0 : fullRating.RatingValue
                                }).OrderByDescending(comment => comment.CommentRating)
                                  .ThenBy(comment => comment.AdditionTime).Distinct().ToList();

                if(comments != null)
                {
                    return Json(comments, JsonRequestBehavior.AllowGet);
                }
                return new JsonResult
                {
                    Data = new { OK = false, ErrorId = 1, ErrorMessege = string.Format("film with id \"{0}\" does not have comments", film_id) },
                    JsonRequestBehavior= JsonRequestBehavior.AllowGet
                };
            }
            return new JsonResult
            {
                Data = new { OK = false, ErrorId = 2, ErrorMessege = string.Format("film id \"{0}\" is null or film does not exist", film_id) },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        } 
 

    }
}