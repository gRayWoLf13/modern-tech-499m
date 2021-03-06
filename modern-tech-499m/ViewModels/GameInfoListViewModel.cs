﻿using System.Collections.Generic;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// A viewmodel for the game info list
    /// </summary>
    public class GameInfoListViewModel : BaseViewModel
    {
        /// <summary>
        /// The game info item list
        /// </summary>
        public List<GameInfoListItemViewModel> Items { get; set; }

        public GameInfoListItemViewModel SelectedItem { get; set; }

        public GameInfoListViewModel(List<GameInfoListItemViewModel> items)
        {
            Items = items;
        }
    }
}
