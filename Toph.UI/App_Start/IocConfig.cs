using System;
using System.Web.Http;
using System.Web.Mvc;
using StructureMap;
using Toph.Common;
using Toph.Common.DataAccess;
using Toph.UI.Infrastructure;

namespace Toph.UI
{
    public static class IocConfig
    {
        public static void Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<INHibernateSessionFactoryHelper>().Singleton().Use<NHibernateSessionFactoryHelper>();

                x.For<INHibernateUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<NHibernateUnitOfWork>();
                x.For<IUnitOfWork>().Use(ctx => ctx.GetInstance<INHibernateUnitOfWork>());

                x.For<IRepository>().Use<NHibernateRepository>();

                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("Toph"));
                    scan.WithDefaultConventions();
                });
            });

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(ObjectFactory.Container);

            Ioc.Initialize(new StructureMapDependencyResolver(ObjectFactory.Container));
        }
    }
}
