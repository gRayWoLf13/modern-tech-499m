using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.Security;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {

        #region Private members

        private readonly IUserRepository _userRepository;

        #endregion


        #region Publilc properties

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// A flag indicating if register command is running
        /// </summary>
        public bool RegisterIsRunning { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The patronymic of the user
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// The birth date of the user
        /// </summary>
        public DateTime? BirthDate { get; set; }

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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            //Create commands
            RegisterCommand = new RelayParameterizedCommand( async parameter => await Register(parameter));
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        #endregion

        /// <summary>
        /// Attempts to register a new user
        /// </summary>
        /// <param name="parameter">The secure string passed in from the view</param>
        /// <returns></returns>
        public async Task Register(object parameter)
        {
            await RunCommand(() => RegisterIsRunning, async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                var username = Username;
                var passwordHash = (parameter as IHavePassword).SecurePassword.Unsecure().GetStringHash();
                var firstName = FirstName;
                var lastName = LastName;
                var patronymic = Patronymic;
                var birthDate = BirthDate;

                var user = new User
                {
                    Username = username,
                    BirthDate = birthDate.HasValue ? birthDate.Value : DateTime.Today,
                    FirstName = firstName,
                    LastName = lastName,
                    Patronymic = patronymic,
                    PasswordHash = passwordHash
                };

                var registerResult = await _userRepository.RegisterUser(user);

                switch (registerResult.registerResult)
                {
                    case RegisterResult.UsernameAlreadyExists:
                        MessageBox.Show("Selected username already exists");
                        break;
                    case RegisterResult.Success:
                        MessageBox.Show("Registration successful, returning to the game page");
                        (NavigationSourcePageViewModel as GamePageViewModel).CurrentPlayerLoggingAction(
                            new UserPlayer(registerResult.registeredUser.Username, registerResult.registeredUser, Guid.Empty));
                        ViewModelLocator.ApplicationViewModel.ReturnToNavigationPageSource(NavigationSourcePage,
                            NavigationSourcePageViewModel);
                        break;
                }
            });
        }

        /// <summary>
        /// Takes the user to the login page
        /// </summary>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            // Go to login page
            //Right now we have to manually resolve a viewmodel for the page to set it's navigation source

            //In this case we are navigating to login page and passing current NavigationSourcePageViewModel and NavigationSourcePage(which was passed from the game page)
            //So, in case of successful logging, the login page will navigate back to the game page (not the register page)
            var loginViewModel = BootStrapper.Resolve<LoginViewModel>();
            ViewModelLocator.ApplicationViewModel
                .GoToPageWithNavigationSource(ApplicationPage.Login, NavigationSourcePage,
                    NavigationSourcePageViewModel, loginViewModel);
        }
    }
}
