using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands
{
    class UndoRedoMoveCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public UndoRedoMoveCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var param = Convert.ToBoolean(parameter);
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
