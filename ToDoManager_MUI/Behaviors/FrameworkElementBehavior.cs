using System.Windows;
using System.Windows.Input;

namespace ToDoManager_MUI.Behaviors
{
    /// <summary>
    /// FrameworkElement用ビヘイビアクラス
    /// </summary>
    public class FrameworkElementBehavior
    {
        /// <summary>
        /// Loadedイベント時に実行するコマンドオブジェクトを取得します。
        /// </summary>
        /// <param name="obj">対象オブジェクト(FrameworkElement)</param>
        /// <returns>コマンドオブジェクト</returns>
        public static ICommand GetOnLoadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(OnLoadedCommandProperty);
        }

        /// <summary>
        /// Loadedイベント時に実行するコマンドオブジェクトを設定します。
        /// </summary>
        /// <param name="obj">対象オブジェクト(FrameworkElement)</param>
        /// <param name="value">コマンドオブジェクト</param>
        public static void SetOnLoadedCommand(
                                DependencyObject obj, ICommand value)
        {
            obj.SetValue(OnLoadedCommandProperty, value);
        }

        /// <summary>Loadedイベント時に実行するコマンドオブジェクト</summary>
        public static readonly DependencyProperty OnLoadedCommandProperty =
            DependencyProperty.RegisterAttached("OnLoadedCommand",
                                typeof(ICommand),
                                typeof(FrameworkElementBehavior),
                                new UIPropertyMetadata(null,
                                            OnLoadedCommandPropertyChanged));

        /// <summary>
        /// OnLoadedCommandプロパティ値変更イベントハンドラ
        /// </summary>
        /// <param name="dpObj">対象オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private static void OnLoadedCommandPropertyChanged(
                                    DependencyObject dpObj,
                                    DependencyPropertyChangedEventArgs e)
        {
            var frwElement = dpObj as FrameworkElement;

            if (frwElement == null)
            {
                //この添付プロパティは、FrameworkElementのサブクラスで
                //使われる事を前提としています。
                //そのため、FrameworkElementを継承していない
                //クラスインスタンスは無視されます。
                return;
            }

            if (e.NewValue != null)
            {
                //e.NewValue→新しいICommandオブジェクトが設定された。
                if (!frwElement.IsLoaded)
                {
                    //IsLoaded = false = まだLoadされていない。
                    //→Loadedイベントにハンドラメソッドを登録します。
                    frwElement.Loaded += new RoutedEventHandler(FrwElement_Loaded);
                }
                else
                {
                    //IsLoaded = true = 既にLoadされている。
                    //→Loadedイベント用ハンドラメソッドを実行します。
                    FrameworkElementBehavior.FrwElement_Loaded(
                            frwElement,
                            new RoutedEventArgs
                            {
                                Source = frwElement,
                                Handled = false
                            });
                }
            }

            return;
        }

        /// <summary>
        /// Loadedイベントハンドラ用メソッド
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="e">イベント引数</param>
        private static void FrwElement_Loaded(object sender, RoutedEventArgs e)
        {
            var frwElement = sender as FrameworkElement;
            //登録したメソッドは削除しないと、メモリリークの元になる。
            frwElement.Loaded -= new RoutedEventHandler(FrwElement_Loaded);

            var command = FrameworkElementBehavior.GetOnLoadedCommand(frwElement);

            //実行可能であれば、コマンドを実行する。
            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }

            return;
        }
    }
}
