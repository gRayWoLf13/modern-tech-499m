using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace modern_tech_499m.Animation
{
    /// <summary>
    /// Helpers to animate pages
    /// </summary>
    public static class PageAnimations
    {
        /// <summary>
        /// Sleds the page in from the right
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this Page page, double seconds)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideFromRight(seconds, page.WindowWidth);

            //Add fade in animation
            sb.AddFadeIn(seconds);

            //Start animation
            sb.Begin(page);

            //Make page visible
            page.Visibility = Visibility.Visible;

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));

        }

        /// <summary>
        /// Sleds the page out to the left
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this Page page, double seconds)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideToLeft(seconds, page.WindowWidth);

            //Add fade in animation
            sb.AddFadeOut(seconds);

            //Start animation
            sb.Begin(page);

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));

            //Make page visible
            page.Visibility = Visibility.Collapsed;

        }
    }
}
