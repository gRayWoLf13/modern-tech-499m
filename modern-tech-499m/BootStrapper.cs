using System;
using System.Reflection;
using Autofac;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.Repositories.Persistence;
using modern_tech_499m.Repositories.Persistence.Repositories;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m
{
    public static class BootStrapper
    {
        private static ILifetimeScope _rootScope;
        public static void Start()
        {
            if (_rootScope != null)
                return;
            var builder = new ContainerBuilder();

            var assemblies = new[] { Assembly.GetExecutingAssembly() };
            builder.RegisterType<UsersDatabaseViewModel>().AsSelf();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<GameInfoRepository>().As<IGameInfoRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<DatabaseContextFactory>().As<IDatabaseContextFactory>();

            _rootScope = builder.Build();
        }

        public static void Stop()
        {
            _rootScope.Dispose();
        }

        public static T Resolve<T>()
        {
            if (_rootScope == null)
                throw new Exception("Bootstrapper hasn't been started");
            return _rootScope.Resolve<T>();
        }
    }
}
