using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Common.Entities
{
    public class DocumentEntity : IEntity
    {
        public Guid Id { get; set; }

        public String FilePath { get; set; }
        public String FileName { get; set; }
        public String FileMD5 { get; set; }
        public String ResourcesPath { get; set; }
        public String FileExtension { get; set; }

        public DateTime UploadTime { get; set; }
        public DateTime ConvertCompleteTime { get; set; }


        public List<HtmlParseContext> HtmlData { get; set; }
        
    }
}
