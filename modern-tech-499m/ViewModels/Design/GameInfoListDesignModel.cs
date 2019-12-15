using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// The design-time data for a <see cref="GameInfoListViewModel"/>
    /// </summary>
    class GameInfoListDesignModel : GameInfoListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static GameInfoListDesignModel Instance => new GameInfoListDesignModel();

        #endregion

        #region Constructor

        public GameInfoListDesignModel()
            : base(new List<GameInfoListItemViewModel>
            {
                new GameInfoListItemViewModel()
                {
                    GameType = "AIvU",
                    GameScore = 13,
                    WasGameFinished = true,
                    Player1Name = "First bot",
                    Player2Name = "Second player",
                },

                new GameInfoListItemViewModel()
                {
                    GameType = "UvU",
                    GameScore = 5,
                    WasGameFinished = false,
                    Player1Name = "First player",
                    Player2Name = "Second player",
                },

                new GameInfoListItemViewModel()
                {
                    GameType = "UvU",
                    GameScore = -8,
                    WasGameFinished = false,
                    Player1Name = "First winner",
                    Player2Name = "Second loser",
                    IsSelected = true
                },

                new GameInfoListItemViewModel()
                {
                    GameType = "UvU",
                    GameScore = 0,
                    WasGameFinished = true,
                    Player1Name = "First broken user",
                    Player2Name = "Second broken user",
                }

            })
        {
        }

        #endregion
    }
}
