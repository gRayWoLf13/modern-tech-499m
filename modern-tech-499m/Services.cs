using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.ViewModels;
using modern_tech_499m.Views;

namespace modern_tech_499m
{
    static class Services
    {
        public static IPlayer SelectPlayer()
        {
            var userWindow = new PlayerSelectionWindow();
            userWindow.ShowDialog();
            return (userWindow.DataContext as IEntitySelectionViewModel<IPlayer>).SelectedEntity;
        }

        public static void AddNewUser()
        {
            var newPlayerWindow = new AddUserView();
            newPlayerWindow.ShowDialog();
        }

        public static GameInfo SelectGameInfo()
        {
            var gameInfo = new GameInfoSelectionWindow();
            gameInfo.ShowDialog();
            return (gameInfo.DataContext as IEntitySelectionViewModel<GameInfo>).SelectedEntity;
        }
    }
}
