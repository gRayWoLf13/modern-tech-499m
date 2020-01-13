using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Pages
{
    /// <summary>
    /// Interaction logic for UsersDatabasePage.xaml
    /// </summary>
    public partial class UsersDatabasePage : BasePage<UsersDatabasePageViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UsersDatabasePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Specific constructor
        /// </summary>
        /// <param name="usersDatabasePageViewModel">Specific viewmodel for the page</param>
        public UsersDatabasePage(UsersDatabasePageViewModel usersDatabasePageViewModel) : base(usersDatabasePageViewModel)
        {
            InitializeComponent();
        }
    }
}
