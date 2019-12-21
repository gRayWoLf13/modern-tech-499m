using System.Windows.Controls;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Pages
{
    /// <summary>
    /// Interaction logic for GameInfoSelectionPage.xaml
    /// </summary>
    public partial class GameInfoSelectionPage : BasePage<GameInfoSelectionPageViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameInfoSelectionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Specific constructor
        /// </summary>
        /// <param name="gameInfoSelectionPageViewModel">Specific viewmodel for the page</param>
        public GameInfoSelectionPage(GameInfoSelectionPageViewModel gameInfoSelectionPageViewModel) : base(gameInfoSelectionPageViewModel)
        {
            InitializeComponent();
        }
    }
}
