using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document.ConvertComponent;
using shenxl.qingdoc.Document.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document
{
    public class ExcelDocument : ConvertDocument
    {
      

        public ExcelDocument(DocumentEntity docEntity)
            : base(docEntity)
        {

        }

        public void ConvertToHtml()
        {
            
        }

        public JsonDocEntity ParseHtmlToEntity()
        {
            
        }

        public override JsonDocEntity ProcessDocument(ConvertComponentType component)
        {
            throw new NotImplementedException();
        }
    }
}
