using CommonFrontEnd.View.Trade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CommonFrontEnd.Common
{
    public static class MultiWindowCheckBoxCheckExtension
    {
        public static bool is4decimalCheckboxCheck { get; set; }
        public static bool GetIsChecked(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCheckedProperty);
        }

        public static void SetIsChecked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.RegisterAttached(
                "IsChecked", typeof(bool), typeof(MultiWindowCheckBoxCheckExtension),
                new UIPropertyMetadata(false, OnIsCheckedPropertyChanged));

        private static void OnIsCheckedPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if (e.NewValue != null)
            {
                // Keyboard.ClearFocus();
                //uie.Focus(); // Don't care about false values.
                //uie.LostFocus += UieOnLostFocus;
                //Keyboard.Focus(uie);
                is4decimalCheckboxCheck = (bool)e.NewValue;
                NetPositionClientWise oNetPositionClientWise = System.Windows.Application.Current.Windows.OfType<NetPositionClientWise>().SingleOrDefault();
                if (oNetPositionClientWise != null)
                {
                    oNetPositionClientWise.ratein4decimal.IsChecked = (bool)e.NewValue;
                }
                NetPositionClientWiseDetails oNetPositionClientWiseDetails = System.Windows.Application.Current.Windows.OfType<NetPositionClientWiseDetails>().SingleOrDefault();
                if (oNetPositionClientWiseDetails != null)
                {
                    oNetPositionClientWiseDetails.ratein4decimal.IsChecked = (bool)e.NewValue;
                }
                NetPositionScripWiseDetails oNetPositionScripWiseDetails = System.Windows.Application.Current.Windows.OfType<NetPositionScripWiseDetails>().SingleOrDefault();
                if (oNetPositionScripWiseDetails != null)
                {
                    oNetPositionScripWiseDetails.ratein4decimal.IsChecked = (bool)e.NewValue;
                }
                NetPositionScripWise oNetPositionScripWise = System.Windows.Application.Current.Windows.OfType<NetPositionScripWise>().SingleOrDefault();
                if (oNetPositionScripWise != null)
                {
                    oNetPositionScripWise.ratein4decimal.IsChecked = (bool)e.NewValue;
                }
                //NetPositionScripWise
                //NetPositionClientWiseDetails
                //NetPositionScripWiseDetails
            }
        }
        //private static void UieOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        //{
        //    var uie = sender as UIElement;
        //    if (uie == null)
        //        return;
        //    uie.LostFocus -= UieOnLostFocus;
        //    uie.SetValue(IsFocusedProperty, false);
        //}
    }

}
