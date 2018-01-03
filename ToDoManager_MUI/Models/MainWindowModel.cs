using Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using ToDoManager_MUI.Common;

namespace ToDoManager_MUI.Models
{
    public class MainWindowModel : Notification
    {
        /// <summary>
        /// ToDoコレクション
        /// </summary>
        public ObservableCollection<ToDo> ColToDo;

        /// <summary>
        /// ToDoを削除する
        /// </summary>
        /// <param name="obj"></param>
        public void Execute(ToDo obj)
        {
            ColToDo.Remove(obj);
        }
    }
}
