﻿namespace PokerMath;

internal sealed class Deck
{
    private readonly Card[] _cards;
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

internal sealed class Card
{
    //                                      Spad Hear Diam Club  A   K   Q   J   T   9   8   7   6   5   4   3   2
    public const ulong MaskS = 0b_000000000_0001_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong MaskH = 0b_000000000_0000_0001_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong MaskD = 0b_000000000_0000_0000_0001_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong MaskC = 0b_000000000_0000_0000_0000_0001_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong MaskA = 0b_000000000_0000_0000_0000_0000_001_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong MaskK = 0b_000000000_0000_0000_0000_0000_000_001_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong MaskQ = 0b_000000000_0000_0000_0000_0000_000_000_001_000_000_000_000_000_000_000_000_000_000;
    public const ulong MaskJ = 0b_000000000_0000_0000_0000_0000_000_000_000_001_000_000_000_000_000_000_000_000_000;
    public const ulong MaskT = 0b_000000000_0000_0000_0000_0000_000_000_000_000_001_000_000_000_000_000_000_000_000;
    public const ulong Mask9 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_001_000_000_000_000_000_000_000;
    public const ulong Mask8 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_001_000_000_000_000_000_000;
    public const ulong Mask7 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_001_000_000_000_000_000;
    public const ulong Mask6 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_001_000_000_000_000;
    public const ulong Mask5 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_001_000_000_000;
    public const ulong Mask4 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_001_000_000;
    public const ulong Mask3 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_001_000;
    public const ulong Mask2 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_001;

    public Value Value { get; }
    public Suit Suit { get; }
    public ulong Mask { get; }

    public Card(Value value, Suit suit)
    {
        Value = value;
        Suit = suit;
        Mask = _valueToMask[(int)value] | _suitToMask[(int)suit];
    }

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
        Mask2,
        Mask3,
        Mask4,
        Mask5,
        Mask6,
        Mask7,
        Mask8,
        Mask9,
        MaskT,
        MaskJ,
        MaskQ,
        MaskK,
        MaskA,
    };

    private static readonly ulong[] _suitToMask = new[]
    {
        0UL,
        MaskC,
        MaskD,
        MaskH,
        MaskS,
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
