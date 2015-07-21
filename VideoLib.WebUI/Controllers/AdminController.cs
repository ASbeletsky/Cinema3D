using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLib.Domian.Entities;
using VideoLib.Domian.Abstract;
using VideoLib.WebUI.Models.Admin;
using Microsoft.AspNet.Identity.Owin;
using VideoLib.WebUI.Models.Shared;

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

    }
}
