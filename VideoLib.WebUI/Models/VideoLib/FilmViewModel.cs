using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.VideoLib
{
    public class FilmViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public string genreName { get; set; }
        public int genreId {get;set;}
        public string companyName { get; set; }
        public int companyId {get;set;}       
        public string Description{ get; set; }
        public string AdditionDate { get; set; }
        public string DownloadUrl { get; set; }
        public string ImageSmallUrl { get; set; }
        public string ImageBigUrl { get; set; }


    }

    public class GenreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}