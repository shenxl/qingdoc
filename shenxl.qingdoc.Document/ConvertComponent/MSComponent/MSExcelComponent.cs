using Microsoft.Office.Interop.Excel;
using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document.ConvertComponent
{
    public class MSExcelComponent : BaseComponent
    {
        
        
        protected static readonly Regex Structure_Mapping
              = new Regex(@"<a\s*?href=""(?'src'(.)*?)""(.|\n)*?><font(.)*?color="".*?"">(?'Name'(.)*?)</font>", REGEX_OPTIONS);
        protected static readonly Regex DIV_REGEX
              = new Regex(@"(?'div'<div\s*(.|\n)*?</div>)", REGEX_OPTIONS);
        protected static readonly Regex TABLE_REGEX
                = new Regex(@"(?'table'<table\s*(.|\n)*</table>)", REGEX_OPTIONS);
        protected static readonly Regex DIV_IMAGE_REGEX
                = new Regex(@"<img(.)*?src=(?'href'(\w)*)", REGEX_OPTIONS);
        protected static readonly Regex TITLE_REGEX
                = new Regex(@"<x:Name>(?'name'[\w|\s]*)</x:Name>", REGEX_OPTIONS);
        protected static readonly Regex STYLE_REGEX
                = new Regex(@"(?'style'(?<=<style>)(.|\n)*?(?=</style>))", REGEX_OPTIONS);

        public override DocumentEntity ConvertToHtml(Common.Entities.DocumentEntity _docEntity)
        {
            if (!_docEntity.isConvert && String.IsNullOrEmpty(_docEntity.ConvertError))
            {
                //Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(_docEntity.FilePath);
                 Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                 Workbook xls = excel.Workbooks.Open(_docEntity.FilePath);
                _docEntity.HtmlData = new HtmlParseContext();
                _docEntity.HtmlData.PageNumber = xls.Worksheets.Count;
                var outputpath = Path.Combine(_docEntity.ResourcesPath, "ConvertFolder");
                _docEntity.ImageFolder = "ConvertFolder.files";
                //if (!Directory.Exists(outputpath))
                //    Directory.CreateDirectory(outputpath);
                try
                {
                    xls.SaveAs(outputpath, XlFileFormat.xlHtml);         
                    //_docEntity.HtmlData.HtmlContent.Add(Encoding.UTF8.GetString(htmlStream.ToArray()));
                    _docEntity.isConvert = true;
                }
                catch (Exception e)
                {
                    _docEntity.isConvert = false;
                    _docEntity.ConvertError = e.Message;
                }
                xls.Close();
                excel.Quit();
                xls = null;
                excel = null;
               
            }
            return _docEntity;
        }

        public override JsonDocEntity ParseHtmlToEntity(Common.Entities.DocumentEntity _docEntity)
        {
            if (!_docEntity.isParse)
            {
                //Excel的文件读取因为涉及到frame里的表关系，所以暂时放置在Parse的逻辑中完成：
                var outputpath = Path.Combine(_docEntity.ResourcesPath, "ConvertFolder.files");
                string[] Files = Directory.GetFiles(outputpath);
                Dictionary<string, string> ParseData = new Dictionary<string, string>();
                string tabstrip = "";
                foreach (var filename in Files)
                {
                    if (filename.EndsWith("tabstrip.htm"))
                    {
                        tabstrip = FileUtils.ReadFile(filename);                   
                    }
                    if (filename.EndsWith("stylesheet.css"))
                    {
                        FileUtils.WriteStyleFile(FileUtils.ReadFile(filename),Path.Combine(_docEntity.ResourcesPath, "etStyle.css"));
                    }
                }

                MatchCollection mapping =  Structure_Mapping.Matches(tabstrip);
                foreach (Match match in mapping)
                {
                    var path = Path.Combine(outputpath,match.Groups["src"].Value);
                    var content = Util.FileUtils.ReadFile(path);
                    var name = match.Groups["Name"].Value;
                    ParseData.Add(name, content);
                    _docEntity.HtmlData.HtmlContent.Add(content);
                }

                foreach (var parsedataitem in ParseData)
                {
                    ///获取当前工作表的表名
                    var worksheetname = parsedataitem.Key; //TITLE_REGEX.Match(parsedataitem.Value).Groups["name"].Value;
                    MatchCollection tablematches = TABLE_REGEX.Matches(parsedataitem.Value);
                    if (String.IsNullOrEmpty(_docEntity.HtmlData.StyleUrl))
                    {
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
                        HashSet<String> hs = new HashSet<string>();
                        foreach (Match iamgematcher in imagematchers)
                        {
                            var src = iamgematcher.Groups["href"].Value;
                            hs.Add(src);
                        }
                        foreach (var item in hs)
                        {
                            table = table.Replace(item, _docEntity.VirtualResourcesPath + "/" +
                                _docEntity.ImageFolder + "/" + Path.GetFileName(item));
                        }
                        divcontent.content = table;
                        _docEntity.HtmlData.ParseContentList.Add(divcontent);
                        count++;
                    }
                }
                _docEntity.isParse = true;
                _docEntity.ConvertCompleteTime = DateTime.Now;
            }
            return JsonDocEntity.Convert(_docEntity);
        }
    }
}
