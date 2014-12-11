using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aspose.Words;
using Aspose.Words.Saving;
using shenxl.qingdoc.Document.Util;

namespace shenxl.qingdoc.Document.ConvertComponent
{
    /// <summary>
    /// Aspose 处理文字的相关逻辑
    /// </summary>
    public class AsposeWordComponent : BaseComponent
    {
        protected static readonly Regex DIV_REGEX
               = new Regex(@"(?'div'<div\s*(.|\n)*?</div>)", REGEX_OPTIONS);

        protected static readonly Regex DIV_IMAGE_REGEX
                = new Regex(@"<img(.|\n)*?src=""(?'src'(.)*?)""", REGEX_OPTIONS);

        protected static readonly Regex STYLE_REGEX
               = new Regex(@"(?'style'(?<=<style\s*type=""text/css"">)\s*(.|\n)*?(?=</style>))", REGEX_OPTIONS);

        public override DocumentEntity ConvertToHtml(DocumentEntity _docEntity)
        {
            if (!_docEntity.isConvert && String.IsNullOrEmpty(_docEntity.ConvertError))
            {
                var imagePath = Path.Combine(_docEntity.ResourcesPath, _docEntity.ImageFolder);
                Aspose.Words.Document doc = new Aspose.Words.Document(_docEntity.FilePath);
                //记录解析前的页面数
                _docEntity.HtmlData.PageNumber = doc.PageCount;
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
                        //HTMLContent 表示原始的HTML内容，此处设置为数组主要为了配合表格多Sheet页的情况，待重构
                        _docEntity.HtmlData.HtmlContent.Add(Encoding.UTF8.GetString(htmlStream.ToArray()));
                        _docEntity.isConvert = true;
                    }
                    catch (Exception e)
                    {
                        _docEntity.isConvert = false;
                        _docEntity.ConvertError = e.Message;
                    }
                }
            }
            return _docEntity;
        }

        public override JsonDocEntity ParseHtmlToEntity(DocumentEntity _docEntity)
        {
            if (_docEntity.isConvert && !_docEntity.isParse)
            {
                var htmldata = _docEntity.HtmlData.HtmlContent[0];
                ///清空当前对象存储HTML解析格式的属性
                ///如果已经解析过的文档就不需要重复处理了
                ///此动作后续需要配合存储一起重构
                if (String.IsNullOrEmpty(_docEntity.HtmlData.StyleUrl))
                {
                    var style = STYLE_REGEX.Match(htmldata).Groups["style"].Value;
                    FileUtils.WriteStyleFile(style, Path.Combine(_docEntity.ResourcesPath, "wpsStyle.css"));
                    _docEntity.HtmlData.StyleUrl = _docEntity.VirtualResourcesPath + "/" + "wpsStyle.css";
                }

                _docEntity.HtmlData.ParseContentList = new List<HtmlParseData>();

                MatchCollection divmatches = DIV_REGEX.Matches(htmldata);
                var count = 1;
                foreach (Match divmatcher in divmatches)
                {
                    HtmlParseData divcontent = new HtmlParseData();
                    divcontent.pagecount = count;

                    var div = divmatcher.Groups["div"].Value;
                    var imagematchers = DIV_IMAGE_REGEX.Matches(div);
                    //根据VirtualResourcesPath属性替换当前页面的Image地址
                    foreach (Match iamgematcher in imagematchers)
                    {
                        var src = iamgematcher.Groups["src"].Value;
                        var replace = _docEntity.VirtualResourcesPath + "/" +
                                _docEntity.ImageFolder + "/" + Path.GetFileName(src);
                        div = div.Replace(src, replace);
                    }
                    divcontent.content = div;
                    _docEntity.HtmlData.ParseContentList.Add(divcontent);

                    count++;
                }
                _docEntity.ConvertCompleteTime = DateTime.Now;
                _docEntity.isParse = true;
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
