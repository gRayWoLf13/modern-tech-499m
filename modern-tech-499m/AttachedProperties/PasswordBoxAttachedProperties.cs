﻿using System.Windows;
using System.Windows.Controls;

namespace modern_tech_499m
{

    /// <summary>
    /// The MonitorPassword attached property for the <see cref="PasswordBox"/>
    /// </summary>
    public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        { 
            //Get the caller and make sure it is a PasswordBox
            if (!(sender is PasswordBox passwordBox))
                return;

            //Remove any previous events
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            //If the caller set MonitorPassword to true, start listening
            if (!(bool) e.NewValue)
                return;
            HasTextProperty.SetValue(passwordBox);
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        /// <summary>
        /// Fired when the password box password value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Set the attached HasText value
            HasTextProperty.SetValue((PasswordBox) sender);
        }
    }

    /// <summary>
    /// The HasText attached property for the <see cref="PasswordBox"/>
    /// </summary>
    public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
    {
        /// <summary>
        /// Sets the HasText property based on the if the caller is <see cref="PasswordBox"/> has any text
        /// </summary>
        /// <param name="sender"></param>
        public static void SetValue(DependencyObject sender)
        {
            SetValue(sender, ((PasswordBox) sender).SecurePassword.Length > 0);
        }
    }
}
