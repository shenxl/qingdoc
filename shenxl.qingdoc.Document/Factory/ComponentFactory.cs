using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document.ConvertComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Document.Factory
{
    public class ComponentFactory
    {
        public static BaseComponent CreateComponent(DocumentType documenttype ,ConvertComponentType componenttype)
        {
            switch (documenttype)
            {
                case DocumentType.Word:
                    if (componenttype == ConvertComponentType.AsposeComponent)
                        return new AsposeWordComponent();
                    else if (componenttype == ConvertComponentType.MSComponent)
                        return new MSWordComponent();
                    return null;
                case DocumentType.Excel:
                    if (componenttype == ConvertComponentType.AsposeComponent)
                        return new AsposeExcelComponent();
                    else if (componenttype == ConvertComponentType.MSComponent)
                        return new MSExcelComponent();
                    return null;
                case DocumentType.PowerPoint:
                    if (componenttype == ConvertComponentType.AsposeComponent)
                        return new AsposePPTComponent();
                    else if (componenttype == ConvertComponentType.MSComponent)
                        return new MSPPTComponent();
                    return null;
                default:
                    return null;
            }
        }
    }
}
