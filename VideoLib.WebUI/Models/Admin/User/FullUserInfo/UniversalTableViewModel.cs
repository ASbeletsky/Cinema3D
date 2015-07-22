using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Admin.User.FullUserInfo
{
    public class UniversalTableViewModel
    {
        public string FilmName { get; set; }
        public string DateTime { get; set; }
        public int ratingVote { get; set; }
        public float FilmRating { get; set; }

        public string ImageUrl { get; set; }
    }
}