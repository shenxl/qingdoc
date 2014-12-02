using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Common.Entities
{
    public class JsonDocEntity
    {
        public string FileName { get; set; }
        public int PageCount { get; set; }
        public string FileExtension { get; set; }
        public string FileMD5 { get; set; }
        public string Key { get; set; }
        public List<HtmlParseData> htmlContent { get; set; }
        public string code { get; set; }
        public string styleUrl { get; set; }
        /// <summary>
        /// 将构造的Document转换为Json实体，方便后续的Web传输。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 
        public static JsonDocEntity Convert(DocumentEntity entity)
        {
            return new JsonDocEntity()
            {
                FileName = entity.FileName,
                PageCount = entity.HtmlData.PageNumber,
                FileExtension = entity.FileExtension,
                FileMD5 = entity.FileMD5,
                styleUrl = entity.HtmlData.StyleUrl,
                Key = entity.Key,
                htmlContent = entity.HtmlData.ParseContentList,
                code = "success"
            };
        }
    }
}
