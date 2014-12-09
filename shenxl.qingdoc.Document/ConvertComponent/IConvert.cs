using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document.ConvertComponent
{
    interface IConvert
    {
        void Convert2HTml(IEntity entity);
        JsonDocEntity ParseHtmlToEntity(IEntity entity);
    }


}
