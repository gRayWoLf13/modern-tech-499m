using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        UserPlayer player1, player2;
        GameLogic logic;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init()
        {
            player1 = new UserPlayer();
            player2 = new UserPlayer();
            //logic = new GameLogic(6, player1, player2);
            logic = new GameLogic(player1, player2, new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 3, 4, 5, 6 });
        }

        private void UpdateCellsValues()
        {
            player1Cell0.Content = logic.GetCellValue(player1, 0);
            player1Cell1.Content = logic.GetCellValue(player1, 1);
            player1Cell2.Content = logic.GetCellValue(player1, 2);
            player1Cell3.Content = logic.GetCellValue(player1, 3);
            player1Cell4.Content = logic.GetCellValue(player1, 4);
            player1Cell5.Content = logic.GetCellValue(player1, 5);
            player1EndingCell.Content = logic.GetCellValue(player1, 6);

            player2Cell0.Content = logic.GetCellValue(player2, 0);
            player2Cell1.Content = logic.GetCellValue(player2, 1);
            player2Cell2.Content = logic.GetCellValue(player2, 2);
            player2Cell3.Content = logic.GetCellValue(player2, 3);
            player2Cell4.Content = logic.GetCellValue(player2, 4);
            player2Cell5.Content = logic.GetCellValue(player2, 5);
            player2EndingCell.Content = logic.GetCellValue(player2, 6);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            UpdateCellsValues();
        }

        private void Player1Cell0_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name;
            IPlayer player = buttonName.StartsWith("player1") ? player1 : player2;
            int index = int.Parse(buttonName.Substring(buttonName.Length - 1));
            try
            {
                MoveResult result = logic.MakeMove(player, index);
                UpdateCellsValues();
                lastStatus.Text = Enum.GetName(typeof(MoveResult), result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }
    }
}
