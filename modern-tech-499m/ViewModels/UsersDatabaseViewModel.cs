using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.ViewModels
{
    public class UsersDatabaseViewModel : IViewModel
    {
        private IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UsersDatabaseViewModel(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
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


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
