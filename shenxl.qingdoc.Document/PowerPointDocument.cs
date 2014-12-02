using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
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
        public override void ConvertToHtml()
        {
            throw new NotImplementedException();
        }

        public override JsonDocEntity ParseHtmlToEntity()
        {
            throw new NotImplementedException();
        }
    }
}
