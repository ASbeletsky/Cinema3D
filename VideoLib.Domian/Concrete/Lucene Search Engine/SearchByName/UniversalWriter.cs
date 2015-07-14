using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Abstract.Lucene_Search_Engine;
using VideoLib.Domian.Entities;

namespace VideoLib.Domian.Concrete.Lucene_Search_Engine.Universal
{
    public class UniversalWriter : BaseWriter
    {
        public UniversalWriter(string dataFolder)
            : base(dataFolder)
        {
            
        }

        public void AddUpdateItemToIndex(UniversalSearchModel model)
        {
            AddUpdateItemsToIndex(new List<UniversalDocument> { (UniversalDocument)model });
        }

        public void AddUpdateItemsToIndex(List<UniversalSearchModel> model)
        {
            AddUpdateItemsToIndex(model.Select(m => (UniversalDocument)m).ToList());
        }

        public void DeleteItemsFromIndex(List<UniversalSearchModel> model)
        {
            DeleteItemsFromIndex(model.Select(m => (UniversalDocument)m).ToList());
        }

        public void DeleteItemFromIndex(int id)
        {
            DeleteItemsFromIndex(new List<UniversalDocument> { new UniversalDocument { Id = id } });
        }
    }
}
