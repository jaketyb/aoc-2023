Part1();
Part2();

void Part1()
{
    var gridLines = File.ReadAllLines("input.txt");

    var result = CountEnergizedTiles(
        gridLines,
        new Dictionary<int, HashSet<Direction>>(),
        Direction.Right,
        (0, 0)
    );

    Console.WriteLine(result.Count);
}

void Part2()
{
    var gridLines = File.ReadAllLines("input.txt");

    var largestCount = 0;

    for (var row = 0; row < gridLines.Length; row++)
    {
        var count = CountEnergizedTiles(
            gridLines,
            new Dictionary<int, HashSet<Direction>>(),
            Direction.Right,
            (row, 0)
        ).Count;

        if (largestCount < count)
        {
            largestCount = count;
        }

        count = CountEnergizedTiles(
            gridLines,
            new Dictionary<int, HashSet<Direction>>(),
            Direction.Left,
            (row, gridLines[0].Length - 1)
        ).Count;

        if (largestCount < count)
        {
            largestCount = count;
        }
    }

    for (var col = 0; col < gridLines[0].Length; col++)
    {
        var count = CountEnergizedTiles(
            gridLines,
            new Dictionary<int, HashSet<Direction>>(),
            Direction.Down,
            (0, col)
        ).Count;

        if (largestCount < count)
        {
            largestCount = count;
        }

        count = CountEnergizedTiles(
            gridLines,
            new Dictionary<int, HashSet<Direction>>(),
            Direction.Up,
            (gridLines.Length - 1, col)
        ).Count;

        if (largestCount < count)
        {
            largestCount = count;
        }
    }

    Console.WriteLine(largestCount);
}

Dictionary<int, HashSet<Direction>> CountEnergizedTiles(
    string[] grid,
    Dictionary<int, HashSet<Direction>> energizedTileIds,
    Direction travelDirection,
    (int row, int col) currentBeamLocation
)
{
    while (
        currentBeamLocation.row < grid.Length
        && currentBeamLocation.col < grid[0].Length
        && currentBeamLocation.row > -1
        && currentBeamLocation.col > -1
    )
    {
        var tileId = currentBeamLocation.col + (currentBeamLocation.row * grid[0].Length);
        if (energizedTileIds.ContainsKey(tileId))
        {
            if (energizedTileIds[tileId].Contains(travelDirection))
            {
                return energizedTileIds;
            }
            energizedTileIds[tileId].Add(travelDirection);
        }
        else
        {
            energizedTileIds.Add(tileId, new HashSet<Direction>());
        }

        var tile = grid[currentBeamLocation.row][currentBeamLocation.col];

        if (
            tile == '.'
            || (tile == '-' && new[] { Direction.Left, Direction.Right }.Contains(travelDirection))
            || (tile == '|' && new[] { Direction.Down, Direction.Up }.Contains(travelDirection))
        )
        {
            currentBeamLocation = GetNewLocation(currentBeamLocation, travelDirection);
        }
        else if (
            (tile == '/' && new[] { Direction.Down, Direction.Up }.Contains(travelDirection))
            || (tile == '\\' && new[] { Direction.Left, Direction.Right }.Contains(travelDirection))
        )
        {
            travelDirection = GetNewDirectionClockwise(travelDirection);
            currentBeamLocation = GetNewLocation(currentBeamLocation, travelDirection);
        }
        else if (
            (tile == '/' && new[] { Direction.Left, Direction.Right }.Contains(travelDirection))
            || (tile == '\\' && new[] { Direction.Down, Direction.Up }.Contains(travelDirection))
        )
        {
            travelDirection = GetNewDirectionAntiClockwise(travelDirection);
            currentBeamLocation = GetNewLocation(currentBeamLocation, travelDirection);
        }
        else if (
            (tile == '-' && new[] { Direction.Up, Direction.Down }.Contains(travelDirection))
            || (tile == '|' && new[] { Direction.Left, Direction.Right }.Contains(travelDirection))
        )
        {
            var beamTwoDirection = GetNewDirectionClockwise(travelDirection);
            var otherBeamTiles = CountEnergizedTiles(
                grid,
                energizedTileIds,
                beamTwoDirection,
                GetNewLocation(currentBeamLocation, beamTwoDirection)
            );
            energizedTileIds.Union(otherBeamTiles);

            travelDirection = GetNewDirectionAntiClockwise(travelDirection);
            currentBeamLocation = GetNewLocation(currentBeamLocation, travelDirection);
        }
    }

    return energizedTileIds;
}

Direction GetNewDirectionClockwise(Direction currentDirection)
{
    return (Direction)((int)(currentDirection + 1) % 4);
}

Direction GetNewDirectionAntiClockwise(Direction currentDirection)
{
    return currentDirection == Direction.Right
        ? Direction.Up
        : (Direction)((int)currentDirection - 1);
}

(int row, int col) GetNewLocation((int row, int col) currentLocation, Direction travelDirection)
{
    if (travelDirection == Direction.Right)
        return (currentLocation.row, currentLocation.col + 1);

    if (travelDirection == Direction.Down)
        return (currentLocation.row + 1, currentLocation.col);

    if (travelDirection == Direction.Left)
        return (currentLocation.row, currentLocation.col - 1);

    return (currentLocation.row - 1, currentLocation.col);
}

enum Direction
{
    Right,
    Down,
    Left,
    Up
}
