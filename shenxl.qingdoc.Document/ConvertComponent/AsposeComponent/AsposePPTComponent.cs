using Aspose.Slides;
using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document.ConvertComponent
{
    public class AsposePPTComponent :  BaseComponent
    {
        public override DocumentEntity ConvertToHtml(DocumentEntity _docEntity)
        {
            var BigPath = Path.Combine(_docEntity.ResourcesPath, "Big");
            if (!Directory.Exists(BigPath))
                Directory.CreateDirectory(BigPath);
            var SmallPath = Path.Combine(_docEntity.ResourcesPath, "Small");
            if (!Directory.Exists(SmallPath))
                Directory.CreateDirectory(SmallPath);
            if (!_docEntity.isConvert && String.IsNullOrEmpty(_docEntity.ConvertError))
            {
                try
                {
                    Presentation ppt = new Presentation(_docEntity.FilePath);
                    _docEntity.HtmlData.PageNumber = ppt.Slides.Count;
                    var Index = 1;
                    foreach (Slide sld in ppt.Slides)
                    {
                        Image bigimg = sld.GetThumbnail(1f, 1f);
                        bigimg.Save(Path.Combine(BigPath, String.Format("Thumbnail{0}.jpg", Index)), System.Drawing.Imaging.ImageFormat.Jpeg);

                        Image smallimg = sld.GetThumbnail();
                        smallimg.Save(Path.Combine(SmallPath, String.Format("Thumbnail{0}.jpg", Index)), System.Drawing.Imaging.ImageFormat.Jpeg);

                        Index++;
                    }
                }
                catch (Exception e)
                {
                    _docEntity.isConvert = false;
                    _docEntity.ConvertError = e.Message;
                }

            }
            return _docEntity;
        }

        public override JsonDocEntity ParseHtmlToEntity(DocumentEntity _docEntity)
        {
            if (!_docEntity.isParse)
            {
                var VirtualResourcesPath = _docEntity.VirtualResourcesPath;
                for (int count = 1; count < _docEntity.HtmlData.PageNumber + 1; count++)
                {
                    HtmlParseData divcontent = new HtmlParseData();
                    divcontent.pagecount = count;
                    divcontent.thumbUrl = VirtualResourcesPath + "/Small/" + String.Format("Thumbnail{0}.jpg", count);
                    divcontent.url = VirtualResourcesPath + "/Big/" + String.Format("Thumbnail{0}.jpg", count);
                    _docEntity.HtmlData.ParseContentList.Add(divcontent);
                }
                _docEntity.ConvertCompleteTime = DateTime.Now;
                _docEntity.isParse = true;
            }
            return JsonDocEntity.Convert(_docEntity);
        }
    }
}
