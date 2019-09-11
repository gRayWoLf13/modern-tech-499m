namespace modern_tech_499m.Logic
{
    class Cell
    {
        public IPlayer Owner { get; set; }
        public int Value { get; set; }
        public bool IsEndingCell { get; set; }
        public int Number { get; set; }
    }
}