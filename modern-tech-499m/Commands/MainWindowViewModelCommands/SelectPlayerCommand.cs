using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class SelectPlayerCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public SelectPlayerCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var param = parameter as string;
            switch (param)
            {
                case "Player1": _viewModel.Player1 = PlayerService.SelectPlayer();
                    break;
                case "Player2": _viewModel.Player2 = PlayerService.SelectPlayer();
                    break;
                default: throw new ArgumentException("Wrong player", nameof(parameter));
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
