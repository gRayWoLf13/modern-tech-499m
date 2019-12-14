﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using modern_tech_499m.Animation;

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
}
