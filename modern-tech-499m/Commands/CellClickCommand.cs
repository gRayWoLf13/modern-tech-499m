using System;
using System.Windows.Input;
using modern_tech_499m.Logic;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands
{
    class CellClickCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public CellClickCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            IPlayer player = (IPlayer)values[0];
            int cellIndex = Convert.ToInt32(values[1]);
            (player as UserPlayer)?.MakeMove(cellIndex);
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            _viewModel.GameController = _viewModel.GameController;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
