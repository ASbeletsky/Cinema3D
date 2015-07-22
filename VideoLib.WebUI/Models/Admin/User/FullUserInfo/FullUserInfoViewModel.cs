using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoLib.WebUI.Models.Admin.User.Events;
using VideoLib.WebUI.Models.Comment;

namespace VideoLib.WebUI.Models.Admin.User.FullUserInfo
{
    public class FullUserInfoViewModel
    {
        public string  UserId { get; set; }
        public IEnumerable<EventViewModel> Events { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public IEnumerable<UniversalTableViewModel> Downloads { get; set; }
        public IEnumerable<UniversalTableViewModel> Favorites { get; set; }
    }
}