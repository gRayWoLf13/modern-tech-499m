using System.Windows;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Views
{
    /// <summary>
    /// Interaction logic for BaseWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        #region Private members

        /// <summary>
        /// The viewmodel for this window
        /// </summary>
        private DialogWindowViewModel _viewModel;

        #endregion

        #region Public properties

        /// <summary>
        /// The viewmodel for this dialog
        /// </summary>
        public DialogWindowViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                //Set a new value
                _viewModel = value;
                //Update data context
                DataContext = _viewModel;
            }
        }

        #endregion
        public DialogWindow()
        {
            InitializeComponent();
        }
    }
}
