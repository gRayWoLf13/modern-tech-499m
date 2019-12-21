using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// The design-time data for a <see cref="UsersDatabaseListItemViewModel"/>
    /// </summary>
    class UsersDatabaseListItemDesignModel : UsersDatabaseListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static UsersDatabaseListItemDesignModel Instance => new UsersDatabaseListItemDesignModel();

        #endregion

        #region Constructor

        public UsersDatabaseListItemDesignModel()
        {
            Initials = "М.С.С.";
            FullName = "Молочников Сергей Сергеевич";
            Username = "gRayWoLf";
            BirthDate = new DateTime(1996, 1, 13);
            IsSelected = false;
        }

        #endregion
    }
}
