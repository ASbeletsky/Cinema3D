using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLib.Domian.Entities;
using System.ComponentModel.DataAnnotations;

namespace VideoLib.WebUI.Models.Admin
{
    public class CountryViewModel
    {
        
        private readonly List<Country> _countries;
        public CountryViewModel() { }
        public CountryViewModel(List<Country> countries)
        {
            _countries = countries;
        }
        [Display(Name = "Страна")]
        [Required]
        public int SelectedCountryId { get; set; }

        public IEnumerable<SelectListItem> CountryItems
        {
            get
            {
                var allCountries = _countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
                return allCountries;
            }
        }
        
    }
}