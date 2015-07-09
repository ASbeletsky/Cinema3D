using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLib.Domian.Entities
{
    public class MyJsonResponse
    {
        public MyJsonResponse()
        {
            this.Errors = new List<string>();
        }
        public bool OK {get;set;}
        public List<string> Errors { get; set; }
    }
}
