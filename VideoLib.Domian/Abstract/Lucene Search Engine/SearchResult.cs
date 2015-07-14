using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Concrete.Lucene_Search_Engine.Universal;

namespace VideoLib.Domian.Abstract.Lucene_Search_Engine
{

        public class SearchResult
        {

            public List<SearchResultItem> SearchResultItems { get; set; }
            public int Hits { get; set; }
        }

        public class SearchResultItem 
        {
            public int Id { get; set; }
            public DocumentType Type { get; set; }
            public string Name { get; set; }
           
        }

}
