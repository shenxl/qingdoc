using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document.ConvertComponent
{
    public abstract class BaseComponent{

        /// <summary>
        /// 将转换的HTML解析为Div
        /// </summary>
        protected static readonly RegexOptions REGEX_OPTIONS =
                    RegexOptions.Compiled | RegexOptions.IgnoreCase |
                    RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

        /// <summary>
        /// Doc转换到HTML
        /// </summary>
        public abstract DocumentEntity ConvertToHtml(DocumentEntity _docEntity);
        /// <summary>
        /// 解析文档内容为前端显示的Json实体
        /// </summary>
        /// <returns></returns>
        public abstract JsonDocEntity ParseHtmlToEntity(DocumentEntity _docEntity);
    }

    public enum ConvertComponentType
    {
        MSComponent = 1,
        AsposeComponent = 2,
        WPSComponent = 3
    }
}
