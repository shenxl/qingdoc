using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shenxl.qingdoc.Document;
using shenxl.qingdoc.Common.Entities;

namespace shenxl.qingdoc.Document.Factory
{
    public class DocumentFactory
    {
        public static ConvertDocument CreateDocument(DocumentEntity docEntity)
        {
            switch (docEntity.Type)
            {
                case DocumentType.Word:
                    return new WordDocument(docEntity);
                case DocumentType.Excel:
                    return new ExcelDocument(docEntity);
                case DocumentType.PowerPoint:
                    return new PowerPointDocument(docEntity);
                default:
                    return null;
            }
        }
    }
}
