Part1();
Part2();

void Part1()
{
    var gridLines = File.ReadAllLines("input.txt");

    var creatureCoords = FindCreatureLocation(gridLines);

    var circuit = GetPipeCircuit(gridLines, creatureCoords);

    Console.WriteLine(circuit.Count / 2);
}

void Part2()
{
    var gridLines = File.ReadAllLines("input.txt");

    var creatureCoords = FindCreatureLocation(gridLines);

    var circuit = GetPipeCircuit(gridLines, creatureCoords);

    var insideCount = 0;

    for (var row = 0; row < gridLines.Length; row++)
    {
        var line = gridLines[row];
        for (var col = 0; col < line.Length; col++)
        {
            var tile = line[col];

            if (circuit.ContainsKey((row, col)))
            {
                continue;
            }

            var intersectionCount = 0;
            char? lastVertex = null;

            foreach (
                var circuitTileIntersection in circuit
                    .Where(kvPair => kvPair.Key.row == row && kvPair.Key.col > col)
                    .OrderBy(kv => kv.Key.col)
            )
            {
                if (
                    circuitTileIntersection.Value.Type == 'F'
                    || circuitTileIntersection.Value.Type == 'L'
                )
                {
                    lastVertex = circuitTileIntersection.Value.Type;
                }
                else if (circuitTileIntersection.Value.Type == 'J' && lastVertex == 'F')
                {
                    intersectionCount++;
                }
                else if (circuitTileIntersection.Value.Type == '7' && lastVertex == 'L')
                {
                    intersectionCount++;
                }
                else if (circuitTileIntersection.Value.Type == '|')
                {
                    intersectionCount++;
                }
            }

            if (intersectionCount % 2 == 1)
            {
                insideCount++;
            }
        }
    }

    Console.WriteLine(insideCount);
}

(int row, int col) FindCreatureLocation(string[] gridLines)
{
    (int row, int col) creatureCoords = (-1, -1);

    for (var i = 0; i < gridLines.Length; i++)
    {
        var line = gridLines[i];
        for (var j = 0; j < line.Length; j++)
        {
            if (line[j] == 'S')
            {
                creatureCoords = (i, j);
                break;
            }
        }
    }
    return creatureCoords;
}

Dictionary<(int row, int col), Pipe> GetPipeCircuit(
    string[] gridLines,
    (int row, int col) creatureLocation
)
{
    var pipeCircuit = new Dictionary<(int row, int col), Pipe>();

    var currentPipe = GetFirstPipe(gridLines, creatureLocation);
    var previousCoords = creatureLocation;

    pipeCircuit.Add(currentPipe.Location, currentPipe);

    var found = false;

    while (!found)
    {
        var nextPipe = GetNextPipe(currentPipe, previousCoords, gridLines);
        previousCoords = currentPipe.Location;
        currentPipe = nextPipe;

        if (currentPipe.Type == 'S')
        {
            found = true;

            var firstPipe = pipeCircuit.First().Value;
            var previousPipe = pipeCircuit.Last().Value;

            currentPipe.Type = DeterminePipeType(previousPipe, firstPipe);
        }

        pipeCircuit.Add(currentPipe.Location, currentPipe);
    }

    return pipeCircuit;
}

char DeterminePipeType(Pipe previousPipe, Pipe nextPipe)
{
    if (previousPipe.Location.row == nextPipe.Location.row)
    {
        return '-';
    }

    if (previousPipe.Location.col == nextPipe.Location.col)
    {
        return '|';
    }

    if (previousPipe.Location.row < nextPipe.Location.row)
    {
        return previousPipe.Location.col < nextPipe.Location.col ? 'L' : 'J';
    }

    return previousPipe.Location.col < nextPipe.Location.col ? 'F' : '7';
}

Pipe GetFirstPipe(string[] gridLines, (int row, int col) startingLocation)
{
    var northTile = gridLines[startingLocation.row - 1][startingLocation.col];

    if (northTile == '|' || northTile == 'F' || northTile == '7')
    {
        return new Pipe
        {
            Type = northTile,
            Location = (startingLocation.row - 1, startingLocation.col)
        };
    }

    var eastTile = gridLines[startingLocation.row][startingLocation.col + 1];

    if (eastTile == '-' || eastTile == 'J' || eastTile == '7')
    {
        return new Pipe
        {
            Type = eastTile,
            Location = (startingLocation.row, startingLocation.col + 1)
        };
    }

    return new Pipe
    {
        Type = gridLines[startingLocation.row + 1][startingLocation.col],
        Location = (startingLocation.row + 1, startingLocation.col)
    };
}

Pipe GetNextPipe(Pipe currentPipe, (int row, int col) previousLocation, string[] gridLines)
{
    var newRow = currentPipe.Location.row;
    var newCol = currentPipe.Location.col;

    if (currentPipe.Type == '|')
    {
        newRow =
            previousLocation.row > currentPipe.Location.row
                ? currentPipe.Location.row - 1
                : currentPipe.Location.row + 1;
    }
    else if (currentPipe.Type == '-')
    {
        newCol =
            previousLocation.col > currentPipe.Location.col
                ? currentPipe.Location.col - 1
                : currentPipe.Location.col + 1;
    }
    else if (currentPipe.Type == 'L')
    {
        newRow =
            previousLocation.col != currentPipe.Location.col
                ? currentPipe.Location.row - 1
                : currentPipe.Location.row;
        newCol =
            previousLocation.row != currentPipe.Location.row
                ? currentPipe.Location.col + 1
                : currentPipe.Location.col;
    }
    else if (currentPipe.Type == 'J')
    {
        newRow =
            previousLocation.col != currentPipe.Location.col
                ? currentPipe.Location.row - 1
                : currentPipe.Location.row;
        newCol =
            previousLocation.row != currentPipe.Location.row
                ? currentPipe.Location.col - 1
                : currentPipe.Location.col;
    }
    else if (currentPipe.Type == '7')
    {
        newRow =
            previousLocation.col != currentPipe.Location.col
                ? currentPipe.Location.row + 1
                : currentPipe.Location.row;
        newCol =
            previousLocation.row != currentPipe.Location.row
                ? currentPipe.Location.col - 1
                : currentPipe.Location.col;
    }
    else if (currentPipe.Type == 'F')
    {
        newRow =
            previousLocation.col != currentPipe.Location.col
                ? currentPipe.Location.row + 1
                : currentPipe.Location.row;
        newCol =
            previousLocation.row != currentPipe.Location.row
                ? currentPipe.Location.col + 1
                : currentPipe.Location.col;
    }

    var newTile = gridLines[newRow][newCol];
    return new Pipe { Type = newTile, Location = (newRow, newCol) };
}

public class Pipe
{
    public char Type { get; set; }
    public (int row, int col) Location { get; set; }
}
