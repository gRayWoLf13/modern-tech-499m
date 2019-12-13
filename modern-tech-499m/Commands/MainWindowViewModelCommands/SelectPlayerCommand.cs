using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class SelectPlayerCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
            _logger.Debug($"Select player command called with parameter {param}");
            switch (param)
            {
                case "Player1":
                    _viewModel.Player1 = Services.SelectPlayer();
                    break;
                case "Player2":
                    _viewModel.Player2 = Services.SelectPlayer();
                    break;
                default:
                    {
                        var exception = new ArgumentException("Wrong player", nameof(parameter));
                        _logger.Fatal(exception, "Select player command caused an exception");
                        throw exception;
                    }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
