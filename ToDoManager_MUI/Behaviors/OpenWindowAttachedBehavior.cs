namespace ToDoManager_MUI.Behaviors
{
    using System.Windows;
    using System.Windows.Input;

    public class OpenWindowAttachedBehavior
    {
        public static bool GetIsModal(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsModalProperty);
        }
        public static void SetIsModal(DependencyObject obj, bool value)
        {
            obj.SetValue(IsModalProperty, value);
        }
        // Using a DependencyProperty as the backing store for IsModal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsModalProperty =
            DependencyProperty.RegisterAttached("IsModal", typeof(bool), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(true));


        public static bool GetHasOwner(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasOwnerProperty);
        }
        public static void SetHasOwner(DependencyObject obj, bool value)
        {
            obj.SetValue(HasOwnerProperty, value);
        }
        // Using a DependencyProperty as the backing store for HasOwner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasOwnerProperty =
            DependencyProperty.RegisterAttached("HasOwner", typeof(bool), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(false));


        public static ICommand GetCloseCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CloseCommandProperty);
        }
        public static void SetCloseCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CloseCommandProperty, value);
        }
        // Using a DependencyProperty as the backing store for CloseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached("CloseCommand", typeof(ICommand), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null));


        public static DataTemplate GetWindowTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(WindowTemplateProperty);
        }
        public static void SetWindowTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(WindowTemplateProperty, value);
        }
        // Using a DependencyProperty as the backing store for WindowTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowTemplateProperty =
            DependencyProperty.RegisterAttached("WindowTemplate", typeof(DataTemplate), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null));


        public static object GetWindowViewModel(DependencyObject obj)
        {
            return (object)obj.GetValue(WindowViewModelProperty);
        }
        public static void SetWindowViewModel(DependencyObject obj, object value)
        {
            obj.SetValue(WindowViewModelProperty, value);
        }
        // Using a DependencyProperty as the backing store for WindowViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowViewModelProperty =
            DependencyProperty.RegisterAttached("WindowViewModel", typeof(object), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null, OnWindowViewModelChanged));

        private static void OnWindowViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null)
                return;

            var template = GetWindowTemplate(d);
            var viewmodel = GetWindowViewModel(d);

            // テンプレートが指定されてないと、ウィンドウは表示できない。
            if (template != null)
            {
                if (viewmodel != null)
                {
                    // VMがセットされたらウィンドウ表示
                    OpenWindow(element);
                }
                else
                {
                    // VMがnullになったら、ウィンドウを閉じる
                    CloseWindow(element);
                }
            }
        }

        private static void OpenWindow(FrameworkElement element)
        {
            var isModal = GetIsModal(element);
            var win = GetWindow(element);
            var cmd = GetCloseCommand(element);
            var template = GetWindowTemplate(element);
            var vm = GetWindowViewModel(element);
            var owner = Window.GetWindow(element);
            var hasOwner = GetHasOwner(element);

            if (win == null)
            {
                win = new Window()
                {
                    ContentTemplate = template,
                    Content = vm,
                    SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
                    Owner = hasOwner ? owner : null,
                };

                // ウィンドウの終了処理追加
                // イベントハンドラの引数からは添付ビヘイビアのプロパティにアクセスできないので、
                // ラムダ式でキャプチャする
                win.Closed += (s, e) =>
                {
                    if (cmd != null)
                    {
                        // ダイアログのVMを引数にCloseCommand実行
                        if (cmd.CanExecute(vm))
                            cmd.Execute(vm);
                    }
                    SetWindow(element, null);
                };

                // ウィンドウの表示処理
                SetWindow(element, win);
                if (isModal)
                    win.ShowDialog();
                else
                    win.Show();
            }
            else
            {
                // すでにウィンドウが表示されているので、アクティブ化で前面に出す
                win.Activate();
            }
        }

        private static void CloseWindow(FrameworkElement element)
        {
            var win = GetWindow(element);

            if (win != null)
            {
                win.Close();
                SetWindow(element, null);
            }
        }


        #region 添付ビヘイビアで使用する内部プロパティ
        public static Window GetWindow(DependencyObject obj)
        {
            return (Window)obj.GetValue(WindowProperty);
        }
        public static void SetWindow(DependencyObject obj, Window value)
        {
            obj.SetValue(WindowProperty, value);
        }
        // Using a DependencyProperty as the backing store for Window.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowProperty =
            DependencyProperty.RegisterAttached("Window", typeof(Window), typeof(OpenWindowAttachedBehavior), new PropertyMetadata(null));
        #endregion
    }
}
