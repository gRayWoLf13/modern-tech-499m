using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using modern_tech_499m.Animation;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.Pages
{
    /// <summary>
    /// Base page for all pages gain base functionality
    /// </summary>
    public class BasePage : Page
    {

        #region Public properties
        /// <summary>
        /// The animation to play when the page is first loaded
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// The animation to play when the page is unloaded
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToLeft;

        /// <summary>
        /// The time any slide animation takes
        /// </summary>
        public double SlideSeconds { get; set; } = 0.8;

        /// <summary>
        /// A flag to indicate if this page should animate out on load
        /// useful for when we are moving the page to another frame
        /// </summary>
        public bool ShouldAnimateOut { get; set; }
        #endregion

        #region Constructor

        public BasePage()
        {
            //If we are animation in, hide to begin with
            if (PageLoadAnimation != PageAnimation.None)
                Visibility = Visibility.Collapsed;

            //Listen out for the page loading
            Loaded += BasePage_Loaded;
        }

        #endregion

        #region Animation load/unload

        /// <summary>
        /// Once the page is loaded, perform any required animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            //If we are setup to animate out on load
            if (ShouldAnimateOut)
                //Animate out
                await AnimateOut();
            //Otherwise...
            else
                //Animate the page in
                await AnimateIn();

        }

        /// <summary>
        /// Animations in this page
        /// </summary>
        public async Task AnimateIn()
        {
            if (PageLoadAnimation == PageAnimation.None)
                return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:

                    //Start the animation
                    await this.SlideAndFadeInFromRight(SlideSeconds);

                    break;
            }
        }

        /// <summary>
        /// Animates the page out
        /// </summary>
        public async Task AnimateOut()
        {
            if (PageUnloadAnimation == PageAnimation.None)
                return;

            switch (PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToLeft:

                    //Start the animation
                    await this.SlideAndFadeOutToLeft(SlideSeconds);

                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// A base page with added viewmodel support
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class BasePage<TViewModel> : BasePage
    where TViewModel : BaseViewModel
    {
        #region Private member

        /// <summary>
        /// The viewmodel associated with the page
        /// </summary>
        private TViewModel _viewModel;

        #endregion

        #region Public properties

        /// <summary>
        /// ViewModel associated with the page
        /// </summary>
        public TViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                //If nothing was changed, return
                if (_viewModel == value)
                    return;

                //Update the value
                _viewModel = value;

                //Set the data context
                DataContext = _viewModel;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
            //Resolve preregistered generic viewmodel from bootstrapper
            ViewModel = BootStrapper.Resolve<TViewModel>();
        }

        /// <summary>
        /// Specific constructor
        /// </summary>
        /// <param name="specificViewModel">The viewmodel type</param>
        public BasePage(TViewModel specificViewModel = null)
        : base()
        {
            //Set specific model, if any
            if (specificViewModel != null)
                ViewModel = specificViewModel;
            else
            //Otherwise - load default viewmodel
                ViewModel = BootStrapper.Resolve<TViewModel>();
        }


        #endregion
    }
}
