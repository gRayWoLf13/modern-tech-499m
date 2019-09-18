using modern_tech_499m.Logic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace modern_tech_499m
{
    class GameController
    {
        public readonly IPlayer player1, player2;
        private IPlayer currentPlayer;
        private (IPlayer player, int cellNumber) lastCell;
        public GameLogic gameLogic;
        private readonly EventWaitHandle cellSelectedEvent;

        private Action<string> updateField;
        private Action showGameEnding;
        private Action showAIWorkingMessage, stopAIWorkingMessage;
        private Action showWaitingForUserMessage, stopWaitingForUserMessage;

        public GameController(IPlayer player1, IPlayer player2, IPlayer firstPlayer, GameLogic gameLogic, Action<string> updateField,
            Action showGameEnding, Action showAIWorkingMessage, Action stopAIWorkingMessage, Action showWaitingForUserMessage, Action stopWaitingForUserMessage)
        {
            if (!firstPlayer.Equals(player1) && !firstPlayer.Equals(player2))
                throw new ArgumentException("First player is incorrect");
            cellSelectedEvent = new AutoResetEvent(false);
            this.player1 = player1;
            this.player2 = player2;
            currentPlayer = firstPlayer;
            this.gameLogic = gameLogic;
            this.updateField = updateField;
            this.showGameEnding = showGameEnding;
            this.showAIWorkingMessage = showAIWorkingMessage;
            this.stopAIWorkingMessage = stopAIWorkingMessage;
            this.showWaitingForUserMessage = showWaitingForUserMessage;
            this.stopWaitingForUserMessage = stopWaitingForUserMessage;
        }

        public void SetLastCell(IPlayer player, int cellIndex)
        {
            if (!player.Equals(player1) && !player.Equals(player2))
                throw new ArgumentException("Player is incorrect");
            lastCell = (player, cellIndex);
            cellSelectedEvent.Set();
        }

        public async void RunGame()
        {
            updateField("Game starting");
            for (; ; )
            {
                if (currentPlayer is UserPlayer)
                {
                    showWaitingForUserMessage();
                    await WaitForUserMove();
                    stopWaitingForUserMessage();
                }
                if (currentPlayer is AIPlayer)
                {
                    showAIWorkingMessage();
                    lastCell = (currentPlayer, await AISolver.GetCell());
                    stopAIWorkingMessage();
                }
                MoveResult moveResult = gameLogic.MakeMove(lastCell.player, lastCell.cellNumber);
                updateField(Enum.GetName(typeof(MoveResult), moveResult));
                if (moveResult == MoveResult.ImpossibleMove)
                    continue;
                currentPlayer = currentPlayer.Equals(player1) ? player2 : player1;
                if (moveResult == MoveResult.GameEnded)
                {
                    showGameEnding();
                    return;
                }
            }
        }

        private Task WaitForUserMove()
        {
            return Task.Factory.StartNew(() => cellSelectedEvent.WaitOne());
        }
    }
}