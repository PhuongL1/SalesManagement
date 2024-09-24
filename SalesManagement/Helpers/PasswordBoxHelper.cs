using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SalesManagement.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPassword = DependencyProperty.RegisterAttached(
            "BoundPassword", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, OnBindPasswordChanged));

        private static bool _isUpdating = false;

        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }

        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }

        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
                if (!_isUpdating)
                {
                    passwordBox.Password = (string)e.NewValue;
                }
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = dp as PasswordBox;

            if (passwordBox != null)
            {
                bool wasBound = (bool)e.OldValue;
                bool needToBind = (bool)e.NewValue;

                if (wasBound)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }

                if (needToBind)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox != null)
            {
                _isUpdating = true;
                SetBoundPassword(passwordBox, passwordBox.Password);
                _isUpdating = false;
            }
        }
    }

}
