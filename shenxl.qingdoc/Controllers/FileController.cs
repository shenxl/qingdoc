using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document;
using shenxl.qingdoc.Document.Factory;
using shenxl.qingdoc.Document.Util;
using shenxl.qingdoc.Filters;
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

        public ActionResult UploadFile()
        {
            return View();
        }
        
        [MultipleResponseFormats]
        public ActionResult Show()
        {
            var file = ConfigurationManager.AppSettings["doc"];
            var format = "." + file.Split('.')[1];
            DocumentEntity doc = new DocumentEntity();

            switch (doc.Type)
            {
                case DocumentType.Word:
                    return View("_ShowWord");
                case DocumentType.Excel:
                    return View("_ShowExcel");
                default:
                    return View();
            }


        }
        [HttpPost]
        public ActionResult FileUpload()
        {
            bool isSavedSuccessfully = true;
            string msg = "";

            DocumentEntity doc = new DocumentEntity();
            doc.Id = Guid.NewGuid();
            try
            {
                DateTime dt = DateTime.Now;
                var time = DateTime.Now.ToString("yyyy/MMdd");
                var path = String.Format("~/File/Upload/{0}/{1}", time, doc.Id);
                
                doc.ResourcesPath = Server.MapPath(path);
                if (!Directory.Exists(doc.ResourcesPath))
                    Directory.CreateDirectory(doc.ResourcesPath);

                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    doc.FileName = file.FileName;
                    doc.FileExtension = Path.GetExtension(file.FileName);
                    doc.FilePath = Path.Combine(doc.ResourcesPath, doc.FileName);
                    doc.FileMD5 = FileUtils.GetMD5HashFromFile(doc.FilePath);
                    file.SaveAs(doc.FilePath);
                    doc.UploadTime = dt;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isSavedSuccessfully = false;
            }

            return Json(new
            {
                Result = isSavedSuccessfully,
                Message = msg
            });
        }

        //public ActionResult ReadAspose()
        //{
        //    string currentPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        //    var filepath = currentPath + ConfigurationManager.AppSettings["doc"];
        //    //var filepath = currentPath + @"File\test6.doc";
        //    var outputpath = currentPath + @"File\output\";
        //    var html = FileConvertHelper.OfficeFile2HtmlAspose(filepath, outputpath);

        //    return Content(html);
        //}

        //public ActionResult Read()
        //{
        //    DocumentEntity doc = new DocumentEntity();
        //    ConvertDocument convertdoc = DocumentFactory.CreateDocument(doc);
        //    string currentPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        //    var filepath = currentPath + ConfigurationManager.AppSettings["doc"];
        //    var outputpath = currentPath + @"File\output\";
        //    FileConvertHelper.OfficeFile2Html(filepath, outputpath);
        //    var outpath = Path.Combine(outputpath, "output.html");
        //    var html = FileConvertHelper.ReadFile(outpath);
        //    HtmlMatcher matcher = new HtmlMatcher();
        //    var test1 = matcher.GetDiv(html);
        //    var test2 = matcher.GetStyle(html);
        //    var content = "";
        //    foreach (var item in test1)
        //    {
        //        content += item;
        //    }
        //    System.IO.File.Delete(outpath);
        //    return Content(content);
        //}

    }
}
