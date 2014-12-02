
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
        protected DocumentEntity _docEntity { get; set; }

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
        
        public abstract void ConvertToHtml();
        public abstract JsonDocEntity ConvertHtmlToEntity();

    }
}
