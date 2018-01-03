/******************************************************************
 *                                                                *
 * ソース                                                         *
 * http://pieceofnostalgy.blogspot.jp/2012/04/wpf-textbox_28.html *
 *                                                                *
 ******************************************************************/
namespace ToDoManager_MUI.Behaviors
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// TextBoxにフォーカスが移った時に全選択する添付ビヘイビア
    /// </summary>
    public class TextBoxGotFocusBehavior
    {
        public static readonly DependencyProperty SelectAllOnGotFocusProperty =
                DependencyProperty.RegisterAttached(
                        "SelectAllOnGotFocus",
                        typeof(bool),
                        typeof(TextBoxGotFocusBehavior),
                        new UIPropertyMetadata(false, SelectAllOnGotFocusChanged)
                );

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetSelectAllOnGotFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllOnGotFocusProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetSelectAllOnGotFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllOnGotFocusProperty, value);
        }

        private static void SelectAllOnGotFocusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs evt)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
                return;

            textBox.GotFocus -= OnTextBoxGotFocus;
            if ((bool)evt.NewValue)
                textBox.GotFocus += OnTextBoxGotFocus;
        }

        private static void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Debug.Assert(textBox != null);

            textBox.Dispatcher.BeginInvoke((Action)(() => textBox.SelectAll()));
        }
    }
}
