
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
        //protected string _filePath {get;set;}
        //protected string _outputPathForImage { get; set; }

        protected DocumentEntity _docEntity { get; set; }
        /// <summary>
        /// 记录转换好的HTML内容
        /// </summary>
        //protected List<HtmlContext> _convert2Html { get; set; }

        /// <summary>
        /// 将转换的HTML解析为Div
        /// </summary>
        protected static readonly RegexOptions REGEX_OPTIONS =
                    RegexOptions.Compiled | RegexOptions.IgnoreCase | 
                    RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

        protected static readonly Regex DIV_REGEX = new Regex(@"(?'div'<div\s*(.|\n)*?</div>)", REGEX_OPTIONS);
        private static readonly Regex DIV_IMAGE_REGEX = new Regex(@"<img\s*src=""(?'src'(.)*?)""\s*width=(.|\n)*?>", REGEX_OPTIONS);

        public ConvertDocument(DocumentEntity docEntity)
        {
            _docEntity = docEntity;
        }
        
        //public abstract List<HtmlContext> ParseHtmlForDocEntit();

        public abstract void ConvertToHtml();
        public abstract DocumentEntity ConvertHtmlToEntity();

    }
}
