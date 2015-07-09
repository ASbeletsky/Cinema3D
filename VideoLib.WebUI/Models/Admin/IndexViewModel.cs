using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Admin
{
    public class IndexViewModel
    {
        public IEnumerable<CRUD_FilmViewModel> Films { get; set; }
        public CompanyPanelViewModel CompanyModel { get; set; }
        public GenrePanelViewModel GenreModel { get; set; }
        public CountryPanelViewModel CountryModel { get; set; }

    }

    public class GenrePanelViewModel
    {
        public string Text { get; set; }
        public GenreViewModel DropDown { get; set; }
    }

    public class CountryPanelViewModel
    {
        public string Text { get; set; }
        public CountryViewModel DropDown { get; set; }
    }
    public class CompanyPanelViewModel
    {
        public string Text { get; set; }
        public CompanyViewModel DropDown { get; set; }
    }
}