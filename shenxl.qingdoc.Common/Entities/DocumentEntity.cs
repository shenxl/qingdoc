using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Common.Entities
{
    /// <summary>
    /// 继承Entity实体
    /// </summary>
    public class DocumentEntity : Entity<ObjectId>
    {
        public String FilePath { get; set; }
        public String FileName { get; set; }
        public String FileMD5 { get; set; }
        public String VirtualResourcesPath { get; set; }
        public String ResourcesPath { get; set; }
        public String FileExtension { get; set; }

        public bool isConvert { get; set; }
        public string ConvertError { get; set; }

        public bool isParse { get; set; }
        public bool isStore { get; set; }
        /// <summary>
        /// 构造函数，保证引用属性初始化
        /// </summary>
        public DocumentEntity()
        {
            HtmlData = new HtmlParseContext();
        }

        public String ImageFolder
        {
            get {
                return "Image";
            }
        }
        public DateTime UploadTime { get; set; }
        public DateTime ConvertCompleteTime { get; set; }
        /// <summary>
        /// 文件类型属性
        /// </summary>
        public DocumentType Type
        {
            get
            {
                var extentesion = this.FileExtension;
                if (extentesion.Equals(".wpt") || extentesion.Equals(".doc") || extentesion.Equals(".docx")
                     || extentesion.Equals(".dotx") || extentesion.Equals(".dot") || extentesion.Equals(".wps"))
                {
                    return DocumentType.Word;
                }
                else if (extentesion.Equals(".et") || extentesion.Equals(".ett") || extentesion.Equals(".xlt")
                     || extentesion.Equals(".xls") || extentesion.Equals(".xlsx"))
                {
                    return DocumentType.Excel;
                }
                else if (extentesion.Equals(".ppt") || extentesion.Equals(".pptx") || extentesion.Equals(".pot")
                    || extentesion.Equals(".dps") || extentesion.Equals(".dpt"))
                {
                    return DocumentType.PowerPoint;
                }
                return DocumentType.UnKnown;
            }
        }
        /// <summary>
        /// HTML解析实体
        /// </summary>
        public HtmlParseContext HtmlData { get; set; }
        
    }


    /// <summary>
    /// 文档类型的实体类
    /// </summary>
    public enum DocumentType { 
        Word = 1,
        Excel = 3,
        PowerPoint = 2,
        UnKnown = 255,
    }
}
