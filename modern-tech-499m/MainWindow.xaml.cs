using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using modern_tech_499m.Logic;
using modern_tech_499m.AILogic;

namespace modern_tech_499m
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController controller;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowPlayerWorkingMessage(IPlayer player)
        {
            currentPlayerInfo.Text = $"Waiting for player '{player.Name}' to make move";
        }

        private void StopPlayerWorkingMessage(IPlayer player)
        {
            currentPlayerInfo.Text = "";
        }

        private void ShowGameEnding(IPlayer player)
        {
            MessageBox.Show("Game ended");
        }

        private void UpdateFeld(string moveResult)
        {
            player1Cell0.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player1, 0);
            player1Cell1.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player1, 1);
            player1Cell2.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player1, 2);
            player1Cell3.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player1, 3);
            player1Cell4.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player1, 4);
            player1Cell5.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player1, 5);
            player1EndingCell.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player1, 6);

            player2Cell0.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player2, 0);
            player2Cell1.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player2, 1);
            player2Cell2.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player2, 2);
            player2Cell3.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player2, 3);
            player2Cell4.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player2, 4);
            player2Cell5.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player2, 5);
            player2EndingCell.Content = controller.gameLogic.GetCellValue(controller.gameLogic.Player2, 6);

            lastStatus.Text = moveResult;
        }

        private void Player1Cell0_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name;
            if (buttonName == null || controller == null)
                return;
            IPlayer player = buttonName.StartsWith("player1") ? controller.gameLogic.Player1 : controller.gameLogic.Player2;
            int index = int.Parse(buttonName.Substring(buttonName.Length - 1));
            (player as UserPlayer)?.MakeMove(index);
        }

        private void UserVsUser_Click(object sender, RoutedEventArgs e)
        {
            IPlayer player1, player2;
            string buttonName = (sender as Button).Name;
            switch(buttonName)
            {
                case "userVsUser":
                    {
                        player1 = new UserPlayer("User1");
                        player2 = new UserPlayer("User2");
                        break;
                    }
                case "userVsAI":
                    {
                        player1 = new UserPlayer("User1");
                        player2 = new AIPlayer("Bot2");
                        break;
                    }
                case "AIVsUser":
                    {
                        player1 = new AIPlayer("Bot1");
                        player2 = new UserPlayer("User2");
                        break;
                    }
                case "AIVsAI":
                default:
                    {
                        player1 = new AIPlayer("Bot1");
                        player2 = new AIPlayer("Bot2");
                        break;
                    }
            }
            GameLogic logic = new GameLogic(6, player1, player2, player1);
            controller?.StopGame();
            controller = new GameController(logic, UpdateFeld, ShowGameEnding,
                ShowPlayerWorkingMessage, StopPlayerWorkingMessage);
            controller.RunGame();
        }
    }
}