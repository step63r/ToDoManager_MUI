using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using ToDoManager_MUI.Common;
using ToDoManager_MUI.Models;

namespace ToDoManager_MUI.ViewModels
{
    public class CreateViewModel : BindableBase
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
        private RelayCommand createDialog_OK;
        public RelayCommand CreateDialog_OK
        {
            get
            {
                if (createDialog_OK == null)
                {
                    createDialog_OK = new RelayCommand(Execute, CanExecute);
                }
                return createDialog_OK;
            }
        }
        /// <summary>
        /// キャンセルボタン
        /// </summary>
        private RelayCommand createDialog_Cancel;
        public RelayCommand CreateDialog_Cancel
        {
            get
            {
                if (createDialog_Cancel == null)
                {
                    createDialog_Cancel = new RelayCommand(CancelExecute, CanCancelExecute);
                }
                return createDialog_Cancel;
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
            }
        }
        // ファイルパス
        private static string filePathToDo = string.Format("{0}/{1}", FilePath.BaseDir, FilePath.XmlPathToDo);
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="onetodo"></param>
        public CreateViewModel(ToDo onetodo)
        {
            ColToDo = Load();
            if (onetodo is null)
            {
                // 新規作成
                OneToDo = new ToDo()
                {
                    ID = 0,
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
        }

        /// <summary>
        /// OKボタンのコマンド実行
        /// </summary>
        public void Execute()
        {
            // クラスを作って
            var createModel = new CreateModel()
            {
                // オブジェクトを渡して
                ColToDo = ColToDo
            };
            // 動かして
            if (OneToDo.ID == 0)
            {
                // IDが0なら新規作成
                createModel.Add(OneToDo);
            }
            else
            {
                // それ以外なら更新
                createModel.Update(OneToDo);
            }
            
            // 戻す
            ColToDo = createModel.ColToDo;

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
        }

        /// <summary>
        /// キャンセルボタンのコマンド実行
        /// </summary>
        public void CancelExecute()
        {
            CloseWindow = true;
        }

        /// <summary>
        /// キャンセルボタンが実行可能かどうかを判定
        /// </summary>
        /// <returns></returns>
        public bool CanCancelExecute()
        {
            // 常にtrue
            return true;
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
