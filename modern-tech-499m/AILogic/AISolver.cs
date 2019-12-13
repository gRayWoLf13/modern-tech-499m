using modern_tech_499m.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using NLog;

namespace modern_tech_499m.AILogic
{
    class AISolver
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public int CalculationDepth { get; }
        public int CellsCount { get; }
        private readonly Random _rand = new Random();
        public bool UseAlphaBetaProcedure { get; }

        public AISolver(bool useDefaultSettings)
        {
            if (useDefaultSettings)
            {
                _logger.Debug("AI solver constructor called with default settings");
                string cellsCount = ConfigurationManager.AppSettings["CellsCount"];
                if (cellsCount == null || !int.TryParse(cellsCount, out int count))
                    CellsCount = 6;
                else
                    CellsCount = count;
                string calculationDepth = ConfigurationManager.AppSettings["CalculationDepth"];
                if (calculationDepth == null || !int.TryParse(calculationDepth, out int depth))
                    CalculationDepth = 4;
                else
                    CalculationDepth = depth;
                string useAlphaBetaProcedure = ConfigurationManager.AppSettings["UseAlphaBetaProcedure"];
                if (useAlphaBetaProcedure == null || !bool.TryParse(useAlphaBetaProcedure, out bool alphaBeta))
                    UseAlphaBetaProcedure = false;
                else
                    UseAlphaBetaProcedure = alphaBeta;
            }
        }

        public int GetCell(GameLogic logic)
        {
            SolvingTreeNode solvingTreeHead = CreateSolvingTree(logic);
            LinkedListNode<SolvingTreeNode> currentChild = solvingTreeHead.ChildrenNodes.First;
            List<int> solvingVariants = new List<int>();
            while (currentChild != null)
            {
                if (currentChild.Value.GameStateWeight.Value == solvingTreeHead.GameStateWeight.Value)
                    solvingVariants.Add(currentChild.Value.ParentCellNumber);
                currentChild = currentChild.Next;
            }

            int cellNumber = _rand.Next(solvingVariants.Count);
            _logger.Info($"AI solver have choosen cell number {cellNumber}");
            return solvingVariants[cellNumber];
            //return solvingVariants[0];
        }

        private SolvingTreeNode CreateSolvingTree(GameLogic logic)
        {
            Stack<SolvingTreeNode> treeNodes = new Stack<SolvingTreeNode>();
            SolvingTreeNode head = new SolvingTreeNode()
            {
                CurrentGameState = logic.Clone() as GameLogic,
                ParentNode = null,
                GameMoveOwner = logic.CurrentPlayer,
                ChildrenNodes = new LinkedList<SolvingTreeNode>(),
                TreeNodeDepth = 0
            };

            AddChildrenNodes(head, logic);
            LinkedListNode<SolvingTreeNode> currentHeadChild = head.ChildrenNodes.Last;
            while (currentHeadChild != null)
            {
                treeNodes.Push(currentHeadChild.Value);
                currentHeadChild = currentHeadChild.Previous;
            }
            do
            {
                SolvingTreeNode currentNode = treeNodes.Pop();
                if (AddChildrenNodes(currentNode, logic))
                {
                    SolvingTreeNode tmpNode = currentNode;
                    (bool continueToParent, bool checkNeeded) pushingResult = TryPushWeightToParentCells(tmpNode);
                    while (pushingResult.continueToParent)
                    {
                        if (pushingResult.checkNeeded)
                            CheckNode(tmpNode);
                        tmpNode = tmpNode.ParentNode;
                        pushingResult = TryPushWeightToParentCells(tmpNode);
                    }
                }
                LinkedListNode<SolvingTreeNode> currentChild = currentNode.ChildrenNodes.Last;
                while (currentChild != null)
                {
                    treeNodes.Push(currentChild.Value);
                    currentChild = currentChild.Previous;
                }
            } while (treeNodes.Count > 0);
            _logger.Debug("Solving tree created");
            return head;
        }

        private bool CheckNode(SolvingTreeNode treeNode)
        {
            bool deletionNeeded = CheckTreeNodeToStopCalculation(treeNode);
            if (deletionNeeded)
            {
                treeNode.GameStateWeight = treeNode.PreviewGameStateWeight;
                treeNode.ChildrenNodes.Clear();
                return true;
            }
            return false;
        }

