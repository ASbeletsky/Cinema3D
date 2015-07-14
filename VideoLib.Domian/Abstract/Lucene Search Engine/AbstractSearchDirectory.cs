using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Store;
using System.IO;

namespace VideoLib.Domian.Abstract.Lucene_Search_Engine
{
    //Содержит сведения о директории
    public abstract class AbstractSearchDirectory
    {
        private const string LuceneIndexFolder = "LuceneIndex";
        private readonly FSDirectory luceneDirectory;
        private readonly string dataFolder;
 
        public string DataFolder
        {
            get { return DataFolder; }
        }
 
        public FSDirectory LuceneDirectory
        {
            get { return luceneDirectory; }
        }

        protected AbstractSearchDirectory(string dataFolder)
        {
            this.dataFolder = dataFolder;
            var di = new DirectoryInfo(Path.Combine(dataFolder,LuceneIndexFolder));
            if (!di.Exists)
            {
                di.Create();
            }
            luceneDirectory = FSDirectory.Open(di.FullName);
        }
    }
}
