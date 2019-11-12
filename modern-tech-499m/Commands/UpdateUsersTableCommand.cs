using System;
using System.Linq;
using System.Windows.Input;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands
{
    internal class UpdateUsersTableCommand : ICommand
    {
        private readonly UsersDatabaseViewModel _viewModel;
        private readonly IUserRepository _userRepository;

        public UpdateUsersTableCommand(UsersDatabaseViewModel viewModel, IUserRepository userRepository)
        {
            _viewModel = viewModel;
            _userRepository = userRepository;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.Users = _userRepository.GetAll();
            _viewModel.CurrentUser = _viewModel.Users.FirstOrDefault();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
