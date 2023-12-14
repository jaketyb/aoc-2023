Part1();
Part2();

void Part1()
{
    var platform = File.ReadAllLines("input.txt");

    var rearrangedPlatform = TiltPlatformAndRearrange(platform.ToList());

    var sum = rearrangedPlatform
        .Select(
            (row, i) =>
                row.Select((tile, j) => tile == 'O' ? rearrangedPlatform.Count - i : 0).Sum()
        )
        .Sum();

    Console.WriteLine(sum);
}

void Part2()
{
    var platform = File.ReadAllLines("input.txt");

    var cycleCount = 0;
    var rearrangedPlatform = platform.Select(p => p).ToList();
    var previousPlatforms = new List<string>();
    while (cycleCount < 1000000000)
    {
        for (var rotations = 0; rotations < 4; rotations++)
        {
            rearrangedPlatform = TiltPlatformAndRearrange(rearrangedPlatform);
            rearrangedPlatform = RotatePattern(rearrangedPlatform);
        }

        cycleCount++;

        var encoded = string.Join(';', rearrangedPlatform);

        var indexOf = previousPlatforms.IndexOf(encoded);

        if (indexOf > -1)
        {
            var repeatingPattern = previousPlatforms.Skip(indexOf).ToList();
            var remainingCycles = 1000000000 - cycleCount;

            var remainder = remainingCycles % repeatingPattern.Count;

            var indexOfLast = indexOf + remainder;
            rearrangedPlatform = previousPlatforms[indexOfLast].Split(';').ToList();
            cycleCount = 1000000000;
        }
        else
        {
            previousPlatforms.Add(encoded);
        }
    }

    var sum = rearrangedPlatform
        .Select(
            (row, i) =>
                row.Select((tile, j) => tile == 'O' ? rearrangedPlatform.Count - i : 0).Sum()
        )
        .Sum();

    Console.WriteLine(sum);
}

List<string> TiltPlatformAndRearrange(List<string> platform)
{
    var rearrangedPlatform = new List<string>();

    for (var row = 0; row < platform.Count; row++)
    {
        rearrangedPlatform.Add(platform[row]);
        for (var col = 0; col < rearrangedPlatform[row].Length; col++)
        {
            var tile = rearrangedPlatform[row][col];
            if (tile == 'O')
            {
                var newPos = FindNewPositionForRock((row, col), rearrangedPlatform);
                rearrangedPlatform[row] = string.Concat(
                    rearrangedPlatform[row].Select((t, i) => i == col ? '.' : t)
                );
                rearrangedPlatform[newPos.row] = string.Concat(
                    rearrangedPlatform[newPos.row].Select((t, i) => i == newPos.col ? 'O' : t)
                );
            }
        }
    }

    return rearrangedPlatform;
}

static (int row, int col) FindNewPositionForRock(
    (int row, int col) rockPosition,
    List<string> platform
)
{
    var currentRockPosition = (rockPosition.row, rockPosition.col);
    char? tileAboveRock =
        rockPosition.row == 0 ? null : platform[currentRockPosition.row - 1][rockPosition.col];

    while (tileAboveRock == '.')
    {
        currentRockPosition = (currentRockPosition.row - 1, currentRockPosition.col);

        tileAboveRock =
            currentRockPosition.row == 0
                ? null
                : platform[currentRockPosition.row - 1][currentRockPosition.col];
    }

    return currentRockPosition;
}

List<string> RotatePattern(List<string> pattern)
{
    var newPattern = new List<string>();

    for (var col = 0; col < pattern[0].Length; col++)
    {
        var newRow = "";
        for (var row = pattern.Count - 1; row >= 0; row--)
        {
            newRow += pattern[row][col];
        }
        newPattern.Add(newRow);
    }

    return newPattern;
}
