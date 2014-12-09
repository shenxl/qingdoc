using Aspose.Slides;
using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document.ConvertComponent;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document
{
    public class PowerPointDocument : ConvertDocument
    {

        public PowerPointDocument(DocumentEntity docEntity)
            : base(docEntity)
        {

        }
        public void ConvertToHtml()
        {
            
        }


        public  JsonDocEntity ProcessDocument(ConvertComponentType component)
        {
            throw new NotImplementedException();
        }
    }
}
