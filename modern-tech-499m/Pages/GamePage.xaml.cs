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
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : BasePage<GamePageViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Specific constructor
        /// </summary>
        /// <param name="gamePageViewModel">Specific viewmodel for the page</param>
        public GamePage(GamePageViewModel gamePageViewModel) : base(gamePageViewModel)
        {
            InitializeComponent();
        }
    }
}
