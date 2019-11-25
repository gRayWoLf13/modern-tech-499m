using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.ViewModels;
using modern_tech_499m.Views;
using NLog;

namespace modern_tech_499m
{
    static class Services
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public static IPlayer SelectPlayer()
        {
            _logger.Debug("Select player method called");
            var userWindow = new PlayerSelectionWindow();
            userWindow.ShowDialog();
            return (userWindow.DataContext as IEntitySelectionViewModel<IPlayer>).SelectedEntity;
        }

        public static void AddNewUser()
        {
            _logger.Debug("Add new user method called");
            var newPlayerWindow = new AddUserView();
            newPlayerWindow.ShowDialog();
        }

        public static GameInfo SelectGameInfo()
        {
            _logger.Debug("Select game info method called");
            var gameInfo = new GameInfoSelectionWindow();
            gameInfo.ShowDialog();
            return (gameInfo.DataContext as IEntitySelectionViewModel<GameInfo>).SelectedEntity;
        }
    }
}
