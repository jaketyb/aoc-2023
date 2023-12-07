class HandComparerPart1 : IComparer<Hand>
{
    private readonly IComparer<char> _cardComparer;

    public HandComparerPart1()
    {
        _cardComparer = new CardComparerPart1();
    }

    public int Compare(Hand? x, Hand? y)
    {
        if (x == null || y == null)
            return 0;

        var xHandType = GetHandType(x);
        var yHandType = GetHandType(y);

        if (xHandType != yHandType)
            return xHandType - yHandType;

        for (var i = 0; i < 5; i++)
        {
            var xCard = x.Cards[i];
            var yCard = y.Cards[i];

            if (xCard != yCard)
                return _cardComparer.Compare(xCard, yCard);
        }

        return 0;
    }

    HandType GetHandType(Hand hand)
    {
        var cardToCountMap = new Dictionary<char, int>();
        foreach (var card in hand.Cards)
        {
            if (cardToCountMap.ContainsKey(card))
            {
                cardToCountMap[card]++;
            }
            else
            {
                cardToCountMap.Add(card, 1);
            }
        }

        var orderedMap = cardToCountMap.OrderByDescending(ccm => ccm.Value);

        var highestCount = orderedMap.ElementAt(0);

        if (highestCount.Value == 5)
        {
            return HandType.FiveKind;
        }
        if (highestCount.Value == 4)
        {
            return HandType.FourKind;
        }

        var nextHighestCount = orderedMap.ElementAt(1);

        if (highestCount.Value == 3)
        {
            if (nextHighestCount.Value == 2)
            {
                return HandType.FullHouse;
            }
            return HandType.ThreeKind;
        }

        if (highestCount.Value == 2)
        {
            if (nextHighestCount.Value == 2)
            {
                return HandType.TwoPair;
            }

            return HandType.OnePair;
        }

        return HandType.HighCard;
    }
}

class CardComparerPart1 : IComparer<char>
{
    static List<char> CardRankings = new List<char>
    {
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        'T',
        'J',
        'Q',
        'K',
        'A'
    };

    public int Compare(char x, char y)
    {
        var xRanking = CardRankings.IndexOf(x);
        var yRanking = CardRankings.IndexOf(y);
        return xRanking - yRanking;
    }
}
