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


namespace VideoLib.WebUI.Controllers
{
    public class SearchController : Controller
    {
        private IVideoLibRepository _repository;
        private string dataFolder = ".";
        private IVideoLibRepository Repository
        {
            get { return _repository ?? HttpContext.GetOwinContext().Get<IVideoLibRepository>(); }
        }

        //Search/GetHint
        [HttpPost]
        public ActionResult GetHint(string term)       
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

            return new JsonResult
            {
                Data = new { OK = false }
            };              
        }

        //POST:search
        [HttpPost]
        public ActionResult OnSearch(string searchString)
        {
            var finalQuery = new BooleanQuery();
            string[] terms = searchString.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < terms.Count(); i++ )
            {
                terms[i].Replace("~", "");

                finalQuery.Add(new FuzzyQuery(new Term("Name", terms[i])), Occur.SHOULD);
                finalQuery.Add(new WildcardQuery(new Term("Name", "*" + terms[i] + "*")), Occur.SHOULD);
            }

            var seacher = new Seacher(dataFolder);  
            var Result = seacher.Search(finalQuery, 25);
            if (Result.SearchResultItems.Count > 0)
            {
                Result.SearchResultItems.OrderBy(x => x.Type);
                return Json(Result.SearchResultItems);
            }
            
            return new JsonResult
            {
                Data = new { OK = false},
            };                
        }


        //Search/ReloadDocuments
        public ActionResult ReloadDocuments()
        {
            var films = (from film in Repository.Films
                          join descr in Repository.Desctiption
                              on film.Id equals descr.Film_Id
                              select new UniversalSearchModel
                              {
                                  Id = film.Id,
                                  Name = film.Name,
                                  Type = DocumentType.Film
                              }).ToList();

            var genres = Repository.Genres.Select(g => new UniversalSearchModel 
            { 
                Id = g.Id, 
                Name = g.Name,
                Type = DocumentType.Genre
            }).ToList();

            var companies = Repository.Companies.Select(c => new UniversalSearchModel
            { 
                Id = c.Id,
                Name = c.Name,
                Type = DocumentType.Company
            }).ToList();

            UniversalWriter wr = new UniversalWriter(dataFolder);
            wr.DeleteAllItemsFromIndex();

            wr.AddUpdateItemsToIndex(films);
            wr.AddUpdateItemsToIndex(genres);
            wr.AddUpdateItemsToIndex(companies);

            return Json(new { ok = true },JsonRequestBehavior.AllowGet);
        }
    }   
}