using System.Net.Http.Headers;

Part1();
Part2();

void Part1()
{
    var patterns = ParsePatterns();

    var result = 0;

    foreach (var pattern in patterns)
    {
        result += ProcessPattern(pattern);
    }

    Console.WriteLine(result);
}

void Part2()
{
    var patterns = ParsePatterns();

    var result = 0;

    foreach (var pattern in patterns)
    {
        var originalReflectionScore = ProcessPattern(pattern);
        var patternResult = 0;
        var nextToFlip = (0, 0);

        while (patternResult == 0 || patternResult == originalReflectionScore)
        {
            var newPattern = new List<string>();
            for (var i = 0; i < pattern.Count; i++)
            {
                var patternRow = pattern[i];
                var newPatternRow = "";
                for (var j = 0; j < patternRow.Length; j++)
                {
                    var tile = patternRow[j];
                    var flipTile = i == nextToFlip.Item1 && j == nextToFlip.Item2;
                    var flippedTile = tile == '.' ? '#' : '.';
                    newPatternRow += flipTile ? flippedTile : tile;
                }
                newPattern.Add(newPatternRow);
            }

            patternResult = ProcessPattern(newPattern, originalReflectionScore);
            // if (nextToFlip.Item1 == 1 && nextToFlip.Item2 == 3)
            // {
            //     Console.WriteLine($"Score of {patternResult} for");
            //     Console.WriteLine(string.Join("\r\n", newPattern));
            // }

            nextToFlip =
                nextToFlip.Item2 == pattern[0].Length
                    ? (nextToFlip.Item1 + 1, 0)
                    : (nextToFlip.Item1, nextToFlip.Item2 + 1);

            if (nextToFlip.Item1 == pattern.Count)
            {
                Console.WriteLine("Unable to find mirror for pattern");
                Console.WriteLine($"Original score: {originalReflectionScore}");
                Console.WriteLine(string.Join("\r\n", pattern));
            }
        }

        Console.WriteLine(patternResult);

        result += patternResult;
    }

    Console.WriteLine(result);
}

int ProcessPattern(List<string> pattern, int scoreToIgnore = 0)
{
    var horizontalResult =
        FindNumberOfRowsAboveMirror(pattern, scoreToIgnore >= 100 ? scoreToIgnore / 100 : 0) * 100;
    if (horizontalResult > 0 && horizontalResult != scoreToIgnore)
        return horizontalResult;

    var rotatedPattern = RotatePattern(pattern);

    var verticalResult = FindNumberOfRowsAboveMirror(
        rotatedPattern,
        scoreToIgnore < 100 ? scoreToIgnore : 0
    );

    return verticalResult;
}

static int FindNumberOfRowsAboveMirror(List<string> pattern, int mirrorLineToIgnore = 0)
{
    var numRowsAboveMirror = 0;
    var matchCount = 0;
    for (var i = 0; i < pattern.Count; i++)
    {
        if (i == 0)
        {
            numRowsAboveMirror++;
            continue;
        }

        var row = pattern[i];
        var comparisonRowIndex = i - (1 + (matchCount * 2));

        if (comparisonRowIndex < 0)
        {
            break;
        }

        var comparisonRow = pattern[comparisonRowIndex];

        // Console.WriteLine($"Row: {row} - Comparison: {comparisonRow}");

        if (row == comparisonRow && numRowsAboveMirror != mirrorLineToIgnore)
        {
            matchCount++;
        }
        else
        {
            // Reset the count
            matchCount = 0;
            numRowsAboveMirror = i + 1;
        }
    }

    return numRowsAboveMirror == pattern.Count ? 0 : numRowsAboveMirror;
}

static List<string> RotatePattern(List<string> pattern)
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

static List<List<string>> ParsePatterns()
{
    var inputLines = File.ReadAllLines("input.txt");

    var patterns = new List<List<string>>();
    var buildingPattern = new List<string>();
    foreach (var line in inputLines)
    {
        if (line == "")
        {
            patterns.Add(buildingPattern);
            buildingPattern = new List<string>();
        }
        else
        {
            buildingPattern.Add(line);
        }
    }
    patterns.Add(buildingPattern);

    return patterns;
}
