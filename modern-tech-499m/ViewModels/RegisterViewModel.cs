using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.Security;
using modern_tech_499m.ViewModels.Base;
using PropertyChanged;

namespace modern_tech_499m.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {

        #region Private members

        private readonly IUserRepository _userRepository;

        #endregion


        #region Publilc properties

        private User _newUser;
        [DoNotNotify]
        public User NewUser
        {
            get => _newUser;
            set
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A flag indicating if register command is running
        /// </summary>
        public bool RegisterIsRunning { get; set; }
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
            RegisterCommand = new RelayParameterizedCommand(async parameter => await Register(parameter),
                parameter => true);
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            _newUser = new User {BirthDate = DateTime.Today};
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
                await Task.Delay(TimeSpan.FromSeconds(1));

                var passwordHash = (parameter as IHavePassword).SecurePassword.Unsecure().GetStringHash();
                NewUser.PasswordHash = passwordHash;

                if (!RegisterCommandCanExecute(out string errorMessage))
                {
                    await IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Error",
                        Message = errorMessage,
                        OkText = "OK"
                    });
                    return;
                }

                var registerResult = await _userRepository.RegisterUser(NewUser);

                switch (registerResult.registerResult)
                {
                    case RegisterResult.UsernameAlreadyExists:
                        await IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Error",
                            Message = "Selected username already exists",
                            OkText = "OK"
                        });
                        break;
                    case RegisterResult.Success:
                        await IoC.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Info",
                            Message = "Registration successful, returning to the game page",
                            OkText = "OK"
                        });

                        (NavigationSourcePageViewModel as GamePageViewModel).CurrentPlayerLoggingAction(
                            new UserPlayer(registerResult.registeredUser.Username, registerResult.registeredUser, Guid.Empty));
                        ViewModelLocator.ApplicationViewModel.ReturnToNavigationPageSource(NavigationSourcePage,
                            NavigationSourcePageViewModel);
                        break;
                }
            });
        }

        private bool RegisterCommandCanExecute(out string errorMessage)
        {
            var errors = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetIndexParameters().Length == 0)
                .Select(property => NewUser[property.Name]).Where(item => !string.IsNullOrEmpty(item));
            StringBuilder sb = new StringBuilder();
            foreach (var error in errors)
                sb.AppendLine(error);
            errorMessage = sb.ToString();
            return errors.Count() == 0;
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
