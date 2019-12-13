using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private members

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window _window;

        /// <summary>
        /// The margin around the window to allow a drop shadow
        /// </summary>
        private int _outerMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int _windowRadius = 10;
        #endregion

        #region Public properties

        /// <summary>
        /// Minimum width of the window
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;
        /// <summary>
        /// Minimum height of the window
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        /// The size of the resize border around the window taking into account outer margin
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OuterMarginSize);

        /// <summary>
        /// The padding of the content of the main window
        /// </summary>
        public Thickness InnerContentPadding => new Thickness(ResizeBorder);

        /// <summary>
        /// The margin around the window to allow a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get => _window.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            set => _outerMarginSize = value;
        }

        /// <summary>
        /// The margin around the window to allow a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            get => _window.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            set => _windowRadius = value;
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// <summary>
        /// The height of the window caption
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);
        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to open window menu
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        public WindowViewModel(Window window)
        {
            _window = window;

            //Lister out for the window resizing
            _window.StateChanged += (sender, e) =>
            {
                //Fire off events for all properties affected by resize
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            //Create commands
            MinimizeCommand = new RelayCommand(() => _window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => _window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => _window.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(_window, GetMousePosition()));

            //Fix window resize issue
            var resizer = new WindowResizer(_window);
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            var position = Mouse.GetPosition(_window);
            return new Point(position.X + _window.Left, position.Y + _window.Top);
        }

        #endregion

    }
}
