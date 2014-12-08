using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using shenxl.qingdoc.Common.Entities;
using shenxl.qingdoc.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenxl.qingdoc.Common.DataAccess
{
    public class MongoDbRepository : IRepository
    {

        private readonly MongoHelper _entitys;

        public MongoDbRepository()
        {
            _entitys = new MongoHelper();
        }

        public void Add<TModel>(TModel instance) where TModel : class, Entities.IEntity
        {
            _entitys.GetCollection<TModel>().Insert<TModel>(instance, WriteConcern.Acknowledged);
        }

        public void Add<TModel>(IEnumerable<TModel> instances) where TModel : class, Entities.IEntity
        {
            foreach (var instance in instances)
            {
                _entitys.GetCollection<TModel>().Save<TModel>(instance);
            }
        }

        public IQueryable<TModel> All<TModel>(params string[] includePaths) where TModel : class, Entities.IEntity
        {
            return _entitys.GetCollection<TModel>().FindAll().AsQueryable<TModel>() as IQueryable<TModel>;
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
            var query = _entitys.GetCollection<TModel>().FindAll().AsQueryable<TModel>();
            return query.Where(item=>item.Key.Equals(key)).SingleOrDefault();
            //var collection = _entitys.GetCollection<TModel>();
            //return collection.Find(MongoDB.Driver.Builders.Query.EQ("key", key as BsonValue)).SingleOrDefault();
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

        public void Update<TModel>(TModel instance,string updatename,object updateobj) where TModel : class, IEntity
        {
            //var collection = _entitys.GetCollection<TModel>();
            //collection.Update(MongoDB.Driver.Builders.Query.EQ("key", instance.Key),
            //        MongoDB.Driver.Builders.Update.PushWrapped(updatename, updateobj));
            var query = MongoDB.Driver.Builders.Query.EQ("Key", instance.Key);
            var itemDoc = updateobj.ToBsonDocument();
            var collection = _entitys.GetCollection<TModel>();
            var update = MongoDB.Driver.Builders.Update.AddToSet(updatename, itemDoc); 
            collection.Update(query, update); 
   
        }


        public void Update<TModel>(TModel instance) where TModel : class, IEntity
        {
             var collection = _entitys.GetCollection<TModel>();
             collection.Update(MongoDB.Driver.Builders.Query<TModel>.EQ(e => e.Key, instance.Key),
                     MongoDB.Driver.Builders.Update<TModel>.Replace(instance), WriteConcern.Acknowledged);
        }
    }
}
