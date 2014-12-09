using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Aspose.Words.Saving;
using System.Text.RegularExpressions;
using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document.Util;
using shenxl.qingdoc.Document.ConvertComponent;
using shenxl.qingdoc.Document.Factory;

namespace shenxl.qingdoc.Document
{
    public class WordDocument : ConvertDocument
    {
       
        public WordDocument(DocumentEntity docentity)
            : base(docentity)
        {

        }

        public WordDocument(DocumentEntity docentity, ConvertComponentType componenttype)
            : base(docentity, componenttype)
        {

        }

        public JsonDocEntity ProcessDocument()
        {
            return null;
        }

    }
 
}
