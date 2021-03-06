﻿using System.Collections.Generic;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    public class UsersDatabaseListViewModel : BaseViewModel
    {
        /// <summary>
        /// The users item list
        /// </summary>
        public List<UsersDatabaseListItemViewModel> Items { get; set; }

        public UsersDatabaseListViewModel(List<UsersDatabaseListItemViewModel> items)
        {
            Items = items;
        }
    }
}
