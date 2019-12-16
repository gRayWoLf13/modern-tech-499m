using System.Windows;
using modern_tech_499m.Animation;

namespace modern_tech_499m
{
    /// <summary>
    /// A base class to run any animation method when a boolean is set to true
    /// and a reverse animation when set to false
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public abstract class AnimateBaseProperty<TParent> : BaseAttachedProperty<TParent, bool>
    where TParent : BaseAttachedProperty<TParent, bool>, new()
    {
        #region Public properties

        /// <summary>
        /// A flag indicating the first load of the property
        /// </summary>
        public bool FirstLoad { get; set; } = true;

        #endregion
        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            //Get the framework element
            if (!(sender is FrameworkElement element))
                return;

            //Don't fire if the value doesn't change
            if (sender.GetValue(ValueProperty) == value && !FirstLoad)
                return;

            //On first load...
            if (FirstLoad)
            {
                //Create a single self-unhookable event for the element loaded event
                RoutedEventHandler onLoaded = null;
                onLoaded = (ss, ee) =>
                {
                    //Unload self
                    element.Loaded -= onLoaded;

                    //Do desired animation
                    DoAnimation(element, (bool)value);

                    //No longer a first load
                    FirstLoad = false;
                };

                //Hook into the Loaded event of the element
                element.Loaded += onLoaded;
            }
            else
                //Do desired animation
                DoAnimation(element, (bool)value);
        }

        /// <summary>
        /// The animation method that is fired when the value changes
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="value">The new value</param>
        protected virtual void DoAnimation(FrameworkElement element, bool value) { }
    }

    /// <summary>
    /// Animates a framework element to slide in from the left on show
    /// and slide out to the left on hide
    /// </summary>
    public class AnimateSlideInFromLeftProperty : AnimateBaseProperty<AnimateSlideInFromLeftProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            if (value)
                //Animate in
                await element.SlideAndFadeInFromLeft(FirstLoad ? 0 : 0.3, keepMargin: false);
            else
                //Animate out
                await element.SlideAndFadeOutToLeft(FirstLoad ? 0 : 0.3, keepMargin: false);
        }
    }
}
