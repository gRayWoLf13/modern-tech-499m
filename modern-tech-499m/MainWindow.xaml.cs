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

        private void Init()
        {
            IPlayer player1 = new UserPlayer(), player2 = new UserPlayer();
            var logic = new GameLogic(6, player1, player2);
            //var logic = new GameLogic(player1, player2, new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 3, 4, 5, 6 });
            controller = new GameController(player1, player2, player1, logic, UpdateFeld, ShowGameEnding,
                ShowAIWorkingMessage, StopAIWorkingMessage, ShowWaitingForUserMessage, StopWaitingForUserMessage);
            controller.RunGame();
        }

        private void ShowAIWorkingMessage()
        {
            //show something
        }

        private void StopAIWorkingMessage()
        {
            //show something
        }

        private void ShowWaitingForUserMessage()
        {
            //show something
        }

        private void StopWaitingForUserMessage()
        {
            //show something
        }

        private void ShowGameEnding()
        {
            MessageBox.Show("Game ended");
        }

        private void UpdateFeld(string moveResult)
        {
            player1Cell0.Content = controller.gameLogic.GetCellValue(controller.player1, 0);
            player1Cell1.Content = controller.gameLogic.GetCellValue(controller.player1, 1);
            player1Cell2.Content = controller.gameLogic.GetCellValue(controller.player1, 2);
            player1Cell3.Content = controller.gameLogic.GetCellValue(controller.player1, 3);
            player1Cell4.Content = controller.gameLogic.GetCellValue(controller.player1, 4);
            player1Cell5.Content = controller.gameLogic.GetCellValue(controller.player1, 5);
            player1EndingCell.Content = controller.gameLogic.GetCellValue(controller.player1, 6);

            player2Cell0.Content = controller.gameLogic.GetCellValue(controller.player2, 0);
            player2Cell1.Content = controller.gameLogic.GetCellValue(controller.player2, 1);
            player2Cell2.Content = controller.gameLogic.GetCellValue(controller.player2, 2);
            player2Cell3.Content = controller.gameLogic.GetCellValue(controller.player2, 3);
            player2Cell4.Content = controller.gameLogic.GetCellValue(controller.player2, 4);
            player2Cell5.Content = controller.gameLogic.GetCellValue(controller.player2, 5);
            player2EndingCell.Content = controller.gameLogic.GetCellValue(controller.player2, 6);

            lastStatus.Text = moveResult;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Player1Cell0_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name;
            IPlayer player = buttonName.StartsWith("player1") ? controller.player1 : controller.player2;
            int index = int.Parse(buttonName.Substring(buttonName.Length - 1));
            controller.SetLastCell(player, index);
        }
    }
}