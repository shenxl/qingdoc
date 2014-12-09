
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Document.ConvertComponent;
using shenxl.qingdoc.Document.Factory;

namespace shenxl.qingdoc.Document
{
    public class ConvertDocument
    {
        public DocumentEntity _docEntity { get; set; }
        public ConvertComponentType _docConvertComponent { get; set; }


        public ConvertDocument(DocumentEntity docEntity)
        {
            _docEntity = docEntity;
            _docConvertComponent =  ConvertComponentType.AsposeComponent;
        }

        public ConvertDocument(DocumentEntity docEntity, ConvertComponentType component)
        {
            _docEntity = docEntity;
            _docConvertComponent = component;
        }

        public JsonDocEntity ProcessDocument()
        {
            var processComponent = ComponentFactory.CreateComponent(_docEntity.Type, _docConvertComponent);
            return processComponent.ParseHtmlToEntity(processComponent.ConvertToHtml(_docEntity));
        }

    }
}
