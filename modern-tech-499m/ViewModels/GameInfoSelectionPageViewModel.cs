using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using modern_tech_499m.Commands;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels.Base;
using NLog;

namespace modern_tech_499m.ViewModels
{
    public class GameInfoSelectionPageViewModel : BaseViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGameInfoRepository _gameInfoRepository;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructor

        public GameInfoSelectionPageViewModel(IGameInfoRepository gameInfoRepository, IUserRepository userRepository)
        {
            _logger.Debug("Game info selection view model constructor called");
            _gameInfoRepository = gameInfoRepository;
            _userRepository = userRepository;
            ChooseGameCommand = new RelayCommand(ChooseGame);
            LoadGames();
        }

        #endregion

        #region Public members

        /// <summary>
        /// Viewmodel of the games list
        /// </summary>
        public GameInfoListViewModel GameInfoListViewModel { get; set; }

        /// <summary>
        /// Command to load chosen game and navigate back to game page
        /// </summary>
        public ICommand ChooseGameCommand { get; set; }

        #endregion

        #region Private methods

        private void LoadGames()
        {
            var gameInfos = _gameInfoRepository.GetAll();
            var gamesList = new List<GameInfoListItemViewModel>();
            foreach (var gameInfo in gameInfos)
            {
                string user1Name, user2Name, gameType;
                var gameUsers = _userRepository.GetUserFromGame(gameInfo.Id).ToList();
                switch (gameUsers.Count)
                {
                    case 0:
                        user1Name = "Bot number 1";
                        user2Name = "Bot number 2";
                        gameType = "AIvAI";
                        break;
                    case 1:
                        user1Name = gameUsers[0].FullName;
                        user2Name = "Bot";
                        gameType = "UvAI";
                        break;
                    case 2:
                        user1Name = gameUsers[0].FullName;
                        user2Name = gameUsers[1].FullName;
                        gameType = "UvU";
                        break;
                    default: throw new ArgumentException("Something is really wrong here...", nameof(gameInfo));
                }

                gamesList.Add(new GameInfoListItemViewModel
                {
                    GameType = gameType,
                    GameScore = gameInfo.Score,
                    Player1Name = user1Name,
                    Player2Name = user2Name,
                    WasGameFinished = gameInfo.GameFinished,
                    InternalGameInfo = gameInfo
                });
            }

            GameInfoListViewModel = new GameInfoListViewModel(gamesList);
        }

        private void ChooseGame()
        {
            var gameInfo = GameInfoListViewModel.SelectedItem?.InternalGameInfo;
            var gamePageViewModel = BootStrapper.Resolve<GamePageViewModel>();
            gamePageViewModel.GameController = new GameController(gameInfo, _gameInfoRepository);
            ViewModelLocator.ApplicationViewModel.GoToPage(ApplicationPage.Game, gamePageViewModel);
        }
        #endregion
    }
}
