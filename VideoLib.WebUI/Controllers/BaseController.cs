﻿using System;
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
    public class BaseController : Controller
    {
        private IVideoLibRepository _repository;
        protected IVideoLibRepository Repository
        {
            get { return _repository ?? HttpContext.GetOwinContext().Get<IVideoLibRepository>(); }
        }
        protected IEnumerable<FilmViewModel> GetFilms<T>(Func<T, bool> predicat)
        {
            var allFilms = (from film in Repository.Films
                            join descr in Repository.Desctiption
                                on film.Id equals descr.Film_Id
                            join genre in Repository.Genres
                                on descr.Genre_Id equals genre.Id
                            join company in Repository.Companies
                                on descr.Company_Id equals company.Id
                            where (predicat(ChoseArg<T>(film, descr)))
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
        private dynamic ChoseArg<T>(Film film, Desctiption descr)
        {
            Type type = typeof(T);
            if (type == typeof(Film))
                return film;
            return descr;
        }
    }
}