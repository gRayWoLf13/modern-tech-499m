using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    public class WelcomePageViewModel : BaseViewModel
    {
        #region Public properties

        /// <summary>
        /// Command to start a new game
        /// </summary>
        public ICommand StartGameCommand { get; set; }

        /// <summary>
        /// Command to load game from database
        /// </summary>
        public ICommand LoadGameCommand { get; set; }

        /// <summary>
        /// Command to show all users info
        /// </summary>
        public ICommand ShowAllUsersCommand { get; set; }

        /// <summary>
        /// Command to quit game
        /// </summary>
        public ICommand QuitGameCommand { get; set; }

        #endregion

        #region Constructor

        public WelcomePageViewModel()
        {
            StartGameCommand = new RelayCommand(StartGame);
            LoadGameCommand = new RelayCommand(LoadGame);
            ShowAllUsersCommand = new RelayCommand(ShowAllUsers);
            QuitGameCommand = new RelayCommand(QuitGame);
        }

        #endregion

        #region Private methods

        private void StartGame()
        {
            ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.Game);
        }

        private void LoadGame()
        {
            ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.GameInfoSelection);
        }

        private void ShowAllUsers()
        {
            ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.UsersDatabase);
        }

        private void QuitGame()
        {
            System.Windows.Application.Current.Shutdown();
        }

        #endregion
    }
}
