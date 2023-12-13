Part1();
Part2();

void Part1()
{
    var imageLines = File.ReadAllLines("input.txt");

    var parseResult = GetGalaxies(imageLines);

    parseResult.Galaxies = RepositionGalaxies(parseResult);

    var pairings = GenerateUniquePairings(parseResult.Galaxies);

    var totalLength = 0;
    foreach (var pairing in pairings)
    {
        var shortestDistance =
            Math.Abs(pairing.Item1.row - pairing.Item2.row)
            + Math.Abs(pairing.Item1.col - pairing.Item2.col);

        totalLength +=
            Math.Abs(pairing.Item1.row - pairing.Item2.row)
            + Math.Abs(pairing.Item1.col - pairing.Item2.col);
    }

    Console.WriteLine(totalLength);
}

void Part2()
{
    var imageLines = File.ReadAllLines("input.txt");

    var parseResult = GetGalaxies(imageLines);

    parseResult.Galaxies = RepositionGalaxies(parseResult, 1000000);

    var pairings = GenerateUniquePairings(parseResult.Galaxies);

    long totalLength = 0;
    foreach (var pairing in pairings)
    {
        var shortestDistance =
            Math.Abs(pairing.Item1.row - pairing.Item2.row)
            + Math.Abs(pairing.Item1.col - pairing.Item2.col);

        totalLength += shortestDistance;
    }

    Console.WriteLine(totalLength);
}

static IEnumerable<Tuple<(int row, int col), (int row, int col)>> GenerateUniquePairings(
    HashSet<(int row, int col)> list
)
{
    return list.SelectMany(
        (item, index) => list.Skip(index + 1).Select(other => Tuple.Create(item, other))
    );
}

HashSet<(int row, int col)> RepositionGalaxies(
    GalaxyParseResult parseResult,
    int expansionMultiplier = 2
)
{
    var newPositions = new HashSet<(int row, int col)>();

    foreach (var galaxy in parseResult.Galaxies)
    {
        var emptyColsBefore = parseResult.EmptyCols.Where(c => c < galaxy.col).Count();
        var emptyRowsBefore = parseResult.EmptyRows.Where(r => r < galaxy.row).Count();

        newPositions.Add(
            (
                galaxy.row + emptyRowsBefore * (expansionMultiplier - 1),
                galaxy.col + emptyColsBefore * (expansionMultiplier - 1)
            )
        );
    }

    return newPositions;
}

GalaxyParseResult GetGalaxies(string[] imageLines)
{
    var galaxies = new HashSet<(int row, int col)>();

    var emptyCols = imageLines[0].Select((x, i) => i).ToList();
    var emptyRows = imageLines.Select((x, i) => i).ToList();

    for (var row = 0; row < imageLines.Length; row++)
    {
        var imageLine = imageLines[row];
        for (var col = 0; col < imageLines.Length; col++)
        {
            if (imageLine[col] == '#')
            {
                emptyCols.Remove(col);
                emptyRows.Remove(row);
                galaxies.Add((row, col));
            }
        }
    }

    return new GalaxyParseResult
    {
        Galaxies = galaxies,
        EmptyCols = emptyCols,
        EmptyRows = emptyRows
    };
}

class GalaxyParseResult
{
    public HashSet<(int row, int col)> Galaxies { get; set; }
    public List<int> EmptyCols { get; set; }
    public List<int> EmptyRows { get; set; }
}
