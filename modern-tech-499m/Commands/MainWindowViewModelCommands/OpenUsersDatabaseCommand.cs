using System;
using System.Windows.Input;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Commands.MainWindowViewModelCommands
{
    class OpenUsersDatabaseCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MainWindowViewModel _viewModel;

        public OpenUsersDatabaseCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _logger.Debug("Open users database command called");
            _viewModel.UsersDatabaseViewOpen = true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
