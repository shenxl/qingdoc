using System.Configuration;
using MongoDB.Driver;

namespace shenxl.qingdoc.Common.Helpers
{
    public class MongoHelper
    {
        public MongoDatabase Database { get; private set; }
        public MongoHelper()
        {
            var con = new MongoConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
            var server = MongoServer.Create(con);
            this.Database = server.GetDatabase(con.DatabaseName);
        }

        public MongoHelper(string ConnectionString)
        {
            var con = new MongoConnectionStringBuilder(ConnectionString);
            var server = MongoServer.Create(con);
            this.Database = server.GetDatabase(con.DatabaseName);    
        }

        public MongoCollection<TModel> GetCollection<TModel>()
        {
            return Database.GetCollection<TModel>(typeof(TModel).Name.ToLower());
        }


    }
}
