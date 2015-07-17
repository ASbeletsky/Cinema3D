using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLib.Domian.Concrete;
using VideoLib.Domian.Entities;
using VideoLib.Domian.Abstract;
using VideoLib.WebUI.Models.VideoLib;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Newtonsoft.Json;

namespace VideoLib.WebUI.Controllers
{
    public class RatingController : Controller
    {
        private IVideoLibRepository _repository;
        protected IVideoLibRepository Repository
        {
            get { return _repository ?? HttpContext.GetOwinContext().Get<IVideoLibRepository>(); }
        }

        //POST:rating/set_rate
        public ActionResult SetFilmRate(string object_id, int film_id, sbyte rate)
        {
            if (!string.IsNullOrEmpty(object_id) && film_id > 0)
            {
                bool success = false;
                if (rate > 0 && rate <=5)
                {
                    Rating current = Repository.Rating.FirstOrDefault(ratingValue => ratingValue.User_Id == object_id
                                                                      && ratingValue.Film_Id == film_id);
                    if (current != null)
                    {
                        success = Repository.EditFilmRate(object_id, film_id, rate);                       
                    }
                    else
                    {
                        success = Repository.AddFilmRate(object_id, film_id, rate);
                    }
                    return new JsonResult { Data = new { OK = success } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = "Rate has incorrect value"
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User id or film id have incorrect value")
                },
            };
        }
        //POST:rating/get_rate
        public ActionResult GetFilmRate(string object_id, int film_id)
        {
            if (!string.IsNullOrEmpty(object_id) && film_id > 0)
            {
                    Rating current = Repository.Rating.FirstOrDefault(ratingValue => ratingValue.User_Id == object_id
                                                                      && ratingValue.Film_Id == film_id);
                    if (current != null)
                    {                        
                        return new JsonResult { Data = new { Rate = current.RatingValue } };
                    }
                    return new JsonResult
                    {
                        Data = new
                        {
                            OK = false,
                            ErrorId = 1,
                            ErrorMessege = string.Format("Film with id \"{0}\" does not have comments from user with id \"{1}\"", film_id, object_id)
                        }
                    };
            }                             
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("User id or film id have incorrect value")
                },
            };
        }
    }
}