        private (bool continueToParent, bool checkNeeded) TryPushWeightToParentCells(SolvingTreeNode currentNode)
        {
            if (currentNode.ParentNode == null)
                return (false, false);
            var cellParent = currentNode.ParentNode;
            LinkedListNode<SolvingTreeNode> currentChild = cellParent.ChildrenNodes.First;
            if (currentChild == null || !currentChild.Value.GameStateWeight.HasValue)
                return (false, false);
            int value = currentChild.Value.GameStateWeight.Value;

            bool checkNeeded = false;
            if (UseAlphaBetaProcedure)
                checkNeeded = TrySetPreviewWeightForNode(cellParent, value);

            currentChild = currentChild.Next;
            while(currentChild != null)
            {
                if (!currentChild.Value.GameStateWeight.HasValue)
                    return (false, checkNeeded);
                if (currentNode.TreeNodeDepth % 2 == 0)
                    value = Math.Max(value, currentChild.Value.GameStateWeight.Value);
                else
                    value = Math.Min(value, currentChild.Value.GameStateWeight.Value);

                if (UseAlphaBetaProcedure)
                {
                    var tmpCheck = TrySetPreviewWeightForNode(currentNode, value);
                    if (!checkNeeded)
                        checkNeeded = tmpCheck;
                }

                currentChild = currentChild.Next;
            }
            if (cellParent.GameStateWeight.HasValue)
                throw new Exception();
            cellParent.GameStateWeight = value;
            return (true, checkNeeded);
        }

        private bool TrySetPreviewWeightForNode(SolvingTreeNode treeNode, int previewWeight)
        {
            if (!treeNode.PreviewGameStateWeight.HasValue)
            {
                treeNode.PreviewGameStateWeight = previewWeight;
                return true;
            }
            if (treeNode.TreeNodeDepth % 2 == 0)
            {
                if (previewWeight > treeNode.PreviewGameStateWeight.Value)
                {
                    treeNode.PreviewGameStateWeight = previewWeight;
                    return true;
                }
                return false;
            }
            else
            {
                if (previewWeight < treeNode.PreviewGameStateWeight.Value)
                {
                    treeNode.PreviewGameStateWeight = previewWeight;
                    return true;
                }
                return false;
            }
        }

        private bool CheckTreeNodeToStopCalculation(SolvingTreeNode treeNode)
        {
            if (treeNode.TreeNodeDepth == CalculationDepth)
                return false;
            int? parentPreviewWeight = treeNode.ParentNode.PreviewGameStateWeight;
            if (!parentPreviewWeight.HasValue)
                return false;
            if (treeNode.TreeNodeDepth % 2 == 0)
            {
                if (treeNode.PreviewGameStateWeight >= parentPreviewWeight)
                    return true;
                return false;
            }
            else
            {
                if (treeNode.PreviewGameStateWeight <= parentPreviewWeight)
                    return true;
                return false;
            }
        }

        private bool AddChildrenNodes(SolvingTreeNode treeNode, GameLogic logic)
        {
            if (treeNode.TreeNodeDepth == CalculationDepth)
            {
                treeNode.GameStateWeight = GetWeightFunctionValue(logic.CurrentPlayer, treeNode.CurrentGameState, logic);
                treeNode.PreviewGameStateWeight = treeNode.GameStateWeight;
                return true;
            }
            for(int i = 0; i < CellsCount; i++)
            {
                GameLogic logicCopy = treeNode.CurrentGameState.Clone() as GameLogic;
                MoveResult moveResult = logicCopy.MakeMove(treeNode.GameMoveOwner, i);
                if (moveResult == MoveResult.ImpossibleMove)
                    continue;
                IPlayer childMoveOwner = logic.GetOtherPlayer(treeNode.GameMoveOwner);
                SolvingTreeNode childNode = new SolvingTreeNode()
                {
                    CurrentGameState = logicCopy,
                    ParentNode = treeNode,
                    GameMoveOwner = childMoveOwner,
                    ParentCellNumber = i,
                    ChildrenNodes = new LinkedList<SolvingTreeNode>(),
                    TreeNodeDepth = treeNode.TreeNodeDepth + 1
                };
                treeNode.ChildrenNodes.AddLast(childNode);
            }
            if (treeNode.ChildrenNodes.Count == 0)
            {
                treeNode.GameStateWeight = GetWeightFunctionValue(logic.CurrentPlayer, treeNode.CurrentGameState, logic);
                treeNode.PreviewGameStateWeight = treeNode.GameStateWeight;
                return true;
            }
            return false;
        }

        private int GetWeightFunctionValue(IPlayer currentPlayer, GameLogic gameLogic, GameLogic oldLogic)
        {
            IPlayer player, enemy;
            if (currentPlayer.Equals(oldLogic.Player1))
            {
                player = oldLogic.Player1;
                enemy = oldLogic.Player2;
            }
            else
            {
                player = oldLogic.Player2;
                enemy = oldLogic.Player1;
            }
            return gameLogic[player, CellsCount] - gameLogic[enemy, CellsCount];
        }
    }
}
