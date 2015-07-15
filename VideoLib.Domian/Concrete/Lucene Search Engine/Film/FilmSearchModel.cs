using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Concrete.Lucene_Search_Engine.Universal;

namespace VideoLib.Domian.Concrete.Lucene_Search_Engine.Film
{
    public class FilmSearchModel
    {
        private string name;
        private string genre;
        private string company;
        public int Id { get; set; }
        public DocumentType Type { get; set; }
        public string Name
        {
            get { return name; }
            set { name = value.ToLower(); }
        }
        public string Genre
        {
            get { return genre; }
            set { genre = value.ToLower(); }
        }
        public string Company
        {
            get { return company; }
            set { company = value.ToLower(); }
        }
    }
}
