﻿using System;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// A viewmodel for each user item in the list
    /// </summary>
    public class UsersDatabaseListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// Initials of the user
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The full name of the user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The birth date of the user
        /// </summary>
        public DateTime? BirthDate { get; set; }
    }
}