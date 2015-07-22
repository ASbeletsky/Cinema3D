using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoLib.WebUI.Models.Admin.User.Events
{
    public enum EventType
    {
        Comment,
        Download,
        RatingVote,
        AddToFavorite
    }
    public class EventViewModel
    {
        public EventType Type { get; set; }
        public int RelatedObjectId { get; set; }
        public string Message {get;set;}
        public DateTime Time { get; set; }
    }
}