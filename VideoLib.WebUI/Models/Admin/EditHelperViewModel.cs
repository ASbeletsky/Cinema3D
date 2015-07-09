using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Admin
{
    public class EditHelperViewModel
    {
        public CRUD_FilmViewModel model { get; set; }
        public int selectedCountryId {get;set;}
        public int selectedGenreId {get;set;}
        public int sekectedCompanyId {get;set;}                  
    }
}