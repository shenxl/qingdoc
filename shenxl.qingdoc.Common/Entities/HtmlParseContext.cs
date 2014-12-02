using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Common.Entities
{
    /// <summary>
    /// HTML内部解析类，待拓展
    /// </summary>
    public class HtmlParseContext
    {
        /// <summary>
        /// 记录解析后的原始HTml内容（暂时属性，视后续逻辑替代）
        /// </summary>
        public List<string> HtmlContent { get; set; }
        /// <summary>
        /// 记录解析为HTML后文档的Style文件位置
        /// </summary>
        public string StyleUrl { get; set; }
        /// <summary>
        /// 记录该文档解析后的总页数
        /// </summary>
        public int PageNumber { get; set; }
        
        /// <summary>
        /// 记录文档的具体内容
        /// </summary>
        public List<HtmlParseData> ParseContentList { get; set; }
        
        public HtmlParseContext()
        {
            ParseContentList = new List<HtmlParseData>();
            HtmlContent = new List<string>();
        }
    }
}
