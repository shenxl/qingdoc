using Aspose.Words.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shenxl.qingdoc.Helper
{
    public class HandleImageSaving : IImageSavingCallback
    {
        public void ImageSaving(ImageSavingArgs args)
        {
            if (args.ImageFileName.StartsWith("Aspose.Words."))
                args.ImageFileName = args.ImageFileName.Replace("Aspose.Words.", "");
        }
    }
}
