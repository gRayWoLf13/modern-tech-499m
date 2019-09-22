namespace modern_tech_499m.Logic
{
    interface IPlayer
    {
        IPlayer Enemy { get; set; }
        string Name { get; set; }
        MoveResult MakeMove(int cellIndex);
    }
}
