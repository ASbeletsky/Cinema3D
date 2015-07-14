using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLib.Domian.Concrete.Lucene_Search_Engine.Universal
{
    public class UniversalSearchModel
    {
        private string name;
        public int Id {get;set;}
        public DocumentType Type { get; set; }
        public string Name 
        {
            get { return name; }
            set { name = value.ToLower(); }
        }

        

    }

}
