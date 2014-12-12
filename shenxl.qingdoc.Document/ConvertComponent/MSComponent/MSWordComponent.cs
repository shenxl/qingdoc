using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using shenxl.qingdoc.Document.Util;
using shenxl.qingdoc.Common.Entities;
using Microsoft.Office.Interop.Word;

namespace shenxl.qingdoc.Document.ConvertComponent
{
    /// <summary>
    /// Aspose 处理文字的相关逻辑
    /// </summary>
    public class MSWordComponent : BaseComponent
    {
        protected static readonly Regex DIV_REGEX
               = new Regex(@"(?'div'<div\s*(.|\n)*?</div>)", REGEX_OPTIONS);

        protected static readonly Regex DIV_IMAGE_REGEX
                = new Regex(@"<img(.|\n)*?src=""(?'src'(.)*?)""", REGEX_OPTIONS);

        protected static readonly Regex STYLE_REGEX
               = new Regex(@"(?'style'(?<=<style>)(.|\n)*?(?=</style>))", REGEX_OPTIONS);

        public override DocumentEntity ConvertToHtml(DocumentEntity _docEntity)
        {
            if (!_docEntity.isConvert && String.IsNullOrEmpty(_docEntity.ConvertError))
            {
                //var imagePath = Path.Combine(_docEntity.ResourcesPath, _docEntity.ImageFolder);
                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(_docEntity.FilePath);
                try
                {   
                    //记录解析前的页面数
                    //_docEntity.HtmlData.PageNumber = doc.PageSetup.PaperSize
                    var outputpath = Path.Combine(_docEntity.ResourcesPath,"Convert");
                    _docEntity.ImageFolder = "Convert" + @"/" + "output.files";
                    if (!Directory.Exists(outputpath))
                        Directory.CreateDirectory(outputpath);
                    word.ChangeFileOpenDirectory(outputpath);
                    doc.SaveAs2("output.htm", WdSaveFormat.wdFormatFilteredHTML);
                    
                    _docEntity.HtmlData.HtmlContent.Add(FileUtils.ReadFile(Path.Combine(outputpath,"output.htm")));
                    _docEntity.isConvert = true;
                }
                catch (Exception e)
                {
                    _docEntity.isConvert = false;
                    _docEntity.ConvertError = e.Message;
                }
                doc.Close(Type.Missing, Type.Missing, Type.Missing);
                word.Quit(Type.Missing, Type.Missing, Type.Missing);
            }
            return _docEntity;
        }

        public override JsonDocEntity ParseHtmlToEntity(DocumentEntity _docEntity)
        {
            if (_docEntity.isConvert && !_docEntity.isParse)
            {
                var htmldata = _docEntity.HtmlData.HtmlContent[0];
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
                    HashSet<String> hs = new HashSet<string>();
                    foreach (Match iamgematcher in imagematchers)
                    {
                        var src = iamgematcher.Groups["src"].Value;
                        hs.Add(src);
                    }

                    foreach (var item in hs)
                    {
                        div = div.Replace(item, _docEntity.VirtualResourcesPath + "/" +
                            _docEntity.ImageFolder + "/" + Path.GetFileName(item));
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

        public string ReadFile { get; set; }
    }
}
