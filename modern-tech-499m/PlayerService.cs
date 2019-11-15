using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Logic;
using modern_tech_499m.ViewModels;
using modern_tech_499m.Views;

namespace modern_tech_499m
{
    static class PlayerService
    {
        public static IPlayer SelectPlayer()
        {
            var userWindow = new PlayerSelectionWindow();
            userWindow.ShowDialog();
            return (userWindow.DataContext as IPlayerSelectionViewModel).SelectedPlayer;
        }

        public static void AddNewUser()
        {
            var newPlayerWindow = new AddUserView();
            newPlayerWindow.ShowDialog();
        }
    }
}
