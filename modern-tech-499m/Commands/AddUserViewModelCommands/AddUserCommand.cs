using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Commands.AddUserViewModelCommands
{
    internal class AddUserCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly AddUserViewModel _viewModel;
        private readonly IUserRepository _userRepository;

        public AddUserCommand(AddUserViewModel viewModel, IUserRepository userRepository)
        {
            _viewModel = viewModel;
            _userRepository = userRepository;
        }

        public bool CanExecute(object parameter)
        {
            return typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetIndexParameters().Length == 0)
                .All(property => string.IsNullOrWhiteSpace(_viewModel.NewUser[property.Name]));
        }

        public void Execute(object parameter)
        {
            _logger.Debug("Add user command called");
            _userRepository.Add(_viewModel.NewUser);
            _viewModel.NewUser = new User() {BirthDate = DateTime.Today};
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
