﻿using System;
using System.Windows.Input;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels;
using NLog;

namespace modern_tech_499m.Commands.UsersDatabaseViewModelCommands
{
    internal class DeleteCurrentUserCommand : ICommand
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly UsersDatabaseViewModel _viewModel;
        private readonly IUserRepository _userRepository;

        public DeleteCurrentUserCommand(UsersDatabaseViewModel viewModel, IUserRepository userRepository)
        {
            _viewModel = viewModel;
            _userRepository = userRepository;
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.CurrentUser != null;
        }

        public void Execute(object parameter)
        { 
            _logger.Debug("Delete current user command called");
            _userRepository.Remove(_viewModel.CurrentUser);
            _viewModel.UpdateUsersTableCommand.Execute(null);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
