Part2();
void Part1()
{
    var cards = ParseInput();

    double sum = 0;

    foreach (var card in cards)
    {
        var matches = card.WinningNumbers.Sum(wn => card.ActualNumbers.Contains(wn) ? 1 : 0);

        sum += matches == 0 ? 0 : Math.Pow(2, matches - 1);
    }

    Console.WriteLine(sum);
}

void Part2()
{
    var cards = ParseInput();

    var cardCounts = cards.ToDictionary(c => c.Id, c => 1);

    for (var i = 0; i < cards.Count; i++)
    {
        var card = cards[i];

        var matches = card.WinningNumbers.Sum(wn => card.ActualNumbers.Contains(wn) ? 1 : 0);

        for (var j = 0; j < matches; j++)
        {
            cardCounts[i + j + 2] += cardCounts[i + 1];
        }
    }

    var totalCards = cardCounts.Values.Sum();

    Console.WriteLine(totalCards);
}

List<Card> ParseInput()
{
    var inputLines = File.ReadAllLines("input.txt");

    var cards = new List<Card>();
    foreach (var line in inputLines)
    {
        var segments = line.Split(':');
        var cardName = segments[0];
        segments = segments[1].Split('|');
        var winningNumbers = segments[0]
            .Trim()
            .Split(' ')
            .Where(s => s != "")
            .Select(s => int.Parse(s));
        var actualNumbers = segments[1]
            .Trim()
            .Split(' ')
            .Where(s => s != "")
            .Select(s => int.Parse(s));

        cards.Add(
            new Card
            {
                Id = int.Parse(cardName.Split(' ').Last()),
                WinningNumbers = winningNumbers.ToHashSet(),
                ActualNumbers = actualNumbers.ToHashSet()
            }
        );
    }

    return cards;
}

class Card
{
    public int Id { get; set; }
    public HashSet<int> WinningNumbers { get; set; } = new HashSet<int>();
    public HashSet<int> ActualNumbers { get; set; } = new HashSet<int>();
}
