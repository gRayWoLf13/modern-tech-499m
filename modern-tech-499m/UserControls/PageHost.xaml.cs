using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using modern_tech_499m.Pages;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.UserControls
{
    /// <summary>
    /// Interaction logic for PageHost.xaml
    /// </summary>
    public partial class PageHost : UserControl
    {
        #region Dependenty properties

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            nameof(CurrentPage), typeof(ApplicationPage), typeof(PageHost),
            new UIPropertyMetadata(default(ApplicationPage), null, CurrentPagePropertyChanged));
        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get => (ApplicationPage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPageViewModel"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageViewModelProperty = DependencyProperty.Register(
            nameof(CurrentPageViewModel), typeof(BaseViewModel), typeof(PageHost), new UIPropertyMetadata());

        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public BaseViewModel CurrentPageViewModel
        {
            get => (BaseViewModel)GetValue(CurrentPageViewModelProperty);
            set => SetValue(CurrentPageViewModelProperty, value);
        }

        #endregion

        public PageHost()
        {
            InitializeComponent();
        }

        #region Property changed events

        /// <summary>
        /// Called when <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        private static object CurrentPagePropertyChanged(DependencyObject d, object value)
        {
            //Get current values
            var currentPage = (ApplicationPage)d.GetValue(CurrentPageProperty);
            var currentPageViewModel = d.GetValue(CurrentPageViewModelProperty);

            //Get the frames
            var newPageFrame = (d as PageHost).NewPage;
            var oldPageFrame = (d as PageHost).OldPage;

            //Store the old page content as the old page
            var oldPageContent = newPageFrame.Content;

            //Remove current page from new page frame
            newPageFrame.Content = null;
            
            //Move the previous page into the old page frame
            oldPageFrame.Content = oldPageContent;

            //Animate out previous page(not awaiting for it) when the loaded event fires right after this call due to moving frames
            if (oldPageContent is BasePage oldPage)
            {
                //Tell the page to animate out
                oldPage.ShouldAnimateOut = true;

                //Once t's done, remove it
                Task.Delay(TimeSpan.FromSeconds(oldPage.SlideSeconds)).ContinueWith(t =>
                {
                    //Remove old page
                    Application.Current.Dispatcher.Invoke(() => oldPageFrame.Content = null);
                });
            }

            //Set the new page content
            newPageFrame.Content = currentPage.ToBasePage(currentPageViewModel);

            return value;
        }

        #endregion
    }
}
