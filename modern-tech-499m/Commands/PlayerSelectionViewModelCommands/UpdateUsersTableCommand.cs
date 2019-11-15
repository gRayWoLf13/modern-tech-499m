using System;
using System.Linq;
using System.Windows.Input;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels;

namespace modern_tech_499m.Commands.PlayerSelectionViewModelCommands
{
    class UpdateUsersTableCommand : ICommand
    {
        private readonly PlayerSelectionViewModel _playerSelectionViewModel;
        private readonly IUserRepository _userRepository;

        public UpdateUsersTableCommand(PlayerSelectionViewModel playerSelectionViewModel, IUserRepository userRepository)
        {
            _playerSelectionViewModel = playerSelectionViewModel;
            _userRepository = userRepository;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _playerSelectionViewModel.Users = _userRepository.GetAll();
            _playerSelectionViewModel.CurrentUser = _playerSelectionViewModel.Users.FirstOrDefault();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
