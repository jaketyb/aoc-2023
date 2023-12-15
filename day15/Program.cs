Part1();
Part2();

void Part1()
{
    var sum = File.ReadAllLines("input.txt")[0].Split(',').Select(RunHash).Sum();

    Console.WriteLine(sum);
}

void Part2()
{
    var steps = File.ReadAllLines("input.txt")[0].Split(',');

    var boxes = new Dictionary<int, List<(string label, int focalLength)>>();

    foreach (var step in steps)
    {
        var lensLabel = string.Concat(step.Where(char.IsLetter));
        var box = RunHash(lensLabel);
        var operation = step.First(c => c == '-' || c == '=');
        int.TryParse(step.FirstOrDefault(char.IsNumber).ToString(), out var focalLength);

        if (!boxes.ContainsKey(box))
        {
            boxes.Add(box, new List<(string, int)>());
        }

        if (operation == '-')
        {
            boxes[box] = boxes[box].Where(l => l.label != lensLabel).ToList();
        }
        else
        {
            var lensIndex = boxes[box].FindIndex(l => l.label == lensLabel);
            if (lensIndex == -1)
            {
                boxes[box].Add((lensLabel, focalLength));
            }
            else
            {
                boxes[box][lensIndex] = (lensLabel, focalLength);
            }
        }
    }

    var sum = boxes
        .Select(
            box =>
                (box.Key + 1)
                * box.Value.Select((lens, index) => (index + 1) * lens.focalLength).Sum()
        )
        .Sum();

    Console.WriteLine(sum);
}

int RunHash(string toHash)
{
    return toHash.Aggregate(0, (value, stepChar) => (value + stepChar) * 17 % 256);
}
