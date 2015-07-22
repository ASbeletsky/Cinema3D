using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLib.Domian.Entities;
using VideoLib.Domian.Abstract;
using VideoLib.WebUI.Models.Admin;
using Microsoft.AspNet.Identity.Owin;
using VideoLib.WebUI.Models.Shared;
using VideoLib.WebUI.Models.Admin.User;
using VideoLib.WebUI.Models.Admin.User.Statistic;
using System.Collections.Generic;
using VideoLib.WebUI.Models.Admin.User.Events;
using VideoLib.WebUI.Models.Admin.User.FullUserInfo;
using VideoLib.WebUI.Models.Comment;

namespace VideoLib.WebUI.Controllers
{

    public class AdminController : Controller
    {
        private IVideoLibRepository _repository;
        private IVideoLibRepository Repository
        {
            get { return _repository ?? HttpContext.GetOwinContext().Get<IVideoLibRepository>(); }
        }

        public ActionResult Main()
        {
            return View();
        }
                                                #region Film Tab
        [HttpGet]
        public ActionResult CreateFilm()
        {
            CRUD_FilmViewModel model = new CRUD_FilmViewModel();
            model.Genre = new GenreViewModel(Repository.Genres.ToList());
            model.Country = new CountryViewModel(Repository.Countries.ToList());
            model.Company = new CompanyViewModel(Repository.Companies.ToList());
            model.Title = "Добавление фильма";
            model.Action = "CreateFilm";
            model.Style = "primary";
            return PartialView("~/Views/Admin/Film/CRUD_View.cshtml", model);
        }
        [HttpPost]
        
        public ActionResult CreateFilm(CRUD_FilmViewModel model)
        {
                Film film;
                Desctiption description;
                ProducerStaff staff;
                ViewToModel(model, out film, out description, out staff);
                Repository.AddNewFilm(film, description, staff);
                return new JsonResult { Data = new { OK = true } };                                   
        }

        [HttpGet]
        public ActionResult EditFilm(int id)
        {
            var CountryDropDown = new CountryViewModel(Repository.Countries.ToList());
            var GenreDropDown = new GenreViewModel(Repository.Genres.ToList());
            var CompanyDropDown = new CompanyViewModel(Repository.Companies.ToList()); 
            
            var result = (from film in Repository.Films
                          join descr in Repository.Desctiption
                              on film.Id equals descr.Film_Id
                          join staff in Repository.ProducerStaff
                              on film.Id equals staff.Film_Id
                          where (film.Id == id)
                          select new EditHelperViewModel
                             {
                                 model = new CRUD_FilmViewModel
                                 {
                                     Id = film.Id,
                                     Name = film.Name,
                                     Genre = GenreDropDown,
                                     Country = CountryDropDown,
                                     Company = CompanyDropDown,
                                     ReleaseDate = descr.ReleaseDate,
                                     Describtion = descr.Text,
                                     TimeDuration = descr.TimeDuration,
                                     Composer = staff.composer,
                                     Director = staff.director,
                                     Operator = staff.@operator,
                                     Producer = staff.producer,
                                     Painter = staff.painter,
                                     DownloadUrl = film.DownloadUrl,
                                     ImageSmallUrl = film.ImageSmallUrl,
                                     ImageBigUrl = film.ImageBigUrl
                                 },
                                 selectedCountryId = descr.Country_id,
                                 selectedGenreId = descr.Genre_Id,
                                 sekectedCompanyId = descr.Company_Id
                             }).FirstOrDefault();
            
           CRUD_FilmViewModel model = result.model;
            model.Country.SelectedCountryId = result.selectedCountryId;
            model.Genre.SelectedGenreId =result.selectedGenreId;
            model.Company.SelectedCompanyId = result.sekectedCompanyId;
            model.Title = string.Format("Редактирование \"{0}\"",model.Name);
            ViewBag.Style = "warning";
            model.Action = "EditFilm";
            model.Style = "warning";
            return PartialView("~/Views/Admin/Film/CRUD_View.cshtml", model);
        }

        [HttpPost]
        
        public ActionResult EditFilm(CRUD_FilmViewModel model)
        {

                Film film;
                Desctiption description;
                ProducerStaff staff;
                ViewToModel(model, out film, out description, out staff);
                Repository.EditFilm(film, description, staff);
                return new JsonResult { Data = new { OK = true } };          
                
        }

        public ActionResult DeleteFilm(int id)
        {
            if(id != 0)
            {
                Repository.RemoveFilm(id);
                return Json(new { OK = true });
            }
            return new JsonResult { Data = new { OK = true } };
        }

