using System;
using System.Windows.Input;
using modern_tech_499m.Logic;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class CellClickCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MainWindowViewModel _viewModel;

        public CellClickCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.GameController != null && _viewModel.Player1 != null && _viewModel.Player2 != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                var values = (object[])parameter;
                IPlayer player = (IPlayer)values[0];
                int cellIndex = Convert.ToInt32(values[1]);
                _logger.Debug($"Cell click command called with parameters {player}, {cellIndex}");
                (player as UserPlayer)?.MakeMove(cellIndex);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Cell click command caused the exception");
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
