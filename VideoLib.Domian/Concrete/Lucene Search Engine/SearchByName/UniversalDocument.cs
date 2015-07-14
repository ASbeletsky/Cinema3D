using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLib.Domian.Abstract.Lucene_Search_Engine;
using VideoLib.Domian.Entities;

namespace VideoLib.Domian.Concrete.Lucene_Search_Engine.Universal
{
    public class UniversalDocument : AbstactDocument
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                AddParameterToDocumentStoreParameter("Name", name);
            }
        }


        public static explicit operator UniversalDocument(UniversalSearchModel model)
        {
            return new UniversalDocument()
            {
                Id = model.Id,
                Type = model.Type.ToString("G"),
                Name = model.Name,            
            };
        }
    }
}
