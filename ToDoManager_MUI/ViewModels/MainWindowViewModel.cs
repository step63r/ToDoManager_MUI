using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using ToDoManager_MUI.Common;
using ToDoManager_MUI.Models;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows.Threading;

namespace ToDoManager_MUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region コマンド・プロパティ
        /// <summary>
        /// 「新規作成」コマンド
        /// </summary>
        private RelayCommand createNewCommand;
        public RelayCommand CreateNewCommand
        {
            get
            {
                if (createNewCommand == null)
                {
                    createNewCommand = new RelayCommand(Create, CanCreate);
                }
                return createNewCommand;
            }
        }
        /// <summary>
        /// DataGridダブルクリックコマンド
        /// </summary>
        private RelayCommand dataGridDoubleClick;
        public RelayCommand DataGridDoubleClick
        {
            get
            {
                if (dataGridDoubleClick == null)
                {
                    dataGridDoubleClick = new RelayCommand(Edit, CanEdit);
                }
                return dataGridDoubleClick;
            }
        }
        /// <summary>
        /// DataGrid Deleteキー押下コマンド
        /// </summary>
        private RelayCommand dataGridDelete;
        public RelayCommand DataGridDelete
        {
            get
            {
                if (dataGridDelete == null)
                {
                    dataGridDelete = new RelayCommand(Delete, CanDelete);
                }
                return dataGridDelete;
            }
        }
        /// <summary>
        /// ViewのInteractionRequestTriggerからバインドするプロパティ
        /// </summary>
        public InteractionRequest<Notification> ShowCreateDialogRequest { get; } = new InteractionRequest<Notification>();
        /// <summary>
        /// ToDoコレクション
        /// </summary>
        private ObservableCollection<ToDo> colToDo;
        public ObservableCollection<ToDo> ColToDo
        {
            get
            {
                return colToDo;
            }
            set
            {
                colToDo = value;
                RaisePropertyChanged("ColToDo");
            }
        }
        /// <summary>
        /// 選択されたToDo
        /// </summary>
        private ToDo selectedToDo;
        public ToDo SelectedToDo
        {
            get
            {
                return selectedToDo;
            }
            set
            {
                selectedToDo = value;
                RaisePropertyChanged("SelectedToDo");
            }
        }
        /// <summary>
        /// 通知領域テキスト
        /// </summary>
        private string taskBarMessage;
        public string TaskBarMessage
        {
            get
            {
                return taskBarMessage;
            }
            set
            {
                taskBarMessage = value;
                RaisePropertyChanged("TaskBarMessage");
            }
        }
        // ファイルパス
        private static string filePathToDo = string.Format("{0}/{1}", FilePath.BaseDir, FilePath.XmlPathToDo);
        // ディスパッチャタイマ
        private DispatcherTimer dispatcherTimer;
        // 本日日付
        private DateTime dateToday;
        public DateTime DateToday
        {
            get
            {
                return dateToday;
            }
            set
            {
                dateToday = value;
                RaisePropertyChanged("DateToday");
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            // ディスパッチャタイマの作成
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += new EventHandler(dispathcerTimer_Tick);
            dispatcherTimer.Start();

            ColToDo = Load();
        }

        /// <summary>
        /// 指定時間おきに現在日付を取得する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispathcerTimer_Tick(object sender, EventArgs e)
        {
            foreach (var item in ColToDo)
            {
                item.Today = DateTime.Today;
            }

            // 通知領域テキスト作成
            TaskBarMessage = CreateTaskBarMessage(ColToDo);
        }

        /// <summary>
        /// 新規作成のコマンド実行
        /// </summary>
        public void Create()
        {
            // 別ウィンドウのDataContextに使うオブジェクト
            var vm = new CreateViewModel(null);

            // requestのRaiseを呼び出す
            ShowCreateDialogRequest.Raise(new Notification { Content = vm });

            // リロード
            ColToDo = Load();
        }

        /// <summary>
        /// 新規作成が実行可能かどうかを判定
        /// </summary>
        /// <returns></returns>
        public bool CanCreate()
        {
            return true;
        }

        /// <summary>
        /// DataGridダブルクリックのコマンド実行
        /// </summary>
        public void Edit()
        {
            // 別ウィンドウのDataContextに使うオブジェクト
            var vm = new CreateViewModel(SelectedToDo);

            // requestのRaiseを呼び出す
            ShowCreateDialogRequest.Raise(new Notification { Content = vm });

            // リロード
            ColToDo = Load();
        }

        /// <summary>
        /// DataGridダブルクリックが実行可能かどうかを判定
        /// </summary>
        /// <returns></returns>
        public bool CanEdit()
        {
            return !(SelectedToDo is null);
        }

        /// <summary>
        /// DataGrid Deleteキー押下のコマンド実行
        /// </summary>
        public void Delete()
        {
            // クラスを作って
            var mainWindowModel = new MainWindowModel()
            {
                // オブジェクトを渡して
                ColToDo = ColToDo
            };
            // 動かして
            mainWindowModel.Execute(SelectedToDo);
            // 戻す
            ColToDo = mainWindowModel.ColToDo;

            // XMLファイルに保存
            // ViewModelで読み込むので保存も収まりよくこちらに
            if (XmlConverter.SerializeFromCol(ColToDo, filePathToDo))
            {
                // 成功
            }
            else
            {
                // 失敗
            }

            // リロード
            ColToDo = Load();
        }
        
        /// <summary>
        /// DataGrid Deleteキー押下が実行可能かどうかを判定
        /// </summary>
        /// <returns></returns>
        public bool CanDelete()
        {
            return !(SelectedToDo is null);
        }
        /// <summary>
        /// データをXMLから読み込む
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<ToDo> Load()
        {
            var ret = XmlConverter.DeSerializeToCol<ToDo>(filePathToDo);

            if (ret is null)
            {
                ret = new ObservableCollection<ToDo>();
                // XMLファイルに保存
                // ViewModelで読み込むので保存も収まりよくこちらに
                if (XmlConverter.SerializeFromCol(ColToDo, filePathToDo))
                {
                    // 成功
                }
                else
                {
                    MessageBox.Show("XMLファイルの保存に失敗しました");
                }
            }
            // ソート
            var list = ret.ToList();
            var comp = new ToDoComparer();
            list.Sort(comp);
            ret = new ObservableCollection<ToDo>(list);

            // 日付の設定
            // あまり良い実装ではないが……
            foreach (var item in ret)
            {
                item.Today = DateTime.Today;
            }

            // 通知領域テキスト作成
            TaskBarMessage = CreateTaskBarMessage(ret);

            return ret;
        }

        /// <summary>
        /// 通知領域のテキストを作成する
        /// </summary>
        /// <returns>「ToDoManager\n期限超過：○件\n本日期限：○件\n明日期限：○件</returns>
        private string CreateTaskBarMessage(ObservableCollection<ToDo> colToDo)
        {
            string ret = "ToDoManager";

            // 期限超過のタスクがあれば作成
            if (colToDo.Where(item => item.Date < DateTime.Today).Count() > 0)
            {
                ret += string.Format("\n期限超過: {0}件", colToDo.Where(item => item.Date < DateTime.Today).Count());
            }

            // 今日期限のタスクがあれば作成
            if (colToDo.Where(item => item.Date == DateTime.Today).Count() > 0)
            {
                ret += string.Format("\n今日期限: {0}件", colToDo.Where(item => item.Date == DateTime.Today).Count());
            }

            // 明日期限のタスクがあれば作成
            if (colToDo.Where(item => item.Date == DateTime.Today.AddDays(1)).Count() > 0)
            {
                ret += string.Format("\n明日期限: {0}件", colToDo.Where(item => item.Date == DateTime.Today.AddDays(1)).Count());
            }
            return ret;
        }
    }
}
