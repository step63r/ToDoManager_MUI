/********************************************************
 *                                                      *
 * ソース                                               *
 * http://takamints.hatenablog.jp/entry/mvvm-messagebox *
 *                                                      *
 ********************************************************/
namespace ToDoManager_MUI.Common
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    /// MVVM的メッセージボックスを表示するためのメッセンジャー。
    ///
    /// MessageBox.Showメソッドで、イベントトリガーを起動する。
    /// 
    /// 実際の表示は、イベントトリガーから実行される
    /// トリガーアクションに実装される。
    /// </summary>
    public class MessageBox
    {
        /// <summary>
        /// MessageBoxTriggerを起動するイベント。
        /// </summary>
        public event EventHandler<EventArgs> ShowMessageBox;

        /// <summary>
        /// MessageBoxTriggerを起動するイベントの名前。
        /// </summary>
        public static string EventName
        { get { return "ShowMessageBox"; } }

        /// <summary>
        /// このクラスはシングルトン。
        /// </summary>
        public static MessageBox Instance
        { get; private set; } = new MessageBox();
        private MessageBox() { }

        /// <summary>
        /// MessageBoxTriggerを起動するイベントの引数。
        /// トリガーアクションへ渡されて処理される。
        /// </summary>
        public class EventArgs : System.EventArgs
        {
            public string Text { get; set; }
            public string Title { get; set; }
            public MessageBoxButton Button { get; set; }
            public MessageBoxImage Icon { get; set; }

            /// <summary>
            /// メッセージボックスの結果を受け取るコールバック
            /// </summary>
            public Action<MessageBoxResult> NotifyResult
            { get; set; }
        }

        /// <summary>
        /// MVVM的メッセージボックスを表示。
        /// 実際にはイベントを発行してイベントトリガーを起動する。
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="title"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static MessageBoxResult Show(
            string messageBoxText,
            string title = null,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.Information)
        {
            //メッセージボックスの結果
            MessageBoxResult messageBoxResult = MessageBoxResult.Cancel;

            //イベントを発行する
            Instance.ShowMessageBox?.Invoke(
                Instance,
                new MessageBox.EventArgs()
                {
                    Text = messageBoxText,
                    Title = title,
                    Button = button,
                    Icon = icon,

                    //コールバックで結果を受け取る
                    NotifyResult = result =>
                    {
                        messageBoxResult = result;
                    }
                });

            //メッセージボックスの結果を返す
            return messageBoxResult;
        }

        /// <summary>
        /// メッセージを表示するトリガーアクション実装用の抽象基底クラス。
        /// 
        /// 派生クラスでShowMessageを実装する。 
        /// </summary>
        public abstract class Action : TriggerAction<DependencyObject>
        {
            /// <summary>
            /// アクションの実態
            /// </summary>
            /// <param name="parameter"></param>
            protected override void Invoke(object parameter)
            {
                //イベント引数の種別を検査
                var messageBoxArgs = parameter as MessageBox.EventArgs;
                if (messageBoxArgs == null)
                {
                    return;
                }

                //メッセージボックスの表示結果を取得
                MessageBoxResult result = ShowMessage(
                        messageBoxArgs.Text,
                        messageBoxArgs.Title,
                        messageBoxArgs.Button,
                        messageBoxArgs.Icon);

                //コールバックで結果を通知
                messageBoxArgs.NotifyResult?.Invoke(result);
            }

            /// <summary>
            /// メッセージボックスを表示する抽象メソッド。
            /// </summary>
            /// <param name="text"></param>
            /// <param name="title"></param>
            /// <param name="button"></param>
            /// <param name="icon"></param>
            /// <returns></returns>
            abstract protected MessageBoxResult
            ShowMessage(
                string text, string title,
                MessageBoxButton button,
                MessageBoxImage icon);
        }
    }

    /// <summary>
    /// MVVM的メッセージボックスを表示するためのイベントトリガー。
    ///
    /// MessageBox メッセンジャーの発行するイベントで起動される。
    /// </summary>
    public class MessageBoxTrigger : System.Windows.Interactivity.EventTrigger
    {
        public MessageBoxTrigger()
            : base(MessageBox.EventName)
        {
            SourceObject = MessageBox.Instance;
        }
    }

    /// <summary>
    /// メッセージボックスを表示するトリガーアクション
    /// </summary>
    public class MessageBoxAction : MessageBox.Action
    {
        protected override MessageBoxResult ShowMessage(
            string text,
            string title,
            MessageBoxButton button,
            MessageBoxImage icon)
        {
            var owner = Application.Current.Windows
                .OfType<Window>().SingleOrDefault(
                    x => x.IsActive);
            if (owner == null)
            {
                owner = Window.GetWindow(AssociatedObject);
            }
            return System.Windows.MessageBox.Show(
                owner, text,
                (title != null ? title :
                    Application.Current.MainWindow.Title),
                button, icon);
        }
    }
}
