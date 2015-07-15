using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using VideoLib.Domian.Abstract;
using Microsoft.AspNet.Identity.Owin;
using VideoLib.Domian.Abstract.Lucene_Search_Engine;
using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using VideoLib.Domian.Concrete.Lucene_Search_Engine.Universal;
using Contrib.Regex;
using System;
using Lucene.Net.QueryParsers;
using VideoLib.Domian.Entities;
using VideoLib.Domian.Concrete.Lucene_Search_Engine.Film;


namespace VideoLib.WebUI.Controllers
{
    public class SearchController : BaseController
    {

        private string dataFolder;

        
         protected override void Initialize(System.Web.Routing.RequestContext requestContext)
          {
            base.Initialize(requestContext);
            dataFolder = Server.MapPath("~/Lucene");
          }

        //Search/GetHint
        [HttpPost]
        public ActionResult GetHint(string term)       
        {
            if (!string.IsNullOrEmpty(term))
            {
                var seacher = new Seacher(dataFolder);
                WildcardQuery nameQuery = new WildcardQuery(new Term("Name", "*" + term + "*"));
                var Result = seacher.Search(nameQuery, 5);
                Result.SearchResultItems.OrderBy(x => x.Type);
                if (Result.SearchResultItems.Count > 0)
                {
                    Result.SearchResultItems.OrderBy(x => x.Type);
                    return Json(Result.SearchResultItems);
                }
            }
            return new JsonResult
            {
                Data = new { OK = false }
            };              
        }

        //POST:search
        [HttpPost]
        public ActionResult OnSearch(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var finalQuery = new BooleanQuery();
                string[] terms = searchString.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < terms.Count(); i++)
                {
                    terms[i].Replace("~", "");
                    finalQuery.Add(new WildcardQuery(new Term("FilmName", "*" + terms[i] + "*")), Occur.SHOULD);
                    finalQuery.Add(new WildcardQuery(new Term("FilmGenre", "*" + terms[i] + "*")), Occur.SHOULD);
                    finalQuery.Add(new WildcardQuery(new Term("FilmCompany", "*" + terms[i] + "*")), Occur.SHOULD);
                }

                var seacher = new Seacher(dataFolder);
                var Result = seacher.Search(finalQuery, 25, "FilmName", OnSeachSort());
                if (Result.SearchResultItems.Count > 0)
                {
                    var film_ids = Result.SearchResultItems.Select(item => item.Id).ToArray();

                    var idToIndexMap = film_ids.Select((i, v) => new { Index = i, Value = v })
                                               .ToDictionary(
                                                    pair => pair.Index,
                                                    pair => pair.Value
                                                );
                    
                                                                                              
                    var searchedFilms = GetFilms<Film>(film => film_ids.Contains(film.Id))
                                       .OrderBy(film => idToIndexMap[film.Id]);
                    return Json(searchedFilms);
                }
            }
            return new JsonResult
            {
                Data = new { OK = false},
            };                
        }

        private Sort OnSeachSort()
        {
            var fields = new[]
            {
                new SortField("FilmName",SortField.STRING),
                new SortField("FilmGenre",SortField.STRING),
                new SortField("FilmCompany",SortField.STRING),
                SortField.FIELD_SCORE
            };
            return new Sort(fields);
        }

        //Search/ReloadDocuments
        public ActionResult ReloadDocuments()
        {            
            var films = (from film in Repository.Films
                              select new UniversalSearchModel
                              {
                                  Id = film.Id,
                                  Name = film.Name,
                                  Type = DocumentType.Film_Hint
                              }).ToList();

            var genres = Repository.Genres.Select(g => new UniversalSearchModel 
            { 
                Id = g.Id, 
                Name = g.Name,
                Type = DocumentType.Genre_Hint
            }).ToList();

            var companies = Repository.Companies.Select(c => new UniversalSearchModel
            { 
                Id = c.Id,
                Name = c.Name,
                Type = DocumentType.Company_Hint
            }).ToList();

            UniversalWriter wr = new UniversalWriter(dataFolder);
            wr.DeleteAllItemsFromIndex();

            wr.AddUpdateItemsToIndex(films);
            wr.AddUpdateItemsToIndex(genres);
            wr.AddUpdateItemsToIndex(companies);

            var filmDocs = (from film in Repository.Films
                            join descr in Repository.Desctiption
                                on film.Id equals descr.Film_Id
                            join genre in Repository.Genres
                                on descr.Genre_Id equals genre.Id
                            join company in Repository.Companies
                                on descr.Company_Id equals company.Id
                            select new FilmSearchModel
                            {
                                Id = film.Id,
                                Name = film.Name,
                                Type = DocumentType.FilmDocument,
                                Genre = genre.Name,
                                Company = company.Name
                            }).ToList();
            FilmWriter fwr = new FilmWriter(dataFolder);
            fwr.AddUpdateItemsToIndex(filmDocs);
            return Json(new { ok = true },JsonRequestBehavior.AllowGet);
        }
    }   
}