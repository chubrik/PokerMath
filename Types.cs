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

    public Card Pop() => _cards[_index++];
}

internal readonly struct Card
{
    public Value Value { get; }
    public Suit Suit { get; }

    public Card(Value value, Suit suit)
    {
        Value = value;
        Suit = suit;
    }

    public override string ToString()
    {
        var value = Value switch
        {
            Value._2 => "2",
            Value._3 => "3",
            Value._4 => "4",
            Value._5 => "5",
            Value._6 => "6",
            Value._7 => "7",
            Value._8 => "8",
            Value._9 => "9",
            Value._T => "T",
            Value._J => "J",
            Value._Q => "Q",
            Value._K => "K",
            Value._A => "A",
            _ => throw new InvalidOperationException()
        };

        var suit = Suit switch
        {
            Suit.Spades => "♠",
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            _ => throw new InvalidOperationException()
        };

        return value + suit;
    }
}

internal enum Value : byte
{
    _2 = 2,
    _3,
    _4,
    _5,
    _6,
    _7,
    _8,
    _9,
    _T,
    _J,
    _Q,
    _K,
    _A,
}

internal enum Suit : byte
{
    Spades,   // Пики
    Hearts,   // Черви
    Diamonds, // Бубны
    Clubs     // Трефы 
}
