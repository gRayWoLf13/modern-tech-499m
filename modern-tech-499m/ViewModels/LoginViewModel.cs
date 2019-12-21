using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.Security;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Private members


        #endregion


        #region Publilc properties

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// A flag indicating if login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        public SecureString Password { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// The command to register for a new user
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        /// <summary>
        /// The command to select the bot player instead of real player
        /// </summary>
        public ICommand SelectBotCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            //Create commands
            LoginCommand = new RelayParameterizedCommand( async parameter => await LoginAsync(parameter));
            RegisterCommand = new RelayCommand(async () => await RegisterAsync());
            SelectBotCommand = new RelayCommand(SelectBotPlayer);
        }

        #endregion

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The secure string passed in from the view</param>
        /// <returns></returns>
        public async Task LoginAsync(object parameter)
        {
            await RunCommand(() => LoginIsRunning, async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                var username = Username;
                var password = (parameter as IHavePassword).SecurePassword.Unsecure();

                //TODO Get password hash and create login method in repository
                MessageBox.Show($"{username}{Environment.NewLine}{password}");
            });
        }

        /// <summary>
        /// Takes the user to the register page
        /// </summary>
        /// <returns></returns>
        public async Task RegisterAsync()
        {
            // Go to register page
            //Right now we have to manually resolve a viewmodel for the page to set it's navigation source
            var registerViewModel = BootStrapper.Resolve<RegisterViewModel>();
            ViewModelLocator.ApplicationViewModel
                .GoToPageWithNavigationSource(ApplicationPage.Register, ApplicationPage.Login, this, registerViewModel);

            await Task.Delay(1);
        }

        /// <summary>
        /// Selects bot player and navigates back to game page
        /// </summary>
        public void SelectBotPlayer()
        {
            MessageBox.Show("Bot selected");
        }
    }
}
