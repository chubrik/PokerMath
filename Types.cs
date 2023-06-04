namespace PokerMath;

using static PokerMath.Constants;

internal sealed class Deck
{
    private readonly Card[] _cards;
    private int _index = 0;

    public Deck()
    {
        var cards = AllCards.ToArray();
        cards.Shuffle();
        _cards = cards;
    }

    public Card Pop() => _cards[_index++];
}

internal sealed class Card
{
    public Value Value { get; }
    public Suit Suit { get; }
    public ulong Mask { get; }

    public Card(Value value, Suit suit)
    {
        Value = value;
        Suit = suit;
        Mask = _valueToMask[(int)value] | _suitToMask[(int)suit];
    }

    public int Index => (int)Suit * 13 + (int)Value;

    public override string ToString()
    {
        var value = Value switch
        {
            Value._A => "A",
            Value._K => "K",
            Value._Q => "Q",
            Value._J => "J",
            Value._T => "T",
            Value._9 => "9",
            Value._8 => "8",
            Value._7 => "7",
            Value._6 => "6",
            Value._5 => "5",
            Value._4 => "4",
            Value._3 => "3",
            Value._2 => "2",
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

    private static readonly ulong[] _valueToMask = new[]
    {
        0UL,
        0UL,
        Utils.Mask_2,
        Utils.Mask_3,
        Utils.Mask_4,
        Utils.Mask_5,
        Utils.Mask_6,
        Utils.Mask_7,
        Utils.Mask_8,
        Utils.Mask_9,
        Utils.Mask_T,
        Utils.Mask_J,
        Utils.Mask_Q,
        Utils.Mask_K,
        Utils.Mask_A,
    };

    private static readonly ulong[] _suitToMask = new[]
    {
        0UL,
        Utils.Mask_Clubs,
        Utils.Mask_Diamonds,
        Utils.Mask_Hearts,
        Utils.Mask_Spades,
    };
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
    _T,
    _J,
    _Q,
    _K,
    _A,
}

internal enum Suit
{
    Spades = 4,   // Пики
    Hearts = 3,   // Черви
    Diamonds = 2, // Бубны
    Clubs = 1,    // Трефы 
}
