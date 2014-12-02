using shenxl.qingdoc.Common.DataAccess;
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
        private readonly IRepository _repository;

        public FileController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            return View();
        }
        
        [MultipleResponseFormats]
        public ActionResult Show(string id)
        {
            DocumentEntity doc = _repository.Single<DocumentEntity>(id);
            switch (doc.Type)
            {
                case DocumentType.Word:
                    return View("_ShowWord", new { id = id });
                case DocumentType.Excel:
                    return View("_ShowExcel",new { id = id });
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
                doc.VirtualResourcesPath = String.Format("~/File/Upload/{0}/{1}", time, doc.Key);
                doc.ResourcesPath = Server.MapPath(doc.VirtualResourcesPath);

                if (!Directory.Exists(doc.ResourcesPath))
                    Directory.CreateDirectory(doc.ResourcesPath);

                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    doc.FileName = file.FileName;
                    doc.FileExtension = Path.GetExtension(file.FileName);
                    doc.FilePath = Path.Combine(doc.ResourcesPath, doc.FileName);
                    file.SaveAs(doc.FilePath);
                    doc.FileMD5 = FileUtils.GetMD5HashFromFile(doc.FilePath);
                    doc.UploadTime = dt;
                }
                _repository.Add<DocumentEntity>(doc);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isSavedSuccessfully = false;
            }

            return Json(new
            {
                Result = isSavedSuccessfully,
                Key = doc.Key,
                Message = msg
            });
        }

        public ActionResult ReadDocument(string id)
        {
            DocumentEntity doc = _repository.Single<DocumentEntity>(id);
            ConvertDocument convertdoc = DocumentFactory.CreateDocument(doc);
            convertdoc.ConvertToHtml();
            return Content(doc.HtmlDatas[0].HtmlContent);
        }

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
