// Part1();
Part2();

void Part1()
{
    var parsedInput = ParseInput();

    var stepCount = 0;
    var currentNode = parsedInput.NodeList["AAA"];
    while (currentNode.Id != "ZZZ")
    {
        var instruction = parsedInput.InstructionSet[stepCount % parsedInput.InstructionSet.Length];
        if (instruction == 'L')
        {
            currentNode = parsedInput.NodeList[currentNode.Left];
        }
        else if (instruction == 'R')
        {
            currentNode = parsedInput.NodeList[currentNode.Right];
        }
        stepCount++;
    }

    Console.WriteLine(stepCount);
}

void Part2()
{
    var parsedInput = ParseInput();
    var startNodes = parsedInput.NodeList.Where(nl => nl.Key.EndsWith("A")).Select(nl => nl.Value);

    var stepCounts = new List<int>();

    long? lcm = null;

    foreach (var startNode in startNodes)
    {
        var stepCount = 0;
        var currentNode = parsedInput.NodeList[startNode.Id];
        while (!currentNode.Id.EndsWith("Z"))
        {
            var instruction = parsedInput.InstructionSet[
                stepCount % parsedInput.InstructionSet.Length
            ];
            if (instruction == 'L')
            {
                currentNode = parsedInput.NodeList[currentNode.Left];
            }
            else if (instruction == 'R')
            {
                currentNode = parsedInput.NodeList[currentNode.Right];
            }
            stepCount++;
        }

        if (lcm.HasValue)
        {
            lcm = CalculateLCM(lcm.Value, stepCount);
        }
        else
        {
            lcm = stepCount;
        }
    }

    Console.WriteLine(lcm);
}

long CalculateGCD(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

long CalculateLCM(long a, long b)
{
    return a / CalculateGCD(a, b) * b;
}

(Dictionary<string, Node> NodeList, string InstructionSet) ParseInput()
{
    var inputLines = File.ReadAllLines("input.txt");
    var instructions = inputLines[0].Trim();

    var nodeList = new Dictionary<string, Node>();

    foreach (var nodeLine in inputLines.Skip(2))
    {
        var nodeId = nodeLine.Substring(0, nodeLine.IndexOf(' '));

        var left = nodeLine.Substring(nodeLine.IndexOf('(') + 1, 3);
        var right = nodeLine.Substring(nodeLine.IndexOf(',') + 2, 3);

        nodeList.Add(
            nodeId,
            new Node
            {
                Id = nodeId,
                Left = left,
                Right = right
            }
        );
    }

    return (nodeList, instructions);
}

class Node
{
    public string Id { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }
}
