using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Pages
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : BasePage<WelcomePageViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public WelcomePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Specific constructor
        /// </summary>
        /// <param name="welcomePageViewModel">Specific viewmodel for the page</param>
        public WelcomePage(WelcomePageViewModel welcomePageViewModel) : base(welcomePageViewModel)
        {
            InitializeComponent();
        }
    }
}
