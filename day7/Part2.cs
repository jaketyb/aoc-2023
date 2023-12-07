class HandComparerPart2 : IComparer<Hand>
{
    private readonly IComparer<char> _cardComparer;

    public HandComparerPart2()
    {
        _cardComparer = new CardComparerPart2();
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

    private HandType GetHandType(Hand hand)
    {
        var cardToCountMap = new Dictionary<char, int>();
        var wildcardCount = 0;
        foreach (var card in hand.Cards)
        {
            if (card == 'J')
            {
                wildcardCount++;
                continue;
            }

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

        var highestCount =
            orderedMap.Count() == 0 ? wildcardCount : orderedMap.ElementAt(0).Value + wildcardCount;

        if (highestCount == 5)
        {
            return HandType.FiveKind;
        }
        if (highestCount == 4)
        {
            return HandType.FourKind;
        }

        var nextHighestCount = orderedMap.ElementAt(1);

        if (highestCount == 3)
        {
            if (nextHighestCount.Value == 2)
            {
                return HandType.FullHouse;
            }
            return HandType.ThreeKind;
        }

        if (highestCount == 2)
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

class CardComparerPart2 : IComparer<char>
{
    private static List<char> CardRankings = new List<char>
    {
        'J',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        'T',
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
