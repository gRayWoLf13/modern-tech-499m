using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace modern_tech_499m.Animation
{
    /// <summary>
    /// Helpers to animate framework elements
    /// </summary>
    public static class FrameworkElementAnimations
    {
        /// <summary>
        /// Slides an element in from the right
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideFromRight(seconds, element.ActualWidth);

            //Add fade in animation
            sb.AddFadeIn(seconds);

            //Start animation
            sb.Begin(element);

            //Make page visible
            element.Visibility = Visibility.Visible;

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));

        }

        /// <summary>
        /// Slides an element in from the right
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromLeft(this FrameworkElement element, double seconds, bool keepMargin = true, int size = 0)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideFromLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);

            //Add fade in animation
            sb.AddFadeIn(seconds);

            //Start animation
            sb.Begin(element);

            //Make page visible
            element.Visibility = Visibility.Visible;

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));

        }

        /// <summary>
        /// Slides an element out to the left
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this FrameworkElement element, double seconds, bool keepMargin = true)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideToLeft(seconds, element.ActualWidth, keepMargin: keepMargin);

            //Add fade in animation
            sb.AddFadeOut(seconds);

            //Start animation
            sb.Begin(element);

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));

            //Make page visible
            element.Visibility = Visibility.Hidden;

        }
        /// <summary>
        /// Slides an element out to the right
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        /// 
        public static async Task SlideAndFadeOutToRight(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideToRight(seconds, element.ActualWidth);

            //Add fade in animation
            sb.AddFadeOut(seconds); 

            //Start animation
            sb.Begin(element);

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));

            //Make page visible
            element.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// Fades an element in
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        /// 
        public static async Task FadeIn(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add fade in animation
            sb.AddFadeIn(seconds);

            //Start animation
            sb.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Fades an element out
        /// </summary>
        /// <param name="element">The element to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        /// 
        public static async Task FadeOut(this FrameworkElement element, double seconds)
        {
            //Create the storyboard
            var sb = new Storyboard();

            //Add fade in animation
            sb.AddFadeOut(seconds);

            //Start animation
            sb.Begin(element);

            //Make element visible
            element.Visibility = Visibility.Visible;

            //Wait for it to finish
            await Task.Delay(TimeSpan.FromSeconds(seconds));

            //Fully hide the element
            element.Visibility = Visibility.Collapsed;
        }


    }
}
