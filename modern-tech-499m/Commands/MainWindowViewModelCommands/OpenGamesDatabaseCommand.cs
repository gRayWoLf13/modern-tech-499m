using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class OpenGamesDatabaseCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public OpenGamesDatabaseCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var gameInfo = Services.SelectGameInfo();
            _viewModel.GameController = new GameController(gameInfo, _viewModel.GameInfoRepository);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
