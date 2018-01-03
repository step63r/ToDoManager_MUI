using System;
using System.Windows.Input;

namespace ToDoManager_MUI.Common
{
    public class RelayCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action execute)
            : this(execute, () => true)
        {
        }

        public void Execute()
        {
            this.execute();
        }

        public bool CanExecute()
        {
            return this.canExecute();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        void ICommand.Execute(object parameter)
        {
            this.execute();
        }
    }
}
