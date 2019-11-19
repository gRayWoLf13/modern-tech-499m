using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class SaveGameCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public SaveGameCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.GameController != null && _viewModel.Player1 != null && _viewModel.Player2 != null;
        }

        public void Execute(object parameter)
        {
            _viewModel.GameController.SaveGame();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
