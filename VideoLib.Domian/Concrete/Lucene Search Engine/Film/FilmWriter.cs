using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Abstract.Lucene_Search_Engine;

namespace VideoLib.Domian.Concrete.Lucene_Search_Engine.Film
{
    public class FilmWriter : BaseWriter
    {
        public FilmWriter(string dataFolder)
            : base(dataFolder)
        {
            
        }

        public void AddUpdateItemToIndex(FilmSearchModel model)
        {
            AddUpdateItemsToIndex(new List<FilmDocument> { (FilmDocument)model });
        }

        public void AddUpdateItemsToIndex(List<FilmSearchModel> model)
        {
            AddUpdateItemsToIndex(model.Select(m => (FilmDocument)m).ToList());
        }

        public void DeleteItemsFromIndex(List<FilmSearchModel> model)
        {
            DeleteItemsFromIndex(model.Select(m => (FilmDocument)m).ToList());
        }

        public void DeleteItemFromIndex(int id)
        {
            DeleteItemsFromIndex(new List<FilmDocument> { new FilmDocument { Id = id } });
        }
    }
}
