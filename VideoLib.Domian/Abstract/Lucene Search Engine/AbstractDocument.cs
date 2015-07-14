using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;

namespace VideoLib.Domian.Abstract.Lucene_Search_Engine
{
    public abstract class AbstactDocument : IDocument
    {
        private int id;
        private string type;

        private readonly Document document;

        protected AbstactDocument()
        {
            document = new Document();
        }
        public int Id
        {
            set
            {
                id = value;
                AddParameterToDocument("Id", id, Field.Store.YES, Field.Index.NOT_ANALYZED);
            }
            get { return id; }
        }

        public string Type
        {
            set
            {
                type = value;
                AddParameterToDocument("Type", type, Field.Store.YES, Field.Index.NOT_ANALYZED);
            }
            get { return type; }
        }


        public Document Document 
        { 
            get { return document; }
        }

        
        private void AddParameterToDocument(string name, dynamic value, Field.Store store, Field.Index index)
        {
            document.Add(new Field(name, value.ToString(), store, index));
        }
        //Методы добавления поля к документу
        protected void AddParameterToDocumentStoreParameter(string name, dynamic value)
        {
            AddParameterToDocument(name, value, Field.Store.YES, Field.Index.NOT_ANALYZED);
        }

        protected void AddParameterToDocumentNoStoreParameter(string name, dynamic value)
        {
            AddParameterToDocument(name, value, Field.Store.NO, Field.Index.NOT_ANALYZED);
        }
    }
}
