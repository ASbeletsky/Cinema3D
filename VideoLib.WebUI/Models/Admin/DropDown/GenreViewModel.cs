using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLib.Domian.Entities;
using System.ComponentModel.DataAnnotations;
namespace VideoLib.WebUI.Models.Admin
{
    public class GenreViewModel
    {
        private readonly List<Genre> _genres;
        public GenreViewModel() { }

        public GenreViewModel(List<Genre> genres)
        {
            _genres = genres;
        }

        [Display(Name = "Жанр")]
        public int SelectedGenreId { get; set; }

        public IEnumerable<SelectListItem> GenreItems
        {
            get
            {
                var AllGenres = _genres.Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                });
                return  AllGenres;
            }
        }

        
    }
}