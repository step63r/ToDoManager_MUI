// TODO
// 編集時はタイトルで検索させるため
// タイトル編集不可にする

using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using ToDoManager_MUI.Common;
using ToDoManager_MUI.Models;

namespace ToDoManager_MUI.ViewModels
{
    /// <summary>
    /// CreateDialogのViewModelクラス
    /// </summary>
    public class CreateDialogViewModel : BindableBase
    {
        #region コマンド・プロパティ
        /// <summary>
        /// 画面の表示状態を示すプロパティ
        /// trueにすると画面を閉じる
        /// </summary>
        private bool closeWindow;
        public bool CloseWindow
        {
            get { return closeWindow; }
            set { SetProperty(ref closeWindow, value); }
        }
        /// <summary>
        /// OKボタン
        /// </summary>
        private DelegateCommand createDialog_OK;
        public DelegateCommand CreateDialog_OK
        {
            get
            {
                if (createDialog_OK == null)
                {
                    createDialog_OK = new DelegateCommand(Execute, CanExecute);
                }
                return createDialog_OK;
            }
        }
        /// <summary>
        /// ToDoコレクション
        /// </summary>
        public ObservableCollection<ToDo> ColToDo { get; set; }
        /// <summary>
        /// 渡されたタスク（Nullの場合は新規作成）
        /// </summary>
        private ToDo oneToDo;
        public ToDo OneToDo
        {
            get { return oneToDo; }
            set
            {
                SetProperty(ref oneToDo, value);
                CreateDialog_OK.RaiseCanExecuteChanged();
            }
        }
        // ファイルパス
        private static string filePathToDo = string.Format("{0}/{1}", FilePath.BaseDir, FilePath.XmlPathToDo);
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreateDialogViewModel(ToDo onetodo)
        {
            ColToDo = Load();
            if (onetodo is null)
            {
                // 新規作成
                OneToDo = new ToDo()
                {
                    Title = "",
                    Detail = "",
                    IgnoreDate = 0,
                    Date = DateTime.Today
                };
            }
            else
            {
                // 編集
                OneToDo = onetodo;
            }
            //OKCommand = new DelegateCommand(() =>
            //{
            //    ((CreateDialogModel)this.Notification).OneToDo = this.OneToDo;
            //    this.FinishInteraction();
            //},
            //() => !string.IsNullOrEmpty(this.OneToDo.Title)).ObservesProperty(() => this.OneToDo.Title);
        }

        /// <summary>
        /// OKボタンのコマンド実行
        /// </summary>
        public void Execute()
        {
            // クラスを作って
            var createDialogModel = new CreateDialogModel()
            {
                // オブジェクトを渡して
                ColToDo = ColToDo
            };
            // 動かして
            createDialogModel.Execute(OneToDo);
            // 戻す
            ColToDo = createDialogModel.ColToDo;

            // XMLファイルに保存
            // ViewModelで読み込むので保存も収まりよくこちらに
            if (XmlConverter.SerializeFromCol(ColToDo, filePathToDo))
            {
                // 成功
                CloseWindow = true;
            }
            else
            {
                // 失敗
            }
        }

        /// <summary>
        /// OKボタンが実行可能かどうかを判定
        /// </summary>
        /// <returns></returns>
        public bool CanExecute()
        {
            // タイトルが空文字でない、かつ期限ありの場合は期限が今日以降ならばtrue
            return !string.IsNullOrEmpty(OneToDo.Title) && !(OneToDo.IgnoreDate == 0 && OneToDo.Date < DateTime.Today);
            //return true;
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

            return ret;
        }
    }
}
