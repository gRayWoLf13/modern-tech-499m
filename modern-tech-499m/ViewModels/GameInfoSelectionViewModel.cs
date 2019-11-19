using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using modern_tech_499m.Repositories.Core.Domain;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.ViewModels
{
    class GameInfoSelectionViewModel : BaseViewModel, IEntitySelectionViewModel<GameInfo>
    {
        private readonly IGameInfoRepository _gameInfoRepository;
        private readonly IUserRepository _userRepository;

        public GameInfoSelectionViewModel(IGameInfoRepository gameInfoRepository, IUserRepository userRepository)
        {
            _gameInfoRepository = gameInfoRepository;
            _userRepository = userRepository;
            LoadGames();
        }

        public void LoadGames()
        {
            var gameInfos = _gameInfoRepository.GetAll();
            var wrappedGames = new List<GameInfoWrapper>();
            foreach (var gameInfo in gameInfos)
            {
                string user1Name, user2Name;
                var gameUsers = _userRepository.GetUserFromGame(gameInfo.Id).ToList();
                switch (gameUsers.Count)
                {
                    case 0: user1Name = "AI 1";
                        user2Name = "AI 2";
                        break;
                    case 1: user1Name = gameUsers[0].FullName;
                        user2Name = "AI";
                        break;
                    case 2: user1Name = gameUsers[0].FullName;
                        user2Name = gameUsers[1].FullName;
                        break;
                    default: throw new ArgumentException("Something is really wrong here...", nameof(gameInfo));
                }

                wrappedGames.Add(new GameInfoWrapper
                    {GameInfo = gameInfo, Player1Name = user1Name, Player2Name = user2Name});
            }
            WrappedGameInfos = wrappedGames;
        }
        public class GameInfoWrapper
        {
            public GameInfo GameInfo { get; set; }
            public string Player1Name { get; set; }
            public string Player2Name { get; set; }
        }

        private GameInfoWrapper _gameInfoWrapper;
        public GameInfoWrapper CurrentGameInfoWrapper
        {
            get => _gameInfoWrapper;
            set
            {
                _gameInfoWrapper = value;
                if (value != null)
                    SelectedEntity = value.GameInfo;
                OnPropertyChanged();
            }
        }

        private IEnumerable<GameInfoWrapper> _wrappedGameInfos;
        public IEnumerable<GameInfoWrapper> WrappedGameInfos
        {
            get => _wrappedGameInfos;
            set
            {
                _wrappedGameInfos = value;
                OnPropertyChanged();
            }
        }
        public GameInfo SelectedEntity { get; set; }
    }
}
