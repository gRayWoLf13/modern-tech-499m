using modern_tech_499m.Logic;
using modern_tech_499m.AILogic;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace modern_tech_499m
{
    class GameController
    {
        public GameLogic gameLogic;
        private bool gameStopPending;

        private Action<string> updateField;
        private Action<IPlayer> showGameEnding;
        private Action<IPlayer> showPlayerWorkingMessage, stopPlayerWorkingMessage;

        public GameController(GameLogic gameLogic, Action<string> updateField,
            Action<IPlayer> showGameEnding, Action<IPlayer> showPlayerWorkingMessage, Action<IPlayer> stopPlayerWorkingMessage)
        {
            this.gameLogic = gameLogic;
            this.updateField = updateField;
            this.showGameEnding = showGameEnding;
            this.showPlayerWorkingMessage = showPlayerWorkingMessage;
            this.stopPlayerWorkingMessage = stopPlayerWorkingMessage;
            this.gameLogic.Player1.OnGetCell += new EventHandler<CellGetterEventArgs>(RecieveCellNumber);
            this.gameLogic.Player2.OnGetCell += new EventHandler<CellGetterEventArgs>(RecieveCellNumber);
        }

        public void RunGame()
        {
            gameStopPending = false;
            updateField("Game starting");
            MakeGameStep();
        }

        public void StopGame()
        {
            gameStopPending = true;
        }

        private void MakeGameStep()
        {
            showPlayerWorkingMessage(gameLogic.CurrentPlayer);
            gameLogic.CurrentPlayer.GetCell(gameLogic);
        }

        private void RecieveCellNumber(object sender, CellGetterEventArgs eventArgs)
        {
            MoveResult moveResult = gameLogic.MakeMove(sender as IPlayer, eventArgs.CellNumber);
            updateField(Enum.GetName(typeof(MoveResult), moveResult));
            if (moveResult == MoveResult.GameEnded)
            {
                showGameEnding(sender as IPlayer);
                return;
            }
            if (gameStopPending)
            {
                updateField("Game interrupted");
                return;
            }
            if (moveResult != MoveResult.ImpossibleMove)
            {
                stopPlayerWorkingMessage(sender as IPlayer);
            }
            MakeGameStep();
        }
    }
}