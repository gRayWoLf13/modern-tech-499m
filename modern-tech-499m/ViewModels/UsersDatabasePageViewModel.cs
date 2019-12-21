using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using modern_tech_499m.Repositories.Core;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels.Base;
using NLog;

namespace modern_tech_499m.ViewModels
{
    public class UsersDatabasePageViewModel : BaseViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructor

        public UsersDatabasePageViewModel(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _logger.Debug("Users database view model constructor called");
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            LoadUsers();
        }

        #endregion

        #region Public properties

        public UsersDatabaseListViewModel UsersListViewModel { get; set; }

        #endregion

        #region Private methods

        private void LoadUsers()
        {
            var databaseUsers = _userRepository.GetAll();
            var viewUsers = databaseUsers.Select(item => new UsersDatabaseListItemViewModel
            {
                Initials =
                    $"{item.LastName[0]}.{item.FirstName[0]}{(!string.IsNullOrEmpty(item.Patronymic) ? "." : string.Empty)}{(!string.IsNullOrEmpty(item.Patronymic) ? item.Patronymic[0].ToString() : string.Empty)}",
                FullName = $"{item.LastName} {item.LastName} {item.Patronymic}",
                BirthDate = item.BirthDate,
                IsSelected = false,
                Username = "username"
            }).ToList();
            UsersListViewModel = new UsersDatabaseListViewModel(viewUsers);
        }

        #endregion
    }
}
