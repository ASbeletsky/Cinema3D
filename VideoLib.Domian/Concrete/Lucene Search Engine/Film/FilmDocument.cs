using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Abstract.Lucene_Search_Engine;

namespace VideoLib.Domian.Concrete.Lucene_Search_Engine.Film
{
    public class FilmDocument : AbstactDocument
    {
        private string name;
        private string genre;
        private string company;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                AddParameterToDocumentStoreParameter("FilmName", name);
            }
        }

        public string Genre
        {
            get { return genre; }
            set
            {
                genre = value;
                AddParameterToDocumentStoreParameter("FilmGenre", genre);
            }
        }
        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                AddParameterToDocumentStoreParameter("FilmCompany", company);
            }
        }

        public static explicit operator FilmDocument(FilmSearchModel model)
        {
            return new FilmDocument()
            {
                Id = model.Id,
                Type = model.Type.ToString("G"),
                Name = model.Name,
                Genre = model.Genre,
                Company = model.Company
            };
        }
    }
}
