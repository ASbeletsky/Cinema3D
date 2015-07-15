using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Concrete.Lucene_Search_Engine.Universal;

namespace VideoLib.Domian.Abstract.Lucene_Search_Engine
{
    public class Seacher : AbstractSearchDirectory
    {
        private IndexSearcher seacher;
        public Seacher(string dataFolder) :base(dataFolder)
        {
            seacher = new IndexSearcher(this.LuceneDirectory);
        }

        public SearchResult Search(Query searchQuery, int maxHits, string NameField = "Name", Sort sort = null)
        {
            SearchResult result = new SearchResult();
            TopDocs hits = null;
            result.SearchResultItems = new List<SearchResultItem>();
            if(sort != null)
            {
                hits = seacher.Search(searchQuery, null, maxHits, sort);
            }
            else
            {
                hits = seacher.Search(searchQuery, null, maxHits);
            }
            for (int i = 0; i < hits.ScoreDocs.Count(); i++)
            {
                Document doctemp = seacher.Doc(hits.ScoreDocs[i].Doc);
                result.SearchResultItems.Add(new SearchResultItem
                {
                    Id = int.Parse(doctemp.Get("Id")),
                    Name = doctemp.Get(NameField),
                    Type = (DocumentType) Enum.Parse(typeof(DocumentType), doctemp.Get("Type"))
                });
            }
            result.Hits = hits.ScoreDocs.Count();

            return result;
        }
    }
}
