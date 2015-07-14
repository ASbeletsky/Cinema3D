using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VideoLib.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name:"films",
                url:"films",
                defaults: new {controller = "VideoLib", action = "AllFilmCollection"}
            );

            routes.MapRoute(
                name: "popular_films",
                url: "films/popular",
                defaults: new { controller = "VideoLib", action = "PopularFilmCollection" }
            );

            routes.MapRoute(
               name: "favorites_films",
               url: "films/favorites",
               defaults: new { controller = "VideoLib", action = "UserFavoriteFilmCollection" }
           );

            routes.MapRoute(
               name: "films_by_genre",
               url: "films/by_genre",
               defaults: new { controller = "VideoLib", action = "FilmCollectionByGenre" }
           );

            routes.MapRoute(
               name: "genres",
               url: "genres",
               defaults: new { controller = "VideoLib", action = "GetGenres" }
           );

            routes.MapRoute(
               name: "companies",
               url: "companies",
               defaults: new { controller = "VideoLib", action = "GetCompanies" }
           );

            routes.MapRoute(
               name: "add_favorite",
               url: "films/add_to_favorites",
               defaults: new { controller = "VideoLib", action = "AddFavoriteFilm" }
            );

            routes.MapRoute(
               name: "remove_favorite",
               url: "films/remove_from_favorites",
               defaults: new { controller = "VideoLib", action = "RemoveFavoriteFilm" }
            );

            routes.MapRoute(
               name: "is_favorite",
               url: "films/is_favorite",
               defaults: new { controller = "VideoLib", action = "IsFavorite" }
            );

            routes.MapRoute(
              name: "add_download",
              url: "films/add_download",
              defaults: new { controller = "VideoLib", action = "AddDownload" }
            );

            routes.MapRoute(
              name: "film_by_id",
              url: "films/id={film_id}",
              defaults: new { controller = "VideoLib", action = "FilmById", film_id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "search",
              url: "search",
              defaults: new { controller = "Search", action = "OnSearch" }
            );

            

            routes.MapRoute(
              name: "admin",
              url: "admin",
              defaults: new { controller = "Admin", action = "Index" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new { controller = "Admin", action = "Index" }
            );

        }
    }
}
