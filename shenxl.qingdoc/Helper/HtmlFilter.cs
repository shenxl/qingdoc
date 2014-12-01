using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Helper
{
    public class HtmlMatcher
    {
        private static readonly RegexOptions REGEX_OPTIONS =
        RegexOptions.Compiled | RegexOptions.IgnoreCase | 
        RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

        public HtmlMatcher(){}

        private static readonly Regex STYLE_REGEX = new Regex(@"(?'style'(?<=<style>)(.|\n)*?(?=</style>))", REGEX_OPTIONS);
        private static readonly Regex DIV_REGEX = new Regex(@"(?'div'<div\s*class=(?'replace'\w*)\s*style=[.*|>]?(.|\n)*?</div>)", REGEX_OPTIONS);
        private static readonly Regex IMAGE_REGEX = new Regex(@"<img(.|\n)*?src=""(?'src'(.)*?)"">", REGEX_OPTIONS);


        private static readonly Regex DIV_ASPOSE_REGEX = new Regex(@"(?'div'<div\s*(.|\n)*?</div>)", REGEX_OPTIONS);
        private static readonly Regex DIV_IMAGE_REGEX = new Regex(@"<img\s*src=""(?'src'(.)*?)""\s*width=(.|\n)*?>", REGEX_OPTIONS);
        //private static readonly Regex BREAK_REGEX = new Regex(@"<br\s*clear=all\s*style=\s*'(?'a'[a-zA-Z|\-|:]+)[^']*", REGEX_OPTIONS);
        

        public string GetStyle(string html)
        {
            // 对每个HTML标记进行替换?
            var validStyleMatch = STYLE_REGEX.Match(html);
            if (!validStyleMatch.Success) return "";
            var style = validStyleMatch.Groups["style"].Value;
            return style;
        }

        public List<string> GetDivAspose(string html)
        {
            // 对每个HTML标记进行替换?
            var attrMatches = DIV_ASPOSE_REGEX.Matches(html);
            var divarray = new List<string>();
            foreach (Match matcher in attrMatches)
            {
                var div = matcher.Groups["div"].Value;
                var imageMatches = DIV_IMAGE_REGEX.Matches(div);
                foreach (Match iamge in imageMatches)
                {
                    var iamgesrc = iamge.Groups["src"].Value;
                    var iamgearray = iamge.Groups["src"].Value.Split('\\');
                    var iamgename = iamgearray[iamgearray.Length - 1];
                    var iamgepath = "\\File\\output\\" + iamgename;
                    div = div.Replace(iamgesrc, iamgepath);

                }
                //var breaktag = matcher.Groups["break"].Value;
                //if (breaktag == "page-break-before:always")
                //    div = div.Replace(raplace, "\"scroll-page HorizontalSection\"");
                //else
                //    div = div.Replace(raplace, "\"scroll-page VerticalSection\"");
                divarray.Add(div);
            }
            return divarray;
        }



        public List<string> GetDiv(string html)
        {
            // 对每个HTML标记进行替换?
            var attrMatches = DIV_REGEX.Matches(html);
            //var validAttributes = attrMatches.Select(m => m.Groups["div"].Value)
            //            .Where(s => !String.IsNullOrEmpty(s)).ToArray();
            var divarray = new List<string>();
            foreach (Match matcher in attrMatches)
            {
                var div = matcher.Groups["div"].Value;
                var raplace = matcher.Groups["replace"].Value;
                
                div = div.Replace(raplace, "\"scroll-page HorizontalSection\"");
                var imageMatches = IMAGE_REGEX.Matches(div);
                foreach (Match iamge in imageMatches)
                {
                    var iamgesrc = iamge.Groups["src"].Value;
                    var iamgepath = "\\File\\output\\" + iamgesrc.Replace("/","\\");
                    div = div.Replace(iamgesrc, iamgepath);

                }
                //var breaktag = matcher.Groups["break"].Value;
                //if (breaktag == "page-break-before:always")
                //    div = div.Replace(raplace, "\"scroll-page HorizontalSection\"");
                //else
                //    div = div.Replace(raplace, "\"scroll-page VerticalSection\"");
                divarray.Add(div);
            }
            return divarray;
        }

    }
}
