using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shenxl.qingdoc.Common.Entities
{
    public class HtmlParseData
    {
        /// <summary>
        /// 文字、演示：page数，表格：worksheet数目
        /// </summary>
        public int pagecount { get; set; }
        
        /// <summary>
        /// 文字：div内容，演示：image内容，表格：Div/Table内容
        /// </summary>
        public string content { get; set; }
        
        /// <summary>
        /// 提供给演示的页面视图
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 提供给表格的worksheet名称
        /// </summary>
        public string title { get; set; }


    }
}
