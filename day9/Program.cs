Part1();
Part2();

void Part1()
{
    var dataset = ParseInput();

    var sum = 0;

    foreach (var dataLine in dataset)
    {
        var sequences = BuildDifferences(dataLine);

        for (var i = sequences.Count - 1; i >= 0; i--)
        {
            var sequence = sequences[i];
            if (i == sequences.Count - 1)
            {
                sequence.Add(0);
                continue;
            }

            var newValue = sequences[i + 1].Last() + sequence.Last();
            sequence.Add(newValue);
        }

        sum += sequences[0].Last();
    }

    Console.WriteLine(sum);
}

void Part2()
{
    var dataset = ParseInput();

    var sum = 0;

    foreach (var dataLine in dataset)
    {
        var sequences = BuildDifferences(dataLine);

        for (var i = sequences.Count - 1; i >= 0; i--)
        {
            var sequence = sequences[i];
            if (i == sequences.Count - 1)
            {
                sequence.Insert(0, 0);
                continue;
            }

            var newValue = sequence.First() - sequences[i + 1].First();
            sequence.Insert(0, newValue);
        }

        sum += sequences[0].First();
    }

    Console.WriteLine(sum);
}

List<List<int>> BuildDifferences(List<int> sequence)
{
    var sequences = new List<List<int>>() { sequence };

    var latestSequence = sequences.Last();

    while (latestSequence.Any(d => d != 0))
    {
        var newSequence = new List<int>();

        for (var i = 0; i < latestSequence.Count - 1; i++)
        {
            newSequence.Add(latestSequence[i + 1] - latestSequence[i]);
        }
        sequences.Add(newSequence);
        latestSequence = sequences.Last();
    }

    return sequences;
}

List<List<int>> ParseInput()
{
    var inputLines = File.ReadAllLines("input.txt");

    return inputLines.Select(l => l.Split(' ').Select(int.Parse).ToList()).ToList();
}
