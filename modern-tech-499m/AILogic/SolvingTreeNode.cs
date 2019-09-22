using System.Collections.Generic;
using modern_tech_499m.Logic;

namespace modern_tech_499m.AILogic
{
    class SolvingTreeNode
    {
        public GameLogic CurrentGameState;
        public int ParentCellNumber;
        public IPlayer GameMoveOwner;
        public SolvingTreeNode ParentNode;
        public LinkedList<SolvingTreeNode> ChildrenNodes;
        public int? GameStateWeight;
        public int? PreviewGameStateWeight;
        public int TreeNodeDepth;
    }
}
