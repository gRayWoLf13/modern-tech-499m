using modern_tech_499m.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using System.Threading.Tasks;

namespace modern_tech_499m.AILogic
{
    class AISolver
    {
        private static readonly int _calculationDepth;
        private static readonly int _cellsCount;
        private static readonly Random rand = new Random();
        private readonly GameLogic _logic;
        private static readonly bool _useAlphaBetaProcedure;

        static AISolver()
        {
            string cellsCount = ConfigurationManager.AppSettings["CellsCount"];
            if (cellsCount == null || !int.TryParse(cellsCount, out _cellsCount))
                _cellsCount = 6;
            string calculationDepth = ConfigurationManager.AppSettings["CalculationDepth"];
            if (calculationDepth == null || !int.TryParse(calculationDepth, out _calculationDepth))
                _calculationDepth = 4;
            string useAlphaBetaProcedure = ConfigurationManager.AppSettings["UseAlphaBetaProcedure"];
            if (useAlphaBetaProcedure == null || !bool.TryParse(useAlphaBetaProcedure, out _useAlphaBetaProcedure))
                _useAlphaBetaProcedure = false;
        }

        public AISolver(GameLogic logic)
        {
            _logic = logic;
        }

        public int GetCell()
        {
            SolvingTreeNode solvingTreeHead = CreateSolvingTree();
            LinkedListNode<SolvingTreeNode> currentChild = solvingTreeHead.ChildrenNodes.First;
            List<int> solvingVariants = new List<int>();
            while (currentChild != null)
            {
                if (currentChild.Value.GameStateWeight.Value == solvingTreeHead.GameStateWeight.Value)
                    solvingVariants.Add(currentChild.Value.ParentCellNumber);
                currentChild = currentChild.Next;
            }
            foreach (var item in solvingVariants)
                Debug.Write($"{item} ");
            Debug.WriteLine("");
            return solvingVariants[rand.Next(solvingVariants.Count)];
            //return solvingVariants[0];
        }

        private SolvingTreeNode CreateSolvingTree()
        {
            Stack<SolvingTreeNode> treeNodes = new Stack<SolvingTreeNode>();
            SolvingTreeNode head = new SolvingTreeNode()
            {
                CurrentGameState = _logic.Clone() as GameLogic,
                ParentNode = null,
                GameMoveOwner = _logic.CurrentPlayer,
                ChildrenNodes = new LinkedList<SolvingTreeNode>(),
                TreeNodeDepth = 0
            };

            AddChildrenNodes(head);
            LinkedListNode<SolvingTreeNode> currentHeadChild = head.ChildrenNodes.Last;
            while (currentHeadChild != null)
            {
                treeNodes.Push(currentHeadChild.Value);
                currentHeadChild = currentHeadChild.Previous;
            }
            do
            {
                SolvingTreeNode currentNode = treeNodes.Pop();
                if (AddChildrenNodes(currentNode))
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
            if (_useAlphaBetaProcedure)
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

                if (_useAlphaBetaProcedure)
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
            if (treeNode.TreeNodeDepth == _calculationDepth)
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

        private bool AddChildrenNodes(SolvingTreeNode treeNode)
        {
            if (treeNode.TreeNodeDepth == _calculationDepth)
            {
                treeNode.GameStateWeight = GetWeightFunctionValue(_logic.CurrentPlayer, treeNode.CurrentGameState);
                treeNode.PreviewGameStateWeight = treeNode.GameStateWeight;
                return true;
            }
            for(int i = 0; i < _cellsCount; i++)
            {
                GameLogic logicCopy = treeNode.CurrentGameState.Clone() as GameLogic;
                MoveResult moveResult = logicCopy.MakeMove(treeNode.GameMoveOwner, i);
                if (moveResult == MoveResult.ImpossibleMove)
                    continue;
                IPlayer childMoveOwner = _logic.GetOtherPlayer(treeNode.GameMoveOwner);
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
                treeNode.GameStateWeight = GetWeightFunctionValue(_logic.CurrentPlayer, treeNode.CurrentGameState);
                treeNode.PreviewGameStateWeight = treeNode.GameStateWeight;
                return true;
            }
            return false;
        }

        private int GetWeightFunctionValue(IPlayer currentPlayer, GameLogic gameLogic)
        {
            IPlayer player, enemy;
            if (currentPlayer.Equals(_logic.Player1))
            {
                player = _logic.Player1;
                enemy = _logic.Player2;
            }
            else
            {
                player = _logic.Player2;
                enemy = _logic.Player1;
            }
            return gameLogic[player, _cellsCount] - gameLogic[enemy, _cellsCount];
        }
    }
}
