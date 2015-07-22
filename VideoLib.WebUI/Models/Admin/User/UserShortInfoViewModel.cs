using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Admin.User
{
    public class UserShortInfoViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int FilmDownloads { get; set; }
        public int FilmsInFavorites { get; set; }
        public int FilmRatingVotes { get; set; }
        public int CommentsCount { get; set; }

        public int FilmDownloadsPercent { get; set; }
        public int FilmsInFavoritesPercent { get; set; }
        public int FilmRatingVotesPercent { get; set; }
        public int CommentsCountPercent { get; set; }
    }
}