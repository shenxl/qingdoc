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

namespace shenxl.qingdoc.Document
{
    public class WordDocument : ConvertDocument
    {
        
        private static readonly Regex DIV_IMAGE_REGEX = new Regex(@"<img\s*src=""(?'src'(.)*?)""\s*width=(.|\n)*?>", REGEX_OPTIONS);

        public WordDocument(DocumentEntity docentity)
            : base(docentity)
        {

        }

        public override void ConvertToHtml()
        {
            Aspose.Words.Document doc = new Aspose.Words.Document(_docEntity.FilePath);
            HtmlSaveOptions saveOptions = new HtmlSaveOptions();
            saveOptions.ImagesFolder = Path.Combine(_docEntity.ResourcesPath,"image");
            saveOptions.ImageSavingCallback = new HandleImageSaving();
            saveOptions.TableWidthOutputMode = HtmlElementSizeOutputMode.RelativeOnly;
            saveOptions.CssStyleSheetType = CssStyleSheetType.Inline;
            using (MemoryStream htmlStream = new MemoryStream())
            {
                try
                {
                    doc.Save(htmlStream, saveOptions);
                    HtmlParseContext htmldata = new HtmlParseContext();
                    htmldata.HtmlContent = Encoding.UTF8.GetString(htmlStream.ToArray());
                    _docEntity.HtmlData.Add(htmldata);
                }
                catch (Exception e)
                {
                    var message = e.Message;
                }
            }
        }


        public override DocumentEntity ConvertHtmlToEntity()
        {
            throw new NotImplementedException();
        }
    }

    public class HandleImageSaving : IImageSavingCallback
    {
        public void ImageSaving(ImageSavingArgs args)
        {
            if (args.ImageFileName.StartsWith("Aspose.Words."))
                args.ImageFileName = args.ImageFileName.Replace("Aspose.Words.", "");
        }
    }
}
