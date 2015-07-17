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
using VideoLib.WebUI.Models.Comment;

namespace VideoLib.WebUI.Controllers
{
    public class CommentController : Controller
    {
        private IVideoLibRepository _repository;
        protected IVideoLibRepository Repository
        {
            get { return _repository ?? HttpContext.GetOwinContext().Get<IVideoLibRepository>(); }
        }

        // POST: comments/edit
        public ActionResult SetComment(string object_id, int film_id, string message)
        {
            if (!string.IsNullOrEmpty(object_id) && film_id > 0)
            {
                bool success = false;
                if (!string.IsNullOrEmpty(message))
                {
                    Comment current = Repository.Comments.FirstOrDefault(comment => comment.User_Id == object_id
                                                                         && comment.Film_Id == film_id);
                    if(current != null)
                        success = Repository.EditComment(object_id, film_id, message); 
                    else   
                        success = Repository.AddComment(object_id, film_id, message);
                    return new JsonResult { Data = new { OK = success } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 2,
                        ErrorMessege = "Messege is null or empty"
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 3,
                    ErrorMessege = string.Format("User id or film id have incorrect value")
                },
            };
        }


        // POST: comments/remove
        public ActionResult RemoveComment(string object_id, int film_id)
        {
            if (!string.IsNullOrEmpty(object_id) && film_id > 0)
            {
                bool success = false;
                Comment current = Repository.Comments.FirstOrDefault(comment => comment.User_Id == object_id
                                                                                         && comment.Film_Id == film_id);
                if (current != null)
                {
                    success = Repository.RemoveComment(object_id, film_id);
                    return new JsonResult { Data = new { OK = success } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = "Current comment does not exist"
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 3,
                    ErrorMessege = string.Format("User id or film id have incorrect value")
                },
            };
        }
        // GET: comment/user_id={object_id}/film_id={film_id}
        public ActionResult GetUserFilmComment(string object_id, int film_id)
        {
            if (!string.IsNullOrEmpty(object_id) && film_id > 0)
            {
                     var current = (from comment in Repository.Comments
                                     join user in Repository.Users
                                          on comment.User_Id equals user.Id
                                     join filmRating in Repository.Rating
                                         on user.Id equals filmRating.User_Id
                                     where (comment.User_Id == object_id && comment.Film_Id == film_id)
                                     select new CommentViewModel
                                     {
                                         Id = comment.Id,
                                         Author = user.Name ?? user.Login,
                                         Message = comment.Text,
                                         AdditionTime = comment.AdditionData.Value.ToString("dd.MM.yyyy"),
                                         CommentRating = comment.Rating,
                                         FilmRating = filmRating.RatingValue
                                     }).FirstOrDefault();                                                                   
                if (current != null)
                {                   
                    return new JsonResult { Data = current };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = "Current comment does not exist"
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 3,
                    ErrorMessege = string.Format("User id or film id have incorrect value")
                },
            };
        }


        // POST: comments/like
        public ActionResult LikeComment(int comment_id)
        {
            if (comment_id > 0)
            {

                Comment current = Repository.Comments.FirstOrDefault(comment => comment.Id == comment_id);
                                                                         
                    if (current != null)
                    {
                        Repository.LikeComment(comment_id);
                        return new JsonResult { Data = new { OK = true } };
                    }
                    return new JsonResult
                    {
                        Data = new
                        {
                            OK = false,
                            ErrorId = 1,
                            ErrorMessege = "Current comment does not exist"
                        }
                    };                                
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("Comment id has incorrect value")
                },
            };
        }

        // POST: comments/unlike
        public ActionResult UnlikeComment(int comment_id)
        {
            if ( comment_id > 0)
            {

                Comment current = Repository.Comments.FirstOrDefault(comment => comment.Id == comment_id);
                                                                     
                if (current != null)
                {
                    Repository.UnlikeComment(comment_id);
                    return new JsonResult { Data = new { OK = true } };
                }
                return new JsonResult
                {
                    Data = new
                    {
                        OK = false,
                        ErrorId = 1,
                        ErrorMessege = "Current comment does not exist"
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    OK = false,
                    ErrorId = 2,
                    ErrorMessege = string.Format("Comment id has incorrect value")
                },
            };
        }
    }
}