        private void ViewToModel(CRUD_FilmViewModel model, out Film film,
                                out Desctiption description, out ProducerStaff staff)
        {
            film = new Film()
            {
                Id = model.Id,
                AdditionDate = DateTime.Now,
                DownloadUrl = model.DownloadUrl,
                ImageSmallUrl = model.ImageSmallUrl,
                ImageBigUrl = model.ImageBigUrl,
                Name = model.Name,
            };
            description = new Desctiption()
            {
                Country_id = model.Country.SelectedCountryId,
                Genre_Id = model.Genre.SelectedGenreId,
                Company_Id = model.Company.SelectedCompanyId,
                ReleaseDate = model.ReleaseDate,
                Text = model.Describtion,
                TimeDuration = model.TimeDuration,
                Film_Id = film.Id
            };
            staff = new ProducerStaff()
            {
                composer = model.Composer,
                director = model.Director,
                @operator = model.Operator,
                painter = model.Painter,
                producer = model.Producer,
                Film_Id = film.Id
            };
        }

        public JsonResult CreateGenre(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                Repository.AddNewGenre(name);
                return new JsonResult { Data = new { OK = true } };
            }
            return new JsonResult { Data = new { OK = false } };
        }

        public JsonResult EditGenre(GenrePanelViewModel model)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(model.Text))
            {
                success = Repository.UpdateGenre(model.DropDown.SelectedGenreId, model.Text);                
            }
            return new JsonResult { Data = new { OK = success } };
        }

        public JsonResult DeleteGenre(int id)
        {
            {
                bool success = false;
                if (id > 0)
                {
                    success = Repository.RemoveGenre(id);
                }
                return new JsonResult { Data = new { OK = success } };
            }
        }

        public JsonResult CreateCompany(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Repository.AddNewCompany(name);
                return new JsonResult { Data = new { OK = true } };
            }
            return new JsonResult { Data = new { OK = false } };
        }

        public JsonResult EditCompany (CompanyPanelViewModel model)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(model.Text))
            {
                success = Repository.UpdateCompany(model.DropDown.SelectedCompanyId, model.Text);
            }
            return new JsonResult { Data = new { OK = success } };
        }

        public JsonResult DeleteCompany(int id)
        {
            {
                bool success = false;
                if (id > 0)
                {
                    success = Repository.RemoveCompany(id);
                }
                return new JsonResult { Data = new { OK = success } };
            }
        }

        public JsonResult CreateCountry(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Repository.AddNewCountry(name);
                return new JsonResult { Data = new { OK = true } };
            }
            return new JsonResult { Data = new { OK = false } };
        }

        public JsonResult EditCountry(CountryPanelViewModel model)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(model.Text))
            {
                success = Repository.UpdateCountry(model.DropDown.SelectedCountryId, model.Text);
            }
            return new JsonResult { Data = new { OK = success } };
        }

        public JsonResult DeleteCountry(int id)
        {
            {
                bool success = false;
                if (id > 0)
                {
                    success = Repository.RemoveCountry(id);
                }
                return new JsonResult { Data = new { OK = success } };
            }
        }


        [HttpGet]
        public PartialViewResult SidePanel()
        {
            var model = new SidePanelModel
            {
                CompanyModel = new CompanyPanelViewModel { DropDown = new CompanyViewModel(Repository.Companies.ToList()) },
                CountryModel = new CountryPanelViewModel { DropDown = new CountryViewModel(Repository.Countries.ToList()) },
                GenreModel = new GenrePanelViewModel { DropDown = new GenreViewModel(Repository.Genres.ToList()) }
            };
            return PartialView("~/Views/Admin/Film/SidePanel/Panel.cshtml", model);
        }

        public ActionResult FilmIndex()
        {
            IndexViewModel model = new IndexViewModel()
            {
                Films = Repository.Films.Select(film => new CRUD_FilmViewModel
                {
                    Id = film.Id,
                    Name = film.Name,
                    Rating = film.RatingValue
                }).ToList(),
            };

            foreach (var item in model.Films)
            {
                item.DownloadNumber = Repository.Downloads.Count(d => d.Film_Id == item.Id);
                item.AddionFavorite = Repository.FavoriteFilms.Count(f => f.Film_Id == item.Id);
            }
            model.MaxDownloads = model.Films.Max(film => film.DownloadNumber);
            model.MaxAddFavorits = model.Films.Max(film => film.AddionFavorite);

            foreach (var item in model.Films)
            {
                if(item.DownloadNumber > 0)
                    item.DownloadRating = (item.DownloadNumber * 5) / model.MaxDownloads;
                if (item.AddionFavorite > 0)
                    item.AddionFavoriteRating = (item.AddionFavorite * 5) / model.MaxAddFavorits;
            }

            return View("~/Views/Admin/Film/Index.cshtml", model);
        }

                                            #endregion

                                                #region Users Tab
        public ViewResult UserIndex()
        {
            var model = new UserIndexViewModel();

            model.UsersTable = Repository.Users.Select(user => new UserTableViewModel(user.Login)
                {
                    Id = user.Id,
                    Name = user.Name,                  
                }). ToList();
            model.UsersTable.RemoveAll(user => user.Id.Contains("admin"));
            foreach(var user in model.UsersTable)
            {
                user.PhotoUrl = Repository.UserClaims.Where(claim => claim.UserId == user.Id && claim.ClaimType == "Photo")
                                                     .Select(claim => claim.ClaimValue).FirstOrDefault();
            }
            return View("~/Views/Admin/User/Index.cshtml", model);
        }


        public PartialViewResult ShortInfo(string id) 
        {
            List<UserShortInfoViewModel> userStatistic = new List<UserShortInfoViewModel>();
            foreach (var user in Repository.Users.ToArray())
            {
                userStatistic.Add(new UserShortInfoViewModel
                    {
                        CommentsCount = Repository.Comments.Count(c => c.User_Id == user.Id),
                        FilmDownloads = Repository.Downloads.Count(d => d.User_Id == user.Id),
                        FilmRatingVotes = Repository.Rating.Count(r => r.User_Id == user.Id),
                        FilmsInFavorites = Repository.FavoriteFilms.Count(f => f.User_Id == user.Id)
                    });
            }

            var maxValues = new MaxValues
            {
                MaxCommentsCount = userStatistic.Max(s => s.CommentsCount),
                MaxFilmDownloads = userStatistic.Max(s => s.FilmDownloads),
                MaxFilmRatingVotes = userStatistic.Max(s => s.FilmRatingVotes),
                MaxInFavorites = userStatistic.Max(s => s.FilmsInFavorites)
            };

            var current = Repository.Users.FirstOrDefault(user => user.Id == id);
            var model = new UserShortInfoViewModel
            {
                Id = id,
                Name = current.Name,
                Email = current.Email,
                CommentsCount = Repository.Comments.Count(comment => comment.User_Id == id),
                FilmDownloads = Repository.Downloads.Count(download => download.User_Id == id),
                FilmsInFavorites = Repository.FavoriteFilms.Count(favoriteFilm => favoriteFilm.User_Id == id),
                FilmRatingVotes = Repository.Rating.Count(rating => rating.User_Id == id),
            };

            if(maxValues.MaxCommentsCount > 0)
                model.CommentsCountPercent = model.CommentsCount * 100 / maxValues.MaxCommentsCount;
            if (maxValues.MaxFilmDownloads > 0)
                model.FilmDownloadsPercent = model.FilmDownloads * 100 / maxValues.MaxFilmDownloads;
            if (maxValues.MaxInFavorites > 0)
                model.FilmsInFavoritesPercent = model.FilmsInFavorites * 100 / maxValues.MaxInFavorites;
            if (maxValues.MaxFilmRatingVotes > 0)
                model.FilmRatingVotesPercent = model.FilmRatingVotes * 100 / maxValues.MaxFilmRatingVotes;

            return PartialView("~/Views/Admin/User/ShortUserInfo.cshtml", model);

        }

        public PartialViewResult FullUserInfo(string id)
        {
            var model = new FullUserInfoViewModel();
            model.UserId = id;
            model.Events = GetEvents(id);
            model.Comments = GetComments(id);
            model.Downloads = GetDownloads(id);
            model.Favorites = GetFavorites(id);

            return PartialView("~/Views/Admin/User/FullUserInfo.cshtml", model);
        }

        public PartialViewResult UserActivities(string id)
        {
            var model = GetEvents(id);
            return PartialView("~/Views/Admin/User/LastActivities.cshtml", model);
        }

        private IEnumerable<UniversalTableViewModel> GetDownloads(string user_id)
        {
            return (from download in Repository.Downloads
                             join rating in Repository.Rating
                                  on new { download.Film_Id, download.User_Id } equals new { rating.Film_Id, rating.User_Id }
                             into FullRating
                             from fullRating in FullRating.DefaultIfEmpty()
                             where (download.User_Id == user_id)
                             join film in Repository.Films
                                on download.Film_Id equals film.Id
                             select new UniversalTableViewModel
                             {
                                 FilmName = film.Name,
                                 DateTime = download.DownloadTime.Value.ToString(),
                                 FilmRating = film.RatingValue,
                                 ratingVote = (fullRating == null) ? (sbyte)0 : fullRating.RatingValue,
                                 ImageUrl = film.ImageSmallUrl
                             }).ToList();
        }

        private IEnumerable<UniversalTableViewModel> GetFavorites(string user_id)
        {
            return (from favorites in Repository.FavoriteFilms
                    join rating in Repository.Rating
                         on new { favorites.Film_Id, favorites.User_Id } equals new { rating.Film_Id, rating.User_Id }
                    into FullRating
                    from fullRating in FullRating.DefaultIfEmpty()
                    where (favorites.User_Id == user_id)
                    join film in Repository.Films
                       on favorites.Film_Id equals film.Id
                    select new UniversalTableViewModel
                    {
                        FilmName = film.Name,
                        DateTime = favorites.AdditionTime.Value.ToString(),
                        FilmRating = film.RatingValue,
                        ratingVote = (fullRating == null) ? (sbyte)0 : fullRating.RatingValue,
                        ImageUrl = film.ImageSmallUrl
                    }).ToList();
        }
        private IEnumerable<CommentViewModel> GetComments(string user_id)
        {
            return (from comment in Repository.Comments
                    join rating in Repository.Rating
                          on new { comment.Film_Id, comment.User_Id } equals new {rating.Film_Id, rating.User_Id }
                    into FullRating
                    from fullRating in FullRating.DefaultIfEmpty()
                         where (comment.User_Id == user_id)
                    join film in Repository.Films
                          on comment.Film_Id equals film.Id
                    select new CommentViewModel
                    {
                        Id = comment.Id,
                        FilmName = film.Name,
                        Message = comment.Text,
                        AdditionTime = comment.AdditionTime.Value.ToString(),
                        CommentRating = comment.Rating,
                        FilmRating = (fullRating == null) ? (sbyte)0 : fullRating.RatingValue,                        
                     });
        }

        private IEnumerable<EventViewModel> GetEvents(string id) 
        {
            var comments = (from comment in Repository.Comments
                            where (comment.User_Id == id)
                            join film in Repository.Films
                                 on comment.Film_Id equals film.Id
                            select new EventViewModel
                            {
                                Type = EventType.Comment,
                                Message = string.Format("Добавил комментарий к фильму \"{0}\"", film.Name),
                                RelatedObjectId = comment.Id,
                                Time = comment.AdditionTime.Value
                            }).ToList();

            var downloads = (from download in Repository.Downloads
                             where (download.User_Id == id)
                             join film in Repository.Films
                                  on download.Film_Id equals film.Id
                             select new EventViewModel
                             {
                                 Type = EventType.Download,
                                 Message = string.Format("Скачал фильм \"{0}\"", film.Name),
                                 RelatedObjectId = download.Film_Id,
                                 Time = download.DownloadTime.Value
                             }).ToList();

            var addionToFavorites = (from favorite in Repository.FavoriteFilms
                                     where (favorite.User_Id == id)
                                     join film in Repository.Films
                                        on favorite.Film_Id equals film.Id
                                     select new EventViewModel
                                     {
                                         Type = EventType.AddToFavorite,
                                         Message = string.Format("Добавил фильм в избранные \"{0}\"", film.Name),
                                         RelatedObjectId = favorite.Film_Id,
                                         Time = favorite.AdditionTime.Value
                                     }).ToList();

            var ratingVotes = (from rating in Repository.Rating
                               where (rating.User_Id == id)
                               join film in Repository.Films
                                    on rating.Film_Id equals film.Id
                               select new EventViewModel
                               {
                                   Type = EventType.AddToFavorite,
                                   Message = string.Format("Поставил оценку \"{0}\" фильму \"{1}\"", rating.RatingValue, film.Name),
                                   RelatedObjectId = rating.Film_Id,
                                   Time = rating.AdditionTime.Value
                               }).ToList();
            var fullList = new List<EventViewModel>();
            fullList.AddRange(comments);
            fullList.AddRange(downloads);
            fullList.AddRange(ratingVotes);
            fullList.AddRange(addionToFavorites);

            return fullList.OrderBy(_event => _event.Time);
        }
                                                #endregion
    }
}
