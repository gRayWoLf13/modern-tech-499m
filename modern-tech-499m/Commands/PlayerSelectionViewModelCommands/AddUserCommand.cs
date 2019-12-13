using System;
using System.Windows.Input;
using NLog;

namespace modern_tech_499m.Commands.PlayerSelectionViewModelCommands
{
    class AddUserCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _logger.Debug("Add user command called");
            Services.AddNewUser();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
