using shenxl.qingdoc.Common.DataAccess;
using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document;
using shenxl.qingdoc.Document.ConvertComponent;
using shenxl.qingdoc.Document.Factory;
using shenxl.qingdoc.Document.Util;
using shenxl.qingdoc.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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

                case DocumentType.PowerPoint:
                    return View("_ShowPPT", new { id = id });
                default:
                    return View();
            }
        }
        [HttpPost]
        public ActionResult FileUpload()
        {
            bool isSavedSuccessfully = true;
            string msg = "";
            DocumentEntity doc = null;
            try
            {
                DateTime dt = DateTime.Now;
                HttpPostedFileBase file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    ///通过Md5值判断,如果文件已经上传，则直接返回Key值。
                    ///留下与后续数据存放的逻辑一同实现
                    var md5  = FileUtils.GetMD5HashFromFileStream(file.InputStream);
                    var entitycount = _repository.All<DocumentEntity>();
                    if (entitycount != null)
                    {
                        doc = entitycount.
                            Where<DocumentEntity>(item => item.FileMD5 == md5).SingleOrDefault();
                        if (null != doc)
                        {
                            return Json(new
                            {
                                Result = isSavedSuccessfully,
                                Key = doc.Key,
                                Message = msg
                            });
                        }
                    }
                    //如果文件未上传，则构造DocumentEntity实体，上传并保存
                    doc = new DocumentEntity();
                    string time = DateTime.Now.ToString("yyyy/MMdd");
                    doc.VirtualResourcesPath = String.Format("/File/Upload/{0}/{1}", time, doc.Key);
                    doc.ResourcesPath = Server.MapPath(doc.VirtualResourcesPath);

                    if (!Directory.Exists(doc.ResourcesPath))
                        Directory.CreateDirectory(doc.ResourcesPath);
                    doc.FileName = file.FileName;
                    doc.FileExtension = Path.GetExtension(file.FileName);
                    doc.FilePath = Path.Combine(doc.ResourcesPath, doc.FileName);
                    doc.FileMD5 = md5;
                    file.SaveAs(doc.FilePath);
                    doc.UploadTime = dt;
                    _repository.Add<DocumentEntity>(doc);
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
                Key = doc.Key,
                Message = msg
            });

        }

        public ActionResult read(string id)
        {
            DocumentEntity doc = _repository.Single<DocumentEntity>(id);
            var fields = typeof(ConvertComponentType).GetFields(BindingFlags.Static | BindingFlags.Public);
            var componentName = ConfigurationManager.AppSettings["Component"];
            ConvertComponentType selectenum = (ConvertComponentType)Enum.Parse(typeof(ConvertComponentType), componentName, false);
            ConvertDocument convertdoc = new ConvertDocument(doc, selectenum);
            ///未考虑到的情况： 如果文档已经解析完毕，是否需要重新解析
            ///留下与后续数据存放的逻辑一同实现
            ///update 2014-12-11已完成 逻辑在ProcessDocument中实现
            JsonDocEntity parseEntity = convertdoc.ProcessDocument();

            //若解析文档不成功，则直接返回
            if (parseEntity != null)
            {
                //若文档已经存储过，则跳过存储逻辑
                if (!convertdoc._docEntity.isStore)
                {
                    convertdoc._docEntity.isStore = true;
                    _repository.Update<DocumentEntity>(convertdoc._docEntity);
                    
                }
                return Json(parseEntity, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult download(string id)
        {
            DocumentEntity doc = _repository.Single<DocumentEntity>(id);
            return File(Path.Combine(doc.VirtualResourcesPath, doc.FileName), MimeUtils.GetMimeMapping(doc.FileExtension), Url.Encode(doc.FileName));
        }

    }
}
