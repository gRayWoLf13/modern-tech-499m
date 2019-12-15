using System.Windows.Controls;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Pages
{
    /// <summary>
    /// Interaction logic for GameInfoSelectionPage.xaml
    /// </summary>
    public partial class GameInfoSelectionPage : BasePage<GameInfoSelectionPageViewModel>
    {
        public GameInfoSelectionPage()
        {
            InitializeComponent();
            //??
            DataContext = ViewModel;
        }
    }
}
