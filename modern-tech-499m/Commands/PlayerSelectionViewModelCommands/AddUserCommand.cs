using System;
using System.Windows.Input;

namespace modern_tech_499m.Commands.PlayerSelectionViewModelCommands
{
    class AddUserCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Services.AddNewUser();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
