using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// The design-time data for a <see cref="UsersDatabaseListViewModel"/>
    /// </summary>
    class UsersDatabaseListDesignModel : UsersDatabaseListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static UsersDatabaseListDesignModel Instance => new UsersDatabaseListDesignModel();

        #endregion

        #region Constructor

        public UsersDatabaseListDesignModel()
            : base(new List<UsersDatabaseListItemViewModel>
            {
                new UsersDatabaseListItemViewModel()
                {
                    Initials = "М.С.С.",
                    FullName = "Молочников Сергей Сергеевич",
                    Username = "gRayWoLf",
                    BirthDate = new DateTime(1996, 1, 13),
                    IsSelected = false
                },

                new UsersDatabaseListItemViewModel()
                {
                    Initials = "А.А.А.",
                    FullName = "Антонов Антон Антонович",
                    Username = "AntoNoV",
                    BirthDate = new DateTime(2000, 5, 28),
                    IsSelected = false
                },

                new UsersDatabaseListItemViewModel()
                {
                    Initials = "U1",
                    FullName = "User number 1",
                    Username = "Un1",
                    BirthDate = new DateTime(2001, 1, 1),
                    IsSelected = true
                }
            })
        { }

        #endregion
    }
}
