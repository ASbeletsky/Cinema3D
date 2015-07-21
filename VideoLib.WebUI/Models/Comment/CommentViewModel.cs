using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Comment
{
    public class CommentViewModel
    {
       public int Id { get; set; }
       public string Author {get;set;}
       public string AuthorId { get; set; }
       public int FilmId { get; set; }
       public string Message {get;set;}
       public string AdditionTime {get;set;}
       public sbyte CommentRating { get; set; }
       public sbyte FilmRating { get; set; }                                              
    }
}