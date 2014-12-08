
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using shenxl.qingdoc.Common.Entities;

namespace shenxl.qingdoc.Document
{
    public abstract class ConvertDocument
    {
        public DocumentEntity _docEntity { get; set; }

        /// <summary>
        /// 将转换的HTML解析为Div
        /// </summary>
        protected static readonly RegexOptions REGEX_OPTIONS =
                    RegexOptions.Compiled | RegexOptions.IgnoreCase | 
                    RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

        public ConvertDocument(DocumentEntity docEntity)
        {
            _docEntity = docEntity;
        }
        /// <summary>
        /// Doc转换到HTML
        /// </summary>
        public abstract void ConvertToHtml();
        /// <summary>
        /// 解析文档内容为前端显示的Json实体
        /// </summary>
        /// <returns></returns>
        public abstract JsonDocEntity ParseHtmlToEntity();

    }
}
