var games = ParseGames();

Part1();
Part2();

void Part1()
{
    var queryHandful = new Handful
    {
        Red = 12,
        Green = 13,
        Blue = 14
    };

    var possibleGameIdTotal = 0;

    for (var i = 0; i < games.Count; i++)
    {
        var game = games[i];

        var impossible = game.Handfuls.Any(
            h =>
                h.Green > queryHandful.Green
                || h.Red > queryHandful.Red
                || h.Blue > queryHandful.Blue
        );

        if (!impossible)
        {
            possibleGameIdTotal += (i + 1);
        }
    }

    Console.WriteLine(possibleGameIdTotal);
}

void Part2()
{
    var sum = 0;
    foreach (var game in games)
    {
        var minValuesHandful = new Handful();

        foreach (var handful in game.Handfuls)
        {
            if (handful.Red > minValuesHandful.Red)
            {
                minValuesHandful.Red = handful.Red;
            }
            if (handful.Blue > minValuesHandful.Blue)
            {
                minValuesHandful.Blue = handful.Blue;
            }
            if (handful.Green > minValuesHandful.Green)
            {
                minValuesHandful.Green = handful.Green;
            }
        }

        var power = minValuesHandful.Red * minValuesHandful.Blue * minValuesHandful.Green;
        sum += power;
    }

    Console.WriteLine(sum);
}

List<Game> ParseGames()
{
    var inputLines = File.ReadAllLines("input.txt");

    var games = new List<Game>();
    foreach (var line in inputLines)
    {
        var handfuls = new List<Handful>();

        var lineTrimmed = line.Substring(line.IndexOf(':') + 2);

        foreach (var handfulText in lineTrimmed.Split(';'))
        {
            var handful = new Handful();

            foreach (var cubeCount in handfulText.Split(','))
            {
                var countAndColour = cubeCount.Trim().Split(' ');

                if (countAndColour[1] == "blue")
                {
                    handful.Blue = int.Parse(countAndColour[0]);
                }
                else if (countAndColour[1] == "red")
                {
                    handful.Red = int.Parse(countAndColour[0]);
                }
                else if (countAndColour[1] == "green")
                {
                    handful.Green = int.Parse(countAndColour[0]);
                }
            }

            handfuls.Add(handful);
        }

        games.Add(new Game { Handfuls = handfuls });
    }

    return games;
}

class Game
{
    public List<Handful> Handfuls { get; set; }
}

class Handful
{
    public int Blue { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
}
