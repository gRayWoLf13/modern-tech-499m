using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class UndoRedoMoveCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly MainWindowViewModel _viewModel;

        public UndoRedoMoveCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.GameController != null && _viewModel.Player1 != null && _viewModel.Player2 != null;
        }

        public void Execute(object parameter)
        {
            var param = Convert.ToBoolean(parameter);
            _logger.Debug($"Undo/redo mode command called with parameter {param}");
            if (param)
                _viewModel.GameController?.UndoMove();
            else
                _viewModel.GameController?.RedoMove();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
