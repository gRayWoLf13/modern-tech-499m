using System;
using Autofac;
using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.Repositories.Persistence;
using modern_tech_499m.Repositories.Persistence.Repositories;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m
{
    public static class BootStrapper
    {
        private static ILifetimeScope _rootScope;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Start()
        {
            if (_rootScope != null)
                return;
            var builder = new ContainerBuilder();

            builder.RegisterType<UsersDatabaseViewModel>().AsSelf();
            builder.RegisterType<AddUserViewModel>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<GameInfoRepository>().As<IGameInfoRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<DatabaseContextFactory>().As<IDatabaseContextFactory>();
            builder.RegisterType<PlayerSelectionViewModel>().As<IEntitySelectionViewModel<IPlayer>>();
            builder.RegisterType<GameInfoSelectionViewModel>().As<IEntitySelectionViewModel<GameInfo>>();

            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<RegisterViewModel>().AsSelf();
            builder.RegisterType<GameInfoSelectionPageViewModel>().AsSelf();
            builder.RegisterType<GamePageViewModel>().AsSelf();


            builder.RegisterType<ApplicationViewModel>().SingleInstance();


            _rootScope = builder.Build();
            _logger.Debug("Bootsrapper started");
        }

        public static void Stop()
        {
            _rootScope.Dispose();
            _logger.Debug("Bootstrapper stopped");
        }

        public static T Resolve<T>()
        {
            if (_rootScope == null)
                throw new Exception("Bootstrapper hasn't been started");
            return _rootScope.Resolve<T>();
        }
    }
}
