using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using modern_tech_499m.Commands.AddUserViewModelCommands;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;
using NLog;

namespace modern_tech_499m.ViewModels
{
    class AddUserViewModel : INotifyPropertyChanged
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserRepository _userRepository;
        public AddUserViewModel(IUserRepository userRepository)
        {
            _logger.Debug("Add user view movel constructor called");
            _userRepository = userRepository;
            AddUserCommand = new AddUserCommand(this, userRepository);
            NewUser = new User() {BirthDate = DateTime.Today};
        }

        private User _newUser;
        public User NewUser
        {
            get => _newUser;
            set
            {
                _newUser = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddUserCommand { get; private set; }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
