using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLib.Domian.Abstract.Lucene_Search_Engine
{
    public interface IDocument
    {
        int Id { get; set; }
        string Type { get; set; }
    }
}
