using shenxl.qingdoc.Filters;
using shenxl.qingdoc.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shenxl.qingdoc.Controllers
{
    public class FileController : Controller
    {
        //
        // GET: /File/

        public ActionResult Index()
        {
            return View();
        }

        [MultipleResponseFormats]
        public ActionResult Show()
        {
            var file = ConfigurationManager.AppSettings["doc"];
            var format = "." + file.Split('.')[1];
            var type = FileConvertHelper.GetFileType(format);
            switch (type) { 
                case "doc":
                    return View("_ShowWord");
                case "xls":
                    return View("_ShowExcel");
                default:
                    return View();
            }
           
           
        }

        public ActionResult ReadAspose()
        {
            string currentPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            var filepath = currentPath + ConfigurationManager.AppSettings["doc"];
            //var filepath = currentPath + @"File\test6.doc";
            var outputpath = currentPath + @"File\output\";
            var html =  FileConvertHelper.OfficeFile2HtmlAspose(filepath, outputpath);

            return Content(html);
        }

        public ActionResult Read()
        {
            string currentPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            var filepath = currentPath + ConfigurationManager.AppSettings["doc"];
            var outputpath = currentPath + @"File\output\";
            FileConvertHelper.OfficeFile2Html(filepath, outputpath);
            var outpath = Path.Combine(outputpath, "output.html");
            var html = FileConvertHelper.ReadFile(outpath);
            HtmlMatcher matcher = new HtmlMatcher();
            var test1 = matcher.GetDiv(html);
            var test2 = matcher.GetStyle(html);
            var content = "";
            foreach (var item in test1)
	        {
                content += item;
	        }
            System.IO.File.Delete(outpath);
            return Content(content);
        }

    }
}
