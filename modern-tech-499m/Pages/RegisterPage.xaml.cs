using System.Security;
using System.Windows;
using modern_tech_499m.Security;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class RegisterPage : BasePage<RegisterViewModel>, IHavePassword
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Specific constructor
        /// </summary>
        /// <param name="registerViewModel">Specific viewmodel for the page</param>
        public RegisterPage(RegisterViewModel registerViewModel) : base(registerViewModel)
        {
            InitializeComponent();
        }

        /// <summary>
        /// The secure password for this login page
        /// </summary>
        public SecureString SecurePassword => PasswordText.SecurePassword;

        //Still testing...
        private void PasswordText_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var cnt = PasswordTextPlaceholder.Content;
            ((RegisterViewModel) DataContext).NewUser.PasswordHash = SecurePassword.Unsecure().GetStringHash();
            ((RegisterViewModel) DataContext).OnPropertyChanged("PasswordHash");
            cnt = PasswordTextPlaceholder.Content;
            cnt = PasswordTextPlaceholder.Content;
        }
    }
}
