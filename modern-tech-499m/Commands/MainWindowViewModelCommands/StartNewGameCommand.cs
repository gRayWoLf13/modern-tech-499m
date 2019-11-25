using System;
using System.Windows.Input;
using modern_tech_499m.Logic;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class StartNewGameCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MainWindowViewModel _viewModel;

        public StartNewGameCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Player1 != null && _viewModel.Player2 != null;
        }

        public void Execute(object parameter)
        {
            _logger.Debug("Start new game command called");
            GameLogic logic = new GameLogic(6, _viewModel.Player1, _viewModel.Player2, _viewModel.Player1);
            _viewModel.GameController?.StopGame();
            _viewModel.GameController = new GameController(logic, _viewModel.GameInfoRepository);
            _viewModel.GameController.RunGame();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
