using shenxl.qingdoc.Common.DataAccess;
using shenxl.qingdoc.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shenxl.qingdoc.Controllers
{
    public class MongoDBTestController : Controller
    {
        //
        // GET: /MongoDBTest/

        private readonly IRepository _repository;

         public MongoDBTestController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            MongoDocument post = new MongoDocument();
            post.Title = "AAAA";
            post.Url = "BBB";
            post.Author = "Shen";
            _repository.Add<MongoDocument>(post);
            return null;
        }

    }
}
