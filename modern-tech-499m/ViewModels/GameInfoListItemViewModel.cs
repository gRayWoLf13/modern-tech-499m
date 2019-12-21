using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.ViewModels.Base;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// A viewmodel for each game info item in the list
    /// </summary>
    public class GameInfoListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// The type of the game shown in the circle
        /// for example : User vs User = (UvU)
        /// </summary>
        public string GameType { get; set; }

        /// <summary>
        /// The game score
        /// </summary>
        public int GameScore { get; set; }

        /// <summary>
        /// Value indicating was the game finished or not
        /// </summary>
        public bool WasGameFinished { get; set; }

        /// <summary>
        /// The name of the first player
        /// </summary>
        public string Player1Name { get; set; }

        /// <summary>
        /// The name of the second player
        /// </summary>
        public string Player2Name { get; set; }
    }
}
