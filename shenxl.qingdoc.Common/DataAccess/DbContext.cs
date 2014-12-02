using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shenxl.qingdoc.Common.DataAccess
{
    public class DbContext
    {
        public List<IEntity> _entityList { get; set; }
        private static DbContext _dbContent;

        private DbContext()
        {
            _entityList = new List<IEntity>();     
        }
        public static DbContext CreateDataContent()
        {
            if (_dbContent == null)
                _dbContent = new DbContext();
            return _dbContent;
        }

        public void Add(IEntity doc)
        {
            _entityList.Add(doc);
        }

        public IEntity Get(object id)
        {
            return _entityList.Where(item => item.Key.Equals(id)).SingleOrDefault();
        }

        public IQueryable<IEntity> GetALL()
        {
            return _entityList.Where<IEntity>(item => item.Key != null).AsQueryable();
        }
    }
}
