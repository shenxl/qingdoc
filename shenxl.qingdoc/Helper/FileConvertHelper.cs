using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.PowerPoint;
using System.Collections;


namespace shenxl.qingdoc.Helper
{
    public class FileConvertHelper
    {
        public static void OfficeFile2Html(string filepath, string outputpath)
        {
            switch (GetFileType(filepath))
            {
                case "doc":
                    Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                    Document doc = word.Documents.Open(filepath);
                    outputpath = Path.Combine(outputpath, "output.html");
                    doc.SaveAs2(outputpath, WdSaveFormat.wdFormatFilteredHTML);
                    doc.Close(Type.Missing, Type.Missing, Type.Missing);
                    word.Quit(Type.Missing, Type.Missing, Type.Missing);
                    doc = null;
                    word = null;
                    break;

                case "xls":
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Workbook xls = excel.Workbooks.Open(filepath);
                    foreach (Worksheet item in xls.Worksheets)
                    {
                        Worksheet sheet = item;
                        sheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                        sheet.PageSetup.Order = XlOrder.xlDownThenOver;
                        sheet.PageSetup.FitToPagesWide = 1;
                        sheet.PageSetup.FitToPagesTall = 50;
                    }
                    xls.SaveAs(outputpath, XlFileFormat.xlHtml);
                    xls.Close();
                    excel.Quit();
                    xls = null;
                    excel = null;
                    break;
                case "ppt":
                    Microsoft.Office.Interop.PowerPoint.Application powerpoint = new Microsoft.Office.Interop.PowerPoint.Application();
                    Presentation ppt = powerpoint.Presentations.Open(filepath);
                    ppt.SaveAs(outputpath, PpSaveAsFileType.ppSaveAsHTML);
                    ppt.Close();
                    powerpoint.Quit();
                    ppt = null;
                    powerpoint = null;
                    break;
                default:
                    break;
            }

        }

        public static string OfficeFile2HtmlAspose(string filepath, string outputpath)
        {
            string htmlText = "";
            HtmlMatcher matcher = new HtmlMatcher();

            switch (GetFileType(filepath))
            {
                case "doc":
                    Aspose.Words.Document doc = new Aspose.Words.Document(filepath);
                    Aspose.Words.Saving.HtmlSaveOptions saveOptions = new Aspose.Words.Saving.HtmlSaveOptions();
                    saveOptions.ImagesFolder = outputpath;
                    saveOptions.ImageSavingCallback  = new HandleImageSaving();
                    saveOptions.TableWidthOutputMode = Aspose.Words.Saving.HtmlElementSizeOutputMode.RelativeOnly;
                    saveOptions.CssStyleSheetType = Aspose.Words.Saving.CssStyleSheetType.Inline;
                    //saveOptions.CssSavingCallback 
                    using (MemoryStream htmlStream = new MemoryStream())
                    {
                        try
                        {
                            doc.Save(htmlStream, saveOptions);
                            htmlText = Encoding.UTF8.GetString(htmlStream.ToArray());
                        }
                        catch (Exception e)
                        {
                            var message = e.Message;
                        }
                    }
                    matcher = new HtmlMatcher();
                    var wddivList = matcher.GetDivAspose(htmlText);
                    var wdcontent = "";
                    foreach (var item in wddivList)
                    {
                        wdcontent += item;
                    }
                    return wdcontent;

                case "xls":
                    Aspose.Cells.Workbook xls = new Aspose.Cells.Workbook(filepath);
                    foreach (Aspose.Cells.Worksheet item in xls.Worksheets)
                    {
                        Aspose.Cells.HtmlSaveOptions htmlsaveoption = new Aspose.Cells.HtmlSaveOptions(Aspose.Cells.SaveFormat.Html);
                        htmlsaveoption.AttachedFilesDirectory = outputpath;
                        using (MemoryStream htmlStream = new MemoryStream())
                        {
                            try
                            {
                                xls.Save(htmlStream, htmlsaveoption);
                                htmlText = Encoding.UTF8.GetString(htmlStream.ToArray());
                            }
                            catch (Exception e)
                            {
                                var message = e.Message;
                            }
                        }
                        //Aspose.Cells.Worksheet sheet = item;
                        //sheet.PageSetup.Orientation = Aspose.Cells.PageOrientationType.Landscape;
                        //sheet.PageSetup.Order = Aspose.Cells.PrintOrderType.DownThenOver;
                        //sheet.PageSetup.FitToPagesWide = 1;
                        //sheet.PageSetup.FitToPagesTall = 50;
                    }
                    
                    
                    //var test1 = matcher.GetDivAspose(htmlText);
                    //var content = "";
                    //foreach (var item in test1)
                    //{
                    //    content += item;
                    //}
                    return htmlText;

                case "ppt":
                    Aspose.Slides.Presentation ppt = new Aspose.Slides.Presentation(filepath);
                    ppt.Save(outputpath, Aspose.Slides.Export.SaveFormat.Pdf);
                    return "";

                default:
                    return "";
            }

        }

        public static string GetFileType(string Filename)
        {
            Filename.ToLower();
            if (Filename.EndsWith(".wpt") || Filename.EndsWith(".doc") || Filename.EndsWith(".docx")
                 || Filename.EndsWith(".dotx") || Filename.EndsWith(".dot") || Filename.EndsWith(".wps"))
            {
                return "doc";
            }
            else if (Filename.EndsWith(".et") || Filename.EndsWith(".ett") || Filename.EndsWith(".xlt")
                 || Filename.EndsWith(".xls") || Filename.EndsWith(".xlsx"))
            {
                return "xls";
            }
            else if (Filename.EndsWith(".ppt") || Filename.EndsWith(".pptx") || Filename.EndsWith(".pot")
                || Filename.EndsWith(".dps") || Filename.EndsWith(".dpt"))
            {
                return "ppt";
            }
            return "";
        }

        public static void WriteStyleFile(string style, string outpath)
        {
            using (FileStream stream = new FileStream(outpath, FileMode.Create))
            {
                //获取StreamWriter
                using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
                {
                    writer.Write(style);
                }
            }
        }

        public static string ReadFile(string filepath)
        {
            StringBuilder html = new StringBuilder();
            using (StreamReader objReader = new StreamReader(filepath, Encoding.Default))
            {
                string sLine = "";

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null && !sLine.Equals(""))
                        html.AppendLine(sLine);
                }
            }
            return html.ToString();
        }
    }
}