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
    public class DocumentEntity : Entity<Guid>
    {
        public Guid Id { get; set; }

        public String FilePath { get; set; }
        public String FileName { get; set; }
        public String FileMD5 { get; set; }
        public String VirtualResourcesPath { get; set; }
        public String ResourcesPath { get; set; }
        public String FileExtension { get; set; }
        

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
        /// HTML解析实体，待扩展
        /// </summary>
        public List<HtmlParseContext> HtmlDatas { get; set; }


        public  DocumentEntity()
        {
            HtmlDatas = new List<HtmlParseContext>();
        }
    }


    /// <summary>
    /// 文档类型的实体类
    /// </summary>
    public enum DocumentType { 
        Word = 0,
        Excel = 1,
        PowerPoint = 2,
        UnKnown = -1,
    }
}
