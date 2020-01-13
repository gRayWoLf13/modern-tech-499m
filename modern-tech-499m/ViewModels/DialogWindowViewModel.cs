using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// The viewmodel to the base dialog window
    /// </summary>
    public class DialogWindowViewModel : WindowViewModel
    {
        #region Constructor

        public DialogWindowViewModel(Window window) : base(window)
        {
            //Make minimum size smaller
            WindowMinimumWidth = 250;
            WindowMinimumHeight = 100;

            //Make the title bar slower
            TitleHeight = 30;
        }

        #endregion


        #region Public properties

        /// <summary>
        /// The title of this dialog window
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The content to host inside the dialog
        /// </summary>
        public ContentControl Content { get; set; }



        #endregion
    }
}
