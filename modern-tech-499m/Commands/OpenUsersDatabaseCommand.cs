using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands
{
    class OpenUsersDatabaseCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public OpenUsersDatabaseCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.UsersDatabaseViewOpen = true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
