using Prism.Interactivity.InteractionRequest;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ToDoManager_MUI.Common;

namespace ToDoManager_MUI.Models
{
    public class CreateModel : Notification
    {
        /// <summary>
        /// ToDoコレクション
        /// </summary>
        public ObservableCollection<ToDo> ColToDo;

        /// <summary>
        /// ToDoを追加する
        /// </summary>
        /// <param name="obj"></param>
        public void Add(ToDo obj)
        {
            // 新規作成はID取得が必要
            obj.ID = GetId();

            // 無期限のタスクは日付をnull値にしておく
            if (obj.IgnoreDate == 1)
            {
                obj.Date = null;
            }

            ColToDo.Add(obj);
        }

        /// <summary>
        /// ToDoを更新する
        /// </summary>
        /// <param name="obj"></param>
        public void Update(ToDo obj)
        {
            // IDが同一のオブジェクトを探す
            var retToDo = ColToDo.Where(item => item.ID == obj.ID).First();

            {
                retToDo.Title = obj.Title;
                retToDo.Detail = obj.Detail;
                retToDo.IgnoreDate = obj.IgnoreDate;
                if (retToDo.IgnoreDate == 1)
                {
                    retToDo.Date = null;
                }
                else
                {
                    retToDo.Date = obj.Date;
                }
            }

        }

        /// <summary>
        /// ToDoコレクションから未使用のIDを取得する
        /// </summary>
        /// <returns>未使用のIDのうち最も小さい数字</returns>
        private int GetId()
        {
            int ret = 1;
            List<int> IDList = new List<int>();

            foreach (ToDo todo in ColToDo)
            {
                IDList.Add(todo.ID);
            }

            // 昇順に並び替える（はず）
            IDList.Sort();

            foreach (int id in IDList)
            {
                // IDリストを小さい方から調べてポインタ値より大きければ終了
                if (id > ret)
                {
                    break;
                }
                ret++;
            }

            return ret;
        }
    }
}
