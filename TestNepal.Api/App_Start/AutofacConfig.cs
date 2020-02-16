using Autofac;
using Autofac.Integration.Mvc;
using TestNepal.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;
using TestNepal.Api.Helpers;
using AutoMapper;
using TestNepal.API.Mappers;
using Autofac.Core;

namespace TestNepal.Api
{
    public static class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>()
                .WithParameter("tenantId",AuthHelper.GetCurrentTenantId())
                //.WithParameter("userId", AuthHelper.GetCurrentUserId())
                .WithParameter(
                 new ResolvedParameter(
                   (pi, ctx) => pi.ParameterType == typeof(Guid) && pi.Name == "userId",
                   (pi, ctx) => AuthHelper.GetCurrentUserId()))
                .InstancePerRequest();

            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerRequest()
                .WithParameter("configurationProvider", mapperConfig);

            builder.RegisterAssemblyTypes(Assembly.Load("TestNepal.Service"))
                     .Where(t => t.Name.EndsWith("Service"))
                     .AsImplementedInterfaces()
                     .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("TestNepal.Repository"))

                     .Where(t => t.Name.EndsWith("Repository"))

                     .AsImplementedInterfaces()

                     .InstancePerLifetimeScope();
            var container = builder.Build();
            

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}