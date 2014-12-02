using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Common.DataAccess
{
    public class DebugRepository : IRepository
    {
        private readonly DbContext _context;

        public DebugRepository()
        {
            _context = DbContext.CreateDataContent();
        }

        public void Add<TModel>(TModel instance) where TModel : class, Entities.IEntity
        {
            _context.Add(instance);
        }

        public void Add<TModel>(IEnumerable<TModel> instances) where TModel : class, Entities.IEntity
        {
            throw new NotImplementedException();
        }

        public IQueryable<TModel> All<TModel>(params string[] includePaths) where TModel : class, Entities.IEntity
        {
            throw new NotImplementedException();
        }

        public void Delete<TModel>(object key) where TModel : class, Entities.IEntity
        {
            throw new NotImplementedException();
        }

        public void Delete<TModel>(TModel instance) where TModel : class, Entities.IEntity
        {
            throw new NotImplementedException();
        }

        public void Delete<TModel>(System.Linq.Expressions.Expression<Func<TModel, bool>> predicate) where TModel : class, Entities.IEntity
        {
            throw new NotImplementedException();
        }

        public TModel Single<TModel>(object key) where TModel : class, Entities.IEntity
        {
            return _context.Get(key) as TModel;
        }

        public TModel Single<TModel>(System.Linq.Expressions.Expression<Func<TModel, bool>> predicate, params string[] includePaths) where TModel : class, Entities.IEntity
        {
            throw new NotImplementedException();
        }

        public IQueryable<TModel> Query<TModel>(System.Linq.Expressions.Expression<Func<TModel, bool>> predicate, params string[] includePaths) where TModel : class, Entities.IEntity
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
