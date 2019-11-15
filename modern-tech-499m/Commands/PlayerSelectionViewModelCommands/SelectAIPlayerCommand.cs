using System;
using System.Windows.Input;
using modern_tech_499m.Logic;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands.PlayerSelectionViewModelCommands
{
    class SelectAIPlayerCommand : ICommand
    {
        private readonly PlayerSelectionViewModel _playerSelectionViewModel;

        public SelectAIPlayerCommand(PlayerSelectionViewModel playerSelectionViewModel)
        {
            _playerSelectionViewModel = playerSelectionViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _playerSelectionViewModel.SelectedPlayer = new AIPlayer("AI player");
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
