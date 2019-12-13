using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using modern_tech_499m.Commands.UsersDatabaseViewModelCommands;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels.Base;
using NLog;

namespace modern_tech_499m.ViewModels
{
    class UsersDatabaseViewModel : BaseViewModel
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UsersDatabaseViewModel(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _logger.Debug("Users database view model constructor called");
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            OpenAddUserViewCommand = new OpenAddUserViewCommand(this);
            DeleteCurrentUserCommand = new DeleteCurrentUserCommand(this, _userRepository);
            UpdateUsersTableCommand = new UpdateUsersTableCommand(this, _userRepository);
            LoadUsers();
        }

        private void LoadUsers()
        {
            Users = _userRepository.GetAll();
            CurrentUser = Users.FirstOrDefault();
        }

        private IEnumerable<User> _users;
        public IEnumerable<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }

        private bool _addUserViewOpen;
        public bool AddUserViewOpen
        {
            get => _addUserViewOpen;
            set
            {
                _addUserViewOpen = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenAddUserViewCommand { get; private set; }
        public ICommand DeleteCurrentUserCommand { get; private set; }
        public ICommand UpdateUsersTableCommand { get; private set; }
    }
}
