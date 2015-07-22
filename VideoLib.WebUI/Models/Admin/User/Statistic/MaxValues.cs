using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Admin.User.Statistic
{
    public class MaxValues
    {
        public int MaxFilmDownloads { get; set; }
        public int MaxInFavorites { get; set; }
        public int MaxFilmRatingVotes { get; set; }
        public int MaxCommentsCount{ get; set; }
    }
}