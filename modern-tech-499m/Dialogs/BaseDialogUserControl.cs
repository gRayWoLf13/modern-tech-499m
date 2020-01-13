using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.ViewModels;
using modern_tech_499m.ViewModels.Base;
using modern_tech_499m.Views;

namespace modern_tech_499m
{
    /// <summary>
    /// The base class for any content that is being used inside of a <see cref="DialogWindow"/>
    /// </summary>
    public class BaseDialogUserControl : UserControl
    {
        #region Private members

        /// <summary>
        /// The dialog window we will be contained within
        /// </summary>
        private DialogWindow _dialogWindow;

        #endregion

        #region Public properties

        /// <summary>
        /// The minimum width of the dialog
        /// </summary>
        public int WindowMinimumWidth { get; set; } = 250;

        /// <summary>
        /// The minimum height of the dialog
        /// </summary>
        public int WindowMinimumHeight { get; set; } = 100;

        /// <summary>
        /// The title of title bar
        /// </summary>
        public int TitleHeight { get; set; } = 30;

        /// <summary>
        /// The title of this dialog
        /// </summary>
        public string Title { get; set; }

        #endregion


        #region Public commands

        /// <summary>
        /// Closes this dialog
        /// </summary>
        public ICommand CloseCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseDialogUserControl()
        {
            //Create a new dialog window
            _dialogWindow = new DialogWindow();
            _dialogWindow.ViewModel = new DialogWindowViewModel(_dialogWindow);

            //Create close command
            CloseCommand = new RelayCommand(_dialogWindow.Close);
        }

        #endregion

        #region Public Dialog Show Methods

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <typeparam name="T">The viewmodel type for this control</typeparam>
        /// <returns></returns>
        public Task ShowDialog<T>(T viewModel)
        where  T : BaseDialogViewModel
        {
            //Create a task to await a dialog closing
            var tcs = new TaskCompletionSource<bool>();
            //Run on the UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    //Match control's expected size to the dialog window's viewmodel
                    _dialogWindow.ViewModel.WindowMinimumWidth = WindowMinimumWidth;
                    _dialogWindow.ViewModel.WindowMinimumHeight = WindowMinimumHeight;
                    _dialogWindow.ViewModel.TitleHeight = TitleHeight;
                    _dialogWindow.ViewModel.Title = string.IsNullOrEmpty(viewModel.Title) ? Title : viewModel.Title;

                    //Set this control to the dialog window content
                    _dialogWindow.ViewModel.Content = this;

                    //Setup this control's data context binding to the viewmodel
                    DataContext = viewModel;

                    //Show the dialog
                    _dialogWindow.ShowDialog();
                }
                finally
                {
                    //Let caller know we finished
                    tcs.SetResult(true);
                }
            });
            return tcs.Task;
        }

        #endregion

    }
}
