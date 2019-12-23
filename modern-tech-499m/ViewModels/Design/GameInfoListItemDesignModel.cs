namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// The design-time data for a <see cref="GameInfoListItemViewModel"/>
    /// </summary>
    class GameInfoListItemDesignModel : GameInfoListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static GameInfoListItemDesignModel Instance => new GameInfoListItemDesignModel();

        #endregion

        #region Constructor

        public GameInfoListItemDesignModel()
        {
            GameType = "AIvU";
            GameScore = 0;
            WasGameFinished = true;
            Player1Name = "First bot";
            Player2Name = "Second player";
        }

        #endregion
    }
}
