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
            var extentesion = docEntity.FileExtension;
            if (extentesion.Equals(".wpt") || extentesion.Equals(".doc") || extentesion.Equals(".docx")
                 || extentesion.Equals(".dotx") || extentesion.Equals(".dot") || extentesion.Equals(".wps"))
            {
                return new WordDocument(docEntity);
            }
            else if (extentesion.Equals(".et") || extentesion.Equals(".ett") || extentesion.Equals(".xlt")
                 || extentesion.Equals(".xls") || extentesion.Equals(".xlsx"))
            {
                return new ExcelDocument(docEntity);
            }
            else if (extentesion.Equals(".ppt") || extentesion.Equals(".pptx") || extentesion.Equals(".pot")
                || extentesion.Equals(".dps") || extentesion.Equals(".dpt"))
            {
                return new PowerPointDocument(docEntity);
            }
            return null;
        }
    }
}
