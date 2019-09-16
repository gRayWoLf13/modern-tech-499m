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
        public MainWindow()
        {
            InitializeComponent();
            UserPlayer p1 = new UserPlayer(), p2 = new UserPlayer();
            //GameLogic l = new GameLogic(p1, p2, 0, 0, 0, 0, 0, 5, 2, 2, 2, 2, 2, 2);
            GameLogic l = new GameLogic(22, p1, p2);
            l.MakeMove(p1, 5);
        }
    }
}
