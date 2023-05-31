namespace PokerMath;

internal sealed class Deck
{
    private Card[] _cards;
    private int _index = 0;

    public Deck()
    {
        var cards = new Card[52];
        var index = 0;

        foreach (var suit in Enum.GetValues<Suit>())
            foreach (var value in Enum.GetValues<Value>())
                cards[index++] = new Card(value, suit);

        cards.Shuffle();
        _cards = cards;
    }

    public Card Pop()
    {
        var card = _cards[_index++];
        card.Use();
        return card;
    }
}

internal sealed class Card
{
    public Value Value { get; }
    public Suit Suit { get; }
    public bool InUse { get; private set; }

    public Card(Value value, Suit suit)
    {
        Value = value;
        Suit = suit;
    }

    public void Use() => InUse = true;

    public override string ToString()
    {
        var value = Value switch
        {
            Value.A => "A",
            Value._2 => "2",
            Value._3 => "3",
            Value._4 => "4",
            Value._5 => "5",
            Value._6 => "6",
            Value._7 => "7",
            Value._8 => "8",
            Value._9 => "9",
            Value.T => "T",
            Value.J => "J",
            Value.Q => "Q",
            Value.K => "K",
            _ => throw new InvalidOperationException()
        };

        var suit = Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Spades => "♠",
            Suit.Clubs => "♣",
            _ => throw new InvalidOperationException()
        };

        var str = value + suit;

        if (InUse)
            str += " (used)";

        return str;
    }
}

internal enum Value
{
    _2 = 2,
    _3,
    _4,
    _5,
    _6,
    _7,
    _8,
    _9,
    T,
    J,
    Q,
    K,
    A,
}

internal enum Suit
{
    Hearts,   // Черви
    Diamonds, // Бубны
    Spades,   // Пики
    Clubs     // Трефы 
}
