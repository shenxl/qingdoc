using shenxl.qingdoc.Common.Entities;
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
        protected static readonly Regex DIV_REGEX 
                = new Regex(@"(?'div'<div\s*(.|\n)*?</div>)", REGEX_OPTIONS);
        protected static readonly Regex TABLE_REGEX
                = new Regex(@"(?'table'<table\s*(.|\n)*</table>)", REGEX_OPTIONS);

        protected static readonly Regex DIV_IMAGE_REGEX 
                = new Regex(@"<img(.|\n)*?src=""(?'src'(.)*?)""", REGEX_OPTIONS);
        protected static readonly Regex TITLE_REGEX
                = new Regex(@"<x:Name>(?'name'[\w|\s]*)</x:Name>", REGEX_OPTIONS);
        protected static readonly Regex STYLE_REGEX
                = new Regex(@"(?'style'(?<=<style>)(.|\n)*?(?=</style>))", REGEX_OPTIONS);

        public ExcelDocument(DocumentEntity docEntity)
            : base(docEntity)
        {

        }

        public override void ConvertToHtml()
        {
            Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(_docEntity.FilePath);
            _docEntity.HtmlData = new HtmlParseContext();
            _docEntity.HtmlData.PageNumber = xls.Worksheets.Count;
            foreach (Aspose.Cells.Worksheet item in xls.Worksheets)
            {
                Aspose.Cells.HtmlSaveOptions htmlsaveoption = new Aspose.Cells.HtmlSaveOptions(Aspose.Cells.SaveFormat.Html);
                htmlsaveoption.AttachedFilesDirectory = Path.Combine(_docEntity.ResourcesPath, "image");
                htmlsaveoption.HtmlCrossStringType = Aspose.Cells.HtmlCrossType.Cross;
                using (MemoryStream htmlStream = new MemoryStream())
                {
                    try
                    {
                        xls.Worksheets.ActiveSheetIndex = item.Index;
                        xls.Save(htmlStream, htmlsaveoption);
                        _docEntity.HtmlData.HtmlContent.Add(Encoding.UTF8.GetString(htmlStream.ToArray()));
                    }
                    catch (Exception e)
                    {
                        var message = e.Message;
                    }
                }
            }
        }

        public override JsonDocEntity ParseHtmlToEntity()
        {
            ///清空当前对象存储HTML解析格式的属性
            ///如果已经解析过的文档就不需要重复处理了
            ///此动作后续需要配合存储一起重构
            //_docEntity.HtmlData.ParseContentList = new List<HtmlParseData>();
            foreach (var htmldata in _docEntity.HtmlData.HtmlContent)
            {
                ///获取当前工作表的表名
                var worksheetname =  TITLE_REGEX.Match(htmldata).Groups["name"].Value;
                MatchCollection tablematches = TABLE_REGEX.Matches(htmldata);
                if (String.IsNullOrEmpty(_docEntity.HtmlData.StyleUrl))
                {
                    var style = STYLE_REGEX.Match(htmldata).Groups["style"].Value;
                    FileUtils.WriteStyleFile(style, Path.Combine(_docEntity.ResourcesPath, "etStyle.css"));
                    _docEntity.HtmlData.StyleUrl = _docEntity.VirtualResourcesPath + "/" + "etStyle.css";
                }
                var count = 1;
                foreach (Match tablematcher in tablematches)
                {
                    HtmlParseData divcontent = new HtmlParseData();
                    divcontent.pagecount = count;
                    divcontent.title = worksheetname;
                    var table = tablematcher.Groups["table"].Value;
                    var imagematchers = DIV_IMAGE_REGEX.Matches(table);
                    foreach (Match iamgematcher in imagematchers)
                    {
                        var src = iamgematcher.Groups["src"].Value;
                        var replace = _docEntity.VirtualResourcesPath + "/" +
                                _docEntity.ImageFolder + "/" + Path.GetFileName(src);
                        table = table.Replace(src, replace);
                    }
                    divcontent.content = table;
                    _docEntity.HtmlData.ParseContentList.Add(divcontent);
                    count++;
                }
            }
            _docEntity.ConvertCompleteTime = DateTime.Now;
            return JsonDocEntity.Convert(_docEntity);
        }
    }
}
