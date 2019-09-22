using modern_tech_499m.Logic;
using modern_tech_499m.AILogic;
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
        private bool gameStopPending;

        private Action<string> updateField;
        private Action<IPlayer> showGameEnding;
        private Action<IPlayer> showAIWorkingMessage, stopAIWorkingMessage;
        private Action<IPlayer> showWaitingForUserMessage, stopWaitingForUserMessage;

        public GameController(IPlayer player1, IPlayer player2, IPlayer firstPlayer, GameLogic gameLogic, Action<string> updateField,
            Action<IPlayer> showGameEnding, Action<IPlayer> showAIWorkingMessage, Action<IPlayer> stopAIWorkingMessage, Action<IPlayer> showWaitingForUserMessage, Action<IPlayer> stopWaitingForUserMessage)
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
            gameStopPending = false;
            updateField("Game starting");
            for (; ; )
            {
                if (gameStopPending)
                {
                    updateField("Game interrupted");
                    return;
                }
                if (currentPlayer is UserPlayer)
                {
                    showWaitingForUserMessage(currentPlayer);
                    await WaitForUserMove();
                    stopWaitingForUserMessage(currentPlayer);
                }
                if (currentPlayer is AIPlayer)
                {
                    showAIWorkingMessage(currentPlayer);
                    AISolver solver = new AISolver(gameLogic, player1, player2, currentPlayer);
                    lastCell = (currentPlayer, await solver.GetCell());
                    stopAIWorkingMessage(currentPlayer);
                }
                MoveResult moveResult = gameLogic.MakeMove(lastCell.player, lastCell.cellNumber);
                updateField(Enum.GetName(typeof(MoveResult), moveResult));
                if (moveResult == MoveResult.ImpossibleMove)
                    continue;
                currentPlayer = currentPlayer.Equals(player1) ? player2 : player1;
                if (moveResult == MoveResult.GameEnded)
                {
                    showGameEnding(currentPlayer);
                    return;
                }
            }
        }

        public void StopGame()
        {
            gameStopPending = true;
        }

        private Task WaitForUserMove()
        {
            return Task.Factory.StartNew(() => cellSelectedEvent.WaitOne());
        }
    }
}