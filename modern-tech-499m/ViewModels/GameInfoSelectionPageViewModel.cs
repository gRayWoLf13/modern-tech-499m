using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.ViewModels.Base;
using NLog;

namespace modern_tech_499m.ViewModels
{
    public class GameInfoSelectionPageViewModel : BaseViewModel
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGameInfoRepository _gameInfoRepository;
        private readonly IUserRepository _userRepository;

        public GameInfoSelectionPageViewModel(IGameInfoRepository gameInfoRepository, IUserRepository userRepository)
        {
            _logger.Debug("Game info selection view model constructor called");
            _gameInfoRepository = gameInfoRepository;
            _userRepository = userRepository;
            LoadGames();
        }

        public void LoadGames()
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
                    IsSelected = false,
                    Player1Name = user1Name,
                    Player2Name = user2Name,
                    WasGameFinished = gameInfo.GameFinished
                });
            }

            GameInfoListViewModel = new GameInfoListViewModel(gamesList);
        }

        public GameInfoListViewModel GameInfoListViewModel { get; set; }
    }
}
