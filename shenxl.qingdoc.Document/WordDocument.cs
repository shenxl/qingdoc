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
        protected static readonly Regex DIV_REGEX = new Regex(@"(?'div'<div\s*(.|\n)*?</div>)", REGEX_OPTIONS);
        protected static readonly Regex DIV_IMAGE_REGEX = new Regex(@"<img\s*src=""(?'src'(.)*?)""\s*width=(.|\n)*?>", REGEX_OPTIONS);
        
        public WordDocument(DocumentEntity docentity)
            : base(docentity)
        {

        }

        public override void ConvertToHtml()
        {
            var imagePath = Path.Combine(_docEntity.ResourcesPath, _docEntity.ImageFolder);
            Aspose.Words.Document doc = new Aspose.Words.Document(_docEntity.FilePath);
            //记录解析前的页面数
            _docEntity.HtmlDatas.PageNumber = doc.PageCount;
            HtmlSaveOptions saveOptions = new HtmlSaveOptions();
            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);
            saveOptions.ImagesFolder = imagePath;
            saveOptions.ImageSavingCallback = new HandleImageSaving();
            saveOptions.TableWidthOutputMode = HtmlElementSizeOutputMode.RelativeOnly;
            saveOptions.CssStyleSheetType = CssStyleSheetType.Inline;
            using (MemoryStream htmlStream = new MemoryStream())
            {
                try
                {
                    doc.Save(htmlStream, saveOptions);
                    //记录解析实体
                    _docEntity.HtmlDatas.HtmlContent = Encoding.UTF8.GetString(htmlStream.ToArray());
                }
                catch (Exception e)
                {
                    var message = e.Message;
                }
            }
        }


        public override JsonDocEntity ConvertHtmlToEntity()
        {
            var htmldata = _docEntity.HtmlDatas.HtmlContent;
            _docEntity.HtmlDatas.ParseContentList = new List<HtmlParseData>();
            MatchCollection divmatches = DIV_REGEX.Matches(htmldata);
            var count = 1;
            foreach (Match divmatcher in divmatches)
            {
                HtmlParseData divcontent = new HtmlParseData();
                divcontent.pagecount = count;

                var div = divmatcher.Groups["div"].Value;
                var imagematchers = DIV_IMAGE_REGEX.Matches(div);
                foreach (Match iamgematcher in imagematchers)
                {
                    var src = iamgematcher.Groups["src"].Value;
                    var replace = _docEntity.VirtualResourcesPath + "/" + 
                            _docEntity.ImageFolder + "/" +  Path.GetFileName(src);
                    div = div.Replace(src, replace);
                }
                divcontent.content = div;
                _docEntity.HtmlDatas.ParseContentList.Add(divcontent);

                count++;
            }
            return JsonDocEntity.Convert(_docEntity);
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
