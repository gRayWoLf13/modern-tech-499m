using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace modern_tech_499m
{
    /// <summary>
    /// The NoFrameHistoryProperty attached property for creating a <see cref="Frame"/> that never shows navigation and keeps navigation history empty
    /// </summary>
    public class NoFrameHistoryAttachedProperty : BaseAttachedProperty<NoFrameHistoryAttachedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Get the frame
            if (!(sender is Frame frame))
                return;

            //Hide navigation bad
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            //Clear history on navigate
            frame.Navigated += (ss, ee) => ((Frame) ss).NavigationService.RemoveBackEntry();
        }
    }
}
