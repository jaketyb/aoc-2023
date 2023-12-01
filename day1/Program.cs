var numberStrings = new List<NumberString>()
{
    new NumberString("one", 1),
    new NumberString("two", 2),
    new NumberString("three", 3),
    new NumberString("four", 4),
    new NumberString("five", 5),
    new NumberString("six", 6),
    new NumberString("seven", 7),
    new NumberString("eight", 8),
    new NumberString("nine", 9),
    new NumberString("1", 1),
    new NumberString("2", 2),
    new NumberString("3", 3),
    new NumberString("4", 4),
    new NumberString("5", 5),
    new NumberString("6", 6),
    new NumberString("7", 7),
    new NumberString("8", 8),
    new NumberString("9", 9),
};

var instructions = File.ReadAllLines("input.txt");

var digits = new List<string>();
var sum = 0;

foreach (var instruction in instructions)
{
    var matches = new List<InstructionMatch>();
    foreach (var numberString in numberStrings)
    {
        var indexOfNumberString = instruction.IndexOf(numberString.Description);
        if (indexOfNumberString > -1)
        {
            matches.Add(new InstructionMatch(numberString, indexOfNumberString));
        }

        var lastIndexOfNumberString = instruction.LastIndexOf(numberString.Description);
        if (lastIndexOfNumberString > -1)
        {
            matches.Add(new InstructionMatch(numberString, lastIndexOfNumberString));
        }
    }

    var firstMatch = matches.OrderBy(m => m.Index).First();
    var lastMatch = matches.OrderByDescending(m => m.Index).First();

    sum += int.Parse(firstMatch.Match.Value.ToString() + lastMatch.Match.Value.ToString());
}
Console.WriteLine(sum);
