using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands
{
    internal class OpenAddUserViewCommand : ICommand
    {
        private readonly UsersDatabaseViewModel _viewModel;

        public OpenAddUserViewCommand(UsersDatabaseViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.AddUserViewOpen = true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
