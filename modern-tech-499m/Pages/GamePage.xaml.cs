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
