using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using modern_tech_499m.Commands.PlayerSelectionViewModelCommands;
using modern_tech_499m.Logic;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.ViewModels
{
    class PlayerSelectionViewModel : BaseViewModel, IEntitySelectionViewModel<IPlayer>
    {
        private IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public PlayerSelectionViewModel(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            AddUserCommand = new AddUserCommand();
            SelectAIPlayerCommand = new SelectAIPlayerCommand(this);
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
                if (CurrentUser != null)
                    SelectedEntity = new UserPlayer(_currentUser.FullName, CurrentUser);
                OnPropertyChanged();
            }
        }

        private IPlayer _selectedEntity;
        public IPlayer SelectedEntity
        {
            get => _selectedEntity;
            set
            {
                _selectedEntity = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddUserCommand { get; private set; }
        public ICommand SelectAIPlayerCommand { get; private set; }
        public ICommand UpdateUsersTableCommand { get; private set; }
    }
}
