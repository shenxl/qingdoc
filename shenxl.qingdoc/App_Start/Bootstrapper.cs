
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using shenxl.qingdoc.IoC;
using shenxl.qingdoc.Common.DataAccess;
using System.Configuration;

namespace shenxl.qingdoc
{
    public class Bootstrapper
    {
        public static void RegisterIOCContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IRepository, DebugRepository>();
            container.RegisterType<IControllerFactory, UnityControllerFactory>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
        //public static IUnityContainer Init()
        //{
        //    var container = BuildUnityContainer();
        //    DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        //    return container;
        //}

        //private static IUnityContainer BuildUnityContainer()
        //{
        //    IUnityContainer container = new UnityContainer();
        //    UnityConfigurationSection configuration = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
        //    configuration.Configure(container);
        //    return container;
        //}

    }
}