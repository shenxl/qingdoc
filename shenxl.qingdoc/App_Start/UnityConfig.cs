
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using shenxl.qingdoc.IoC;
using shenxl.qingdoc.Common.DataAccess;

namespace shenxl.qingdoc
{
    public class UnityConfig
    {
        public static void RegisterIOCContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IRepository, DebugRepository>();
            container.RegisterType<IControllerFactory, UnityControllerFactory>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}