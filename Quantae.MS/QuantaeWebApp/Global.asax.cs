﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Quantae.Engine;
using System.Configuration;
using log4net;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace QuantaeWebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //ILog logger = LogManager.GetLogger(typeof(MvcApplication));

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.Register<LoggingService>(_ => new LoggingService()).As<ILoggingService>();
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            QuantaeEngine.Init(ConfigurationManager.AppSettings["MONGOHQ_DB"], ConfigurationManager.AppSettings["MONGOHQ_URL"]);

            SessionManager.Load();

            var logger = DependencyResolver.Current.GetService<ILoggingService>().Logger;
            logger.Info("app");
        }
    }
}