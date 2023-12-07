var hands = ParseHands();

Part2();

void Part1()
{
    var orderedHands = hands.OrderBy(h => h, new HandComparerPart1()).ToArray();

    var winnings = 0;

    for (var i = 0; i < orderedHands.Length; i++)
    {
        winnings += orderedHands[i].Bid * (i + 1);
    }

    Console.WriteLine(winnings);
}

void Part2()
{
    var orderedHands = hands.OrderBy(h => h, new HandComparerPart2()).ToArray();

    var winnings = 0;

    for (var i = 0; i < orderedHands.Length; i++)
    {
        winnings += orderedHands[i].Bid * (i + 1);
    }

    Console.WriteLine(winnings);
}

List<Hand> ParseHands()
{
    var inputLines = File.ReadAllLines("input.txt");

    var allHands = new List<Hand>();
    foreach (var line in inputLines)
    {
        var segments = line.Split(' ');
        allHands.Add(new Hand { Cards = segments[0], Bid = int.Parse(segments[1]) });
    }

    return allHands;
}

enum HandType
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeKind,
    FullHouse,
    FourKind,
    FiveKind
}

class Hand
{
    public string Cards { get; set; }
    public int Bid { get; set; }
}
