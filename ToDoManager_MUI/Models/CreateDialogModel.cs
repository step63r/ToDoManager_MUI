using Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using ToDoManager_MUI.Common;

namespace ToDoManager_MUI.Models
{
    public class CreateDialogModel : Notification
    {
        /// <summary>
        /// ToDoコレクション
        /// </summary>
        public ObservableCollection<ToDo> ColToDo;

        /// <summary>
        /// ToDoを追加する
        /// </summary>
        /// <param name="obj"></param>
        public void Execute(ToDo obj)
        {
            ColToDo.Add(obj);
        }
    }
}
