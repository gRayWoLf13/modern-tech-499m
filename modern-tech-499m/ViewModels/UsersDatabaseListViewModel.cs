using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using modern_tech_499m.Commands;
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
