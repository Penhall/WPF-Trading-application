using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CommonFrontEnd.Common
{
    class CustomDataGrid : DataGrid
    {
        public CustomDataGrid()
        {
            this.SelectionChanged += CustomDataGrid_SelectionChanged;
            PreviewKeyDown += CustomDataGrid_PreviewKeyDown;
        }

        private void CustomDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                var ParentofFocus = UIHelper.FindParent<ContentPresenter>(elementWithFocus);
                ParentofFocus.MoveFocus(
         new TraversalRequest(
         FocusNavigationDirection.Next));

                e.Handled = true;
            }
        }

        void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList?.Clear();
            foreach (var item in this.SelectedItems) { this.SelectedItemsList?.Add(item); }
            //this.SelectedItemsList = this.SelectedItems;
        }
        #region SelectedItemsList

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
                DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomDataGrid), new PropertyMetadata(null));

        #endregion

    }


    public class ClickSelectTextBox : TextBox
    {
        public ClickSelectTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent,
              new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent,
              new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent,
              new RoutedEventHandler(SelectAllText), true);
        }

        private static void SelectivelyIgnoreMouseButton(object sender,
                                                         MouseButtonEventArgs e)
        {
            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    // If the text box is not yet focussed, give it the focus and
                    // stop further processing of this click event.
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }
    }
}
