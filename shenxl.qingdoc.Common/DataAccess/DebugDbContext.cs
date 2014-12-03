using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shenxl.qingdoc.Common.DataAccess
{
    public class DebugDbContext
    {
        public List<IEntity> _entityList { get; set; }
        private static DebugDbContext _dbContent;

        private DebugDbContext()
        {
            _entityList = new List<IEntity>();     
        }
        public static DebugDbContext CreateDataContent()
        {
            if (_dbContent == null)
                _dbContent = new DebugDbContext();
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
