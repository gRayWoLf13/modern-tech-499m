using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// The application state as a viewmodel
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Public properties

        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Welcome;

        /// <summary>
        /// The viewmodel to use for the current page when the CurrentPage changes
        /// NOTE: This is no a live up-to-date model of the current page
        ///       It is simply used to set the viewmodel of the current page
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible { get; set; }

        /// <summary>
        /// Command to open/close side menu
        /// </summary>
        public ICommand OpenCloseSettingsCommand { get; set; }

        /// <summary>
        /// Command to go to welcome page
        /// </summary>
        public ICommand NavigateToWelcomePageCommand { get; set; }

        /// <summary>
        /// Command to go to all games list page
        /// </summary>
        public ICommand NavigateToGamesPageCommand { get; set; }

        /// <summary>
        /// Command to go to all users list command
        /// </summary>
        public ICommand NavigateToUsersListPageCommand { get; set; }

        #endregion

        #region Helper methods

        /// <summary>
        /// Navigates to specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="newCurrentPageViewModel">Viewmodel to set explicitly, if any</param>
        public void GoToPage(ApplicationPage page, BaseViewModel newCurrentPageViewModel = null)
        {
            //Set the viewmodel
            CurrentPageViewModel = newCurrentPageViewModel;

            //Set the current page
            CurrentPage = page;

            //Fire off a CurrentPage changed event
            OnPropertyChanged(nameof(CurrentPage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sourcePageType"></param>
        /// <param name="sourcePageViewModel"></param>
        /// <param name="newCurrentPageViewModel"></param>
        public void GoToPageWithNavigationSource(ApplicationPage page, ApplicationPage sourcePageType, BaseViewModel sourcePageViewModel, BaseViewModel newCurrentPageViewModel)
        {
            //Set the viewmodel
            CurrentPageViewModel = newCurrentPageViewModel;

            //Set the current page
            CurrentPage = page;

            //Fire off a CurrentPage changed event
            OnPropertyChanged(nameof(CurrentPage));

            //Set the navigation source page type
            CurrentPageViewModel.NavigationSourcePage = sourcePageType;

            //Set the navigation source page viewmodel
            CurrentPageViewModel.NavigationSourcePageViewModel = sourcePageViewModel;
        }

        public void ReturnToNavigationPageSource(ApplicationPage sourcePageType, BaseViewModel sourcePageViewModel)
        {
            CurrentPageViewModel = sourcePageViewModel;

            CurrentPage = sourcePageType;

            OnPropertyChanged(nameof(CurrentPage));
        }

        #endregion

        #region Constructor

        public ApplicationViewModel()
        {
            OpenCloseSettingsCommand = new RelayCommand(() => SideMenuVisible ^= true);
            NavigateToWelcomePageCommand = new RelayCommand(() =>
                ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.Welcome));
            NavigateToGamesPageCommand = new RelayCommand(() =>
                ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.GameInfoSelection));
            NavigateToUsersListPageCommand = new RelayCommand(() =>
                ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.UsersDatabase));
        }

        #endregion
    }
}
