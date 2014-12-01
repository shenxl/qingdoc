using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document
{
    public class ExcelDocument : ConvertDocument
    {

        public ExcelDocument(DocumentEntity docEntity)
            : base(docEntity)
        {

        }

        public override void ConvertToHtml()
        {
            Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(_docEntity.FilePath);
            foreach (Aspose.Cells.Worksheet item in xls.Worksheets)
            {
                Aspose.Cells.HtmlSaveOptions htmlsaveoption = new Aspose.Cells.HtmlSaveOptions(Aspose.Cells.SaveFormat.Html);
                htmlsaveoption.AttachedFilesDirectory = Path.Combine(_docEntity.ResourcesPath, "image");
                using (MemoryStream htmlStream = new MemoryStream())
                {
                    try
                    {
                        xls.Save(htmlStream, htmlsaveoption);
                        HtmlParseContext htmldata = new HtmlParseContext();
                        htmldata.HtmlContent = Encoding.UTF8.GetString(htmlStream.ToArray());
                        _docEntity.HtmlDatas.Add(htmldata);
                    }
                    catch (Exception e)
                    {
                        var message = e.Message;
                    }
                }
            }
        }

        public override DocumentEntity ConvertHtmlToEntity()
        {
            throw new NotImplementedException();
        }
    }
}
