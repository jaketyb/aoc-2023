var map = File.ReadAllLines("input.txt");
Part1(map);

//Assume some kind of djisktra algorithm but with an actual list of visited nodes to check the adjacency constraint
void Part1(string[] map)
{
    var visitedBlocks = new List<Block>();
    var routeStack = GetAdjacentBlocks(0, (0, 0), map);

    while (true)
    {
        var nextBlock = routeStack.OrderBy(b => b.HeatLoss).First();
    }
}

Block PopNextBlock(List<Block> possibleBlocks, List<Block> visitedBlocks)
{
    var nextBlock = possibleBlocks.OrderBy(b => b.HeatLoss).First();

    // if it violates rules, then discard and get next lowest
}

List<Block> GetAdjacentBlocks(int currentHeatLoss, (int row, int col) location, string[] map)
{
    var blocks = new Block?[]
    {
        GetBlock((location.row - 1, location.col), map),
        GetBlock((location.row + 1, location.col), map),
        GetBlock((location.row, location.col - 1), map),
        GetBlock((location.row, location.col + 1), map)
    };

    return blocks.Where(b => b != null).ToList();
}

Block? GetBlock((int row, int col) location, string[] map)
{
    if (
        location.row < 0
        || location.col < 0
        || location.row >= map.Length
        || location.row >= map[0].Length
    )
        return null;

    return new Block { HeatLoss = map[location.row][location.col], Position = location };
}

class Route
{
    public Stack<(int row, int col)> Blocks { get; } = new Stack<(int row, int col)>();

    public int HeatLoss { get; set; }
}

class Block
{
    public int HeatLoss { get; set; }
    public (int row, int col) Position { get; set; }
}
