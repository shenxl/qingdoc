using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document.Util
{
    public class MimeUtils
    {
        private static Hashtable _mimeMappingTable;

        private static void AddMimeMapping(string extension, string MimeType)
        {
            MimeUtils._mimeMappingTable.Add(extension, MimeType);
        }

        public static string GetMimeMapping(string FileExtension)
        {
            var text = (string)MimeUtils._mimeMappingTable[FileExtension];
            return text;
        }

        static MimeUtils()
        {
            MimeUtils._mimeMappingTable = new Hashtable(30, StringComparer.CurrentCultureIgnoreCase);
            MimeUtils.AddMimeMapping(".dot", "application/msword");
            MimeUtils.AddMimeMapping(".doc", "application/msword");
            MimeUtils.AddMimeMapping(".wps", "application/msword");
            MimeUtils.AddMimeMapping(".wpt", "application/msword");
            MimeUtils.AddMimeMapping(".docx", "application/msword");

            MimeUtils.AddMimeMapping(".xls", "application/vnd.ms-excel");
            MimeUtils.AddMimeMapping(".xlt", "application/vnd.ms-excel");
            MimeUtils.AddMimeMapping(".xlsx", "application/vnd.ms-excel");
            MimeUtils.AddMimeMapping(".et", "application/vnd.ms-excel");
            MimeUtils.AddMimeMapping(".ett", "application/vnd.ms-excel");

            MimeUtils.AddMimeMapping(".dps", "application/vnd.ms-powerpoint");
            MimeUtils.AddMimeMapping(".dpt", "application/vnd.ms-powerpoint");
            MimeUtils.AddMimeMapping(".ppt", "application/vnd.ms-powerpoint");
            MimeUtils.AddMimeMapping(".pptx", "application/vnd.ms-powerpoint");
            MimeUtils.AddMimeMapping(".pot", "application/vnd.ms-powerpoint");
            MimeUtils.AddMimeMapping(".*", "application/octet-stream");
        }
    }
}
