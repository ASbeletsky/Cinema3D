using System.Collections.Generic;
using System.Linq;
using log4net;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace VideoLib.Domian.Abstract.Lucene_Search_Engine
{
    //Описывает действия с документами в индексе
    public abstract class BaseWriter : AbstractSearchDirectory
    {        
        protected BaseWriter(string dataFolder):base(dataFolder)
        {
            
        }

        private void AddItemToIndex(AbstactDocument doc, IndexWriter writer)
        {
            var query = new BooleanQuery();
            query.Add(new TermQuery(new Term("Id", doc.Id.ToString())), Occur.MUST);
            query.Add(new TermQuery(new Term("Type", doc.Type)), Occur.MUST);

            writer.DeleteDocuments(query);
            writer.AddDocument(doc.Document);
        }
         
        protected void AddUpdateItemsToIndex(IEnumerable<AbstactDocument> docs)
        {            
 
            using (var writer = new IndexWriter(LuceneDirectory, new StandardAnalyzer(Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var doc in docs)
                {
                    AddItemToIndex(doc, writer);
                }
            }
        }
       
        private void DeleteItemFromIndex(AbstactDocument doc, IndexWriter writer)
        {
            var query = new TermQuery(new Term("Id", doc.Id.ToString()));
            writer.DeleteDocuments(query);
        }
         
        protected void DeleteItemsFromIndex(IEnumerable<AbstactDocument> docs)
        {
            using (var writer = new IndexWriter(LuceneDirectory, new StandardAnalyzer(Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var doc in docs)
                {                    
                    DeleteItemFromIndex(doc, writer);
                }                
            }
        }

        public void DeleteAllItemsFromIndex()
        {
            using (var writer = new IndexWriter(LuceneDirectory, new StandardAnalyzer(Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED))
            {
                writer.DeleteAll();
            }
        }
        protected void Optimize()
        { 
            using (var writer = new IndexWriter(LuceneDirectory, new StandardAnalyzer(Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED))
            {
                writer.Optimize();                
            }
        }

        
    }
}
