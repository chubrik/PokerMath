namespace PokerMath.Experimental;

using System.Diagnostics;

internal static class Utils2
{
    #region Masks

    //                                                   Spad Hear Diam Club  A   K   Q   J   T   9   8   7   6   5   4   3   2
    public const ulong Mask_Spades /*  */ = 0b_000000000_0001_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_Hearts /*  */ = 0b_000000000_0000_0001_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_Diamonds /**/ = 0b_000000000_0000_0000_0001_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_Clubs /*   */ = 0b_000000000_0000_0000_0000_0001_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_A /*       */ = 0b_000000000_0000_0000_0000_0000_001_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_K /*       */ = 0b_000000000_0000_0000_0000_0000_000_001_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_Q /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_001_000_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_J /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_001_000_000_000_000_000_000_000_000_000;
    public const ulong Mask_T /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_001_000_000_000_000_000_000_000_000;
    public const ulong Mask_9 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_001_000_000_000_000_000_000_000;
    public const ulong Mask_8 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_001_000_000_000_000_000_000;
    public const ulong Mask_7 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_001_000_000_000_000_000;
    public const ulong Mask_6 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_001_000_000_000_000;
    public const ulong Mask_5 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_001_000_000_000;
    public const ulong Mask_4 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_001_000_000;
    public const ulong Mask_3 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_001_000;
    public const ulong Mask_2 /*       */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_001;
    private const ulong Mask_A_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_100_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_K_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_100_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_Q_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_100_000_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_J_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_100_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_T_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_100_000_000_000_000_000_000_000_000;
    private const ulong Mask_9_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_100_000_000_000_000_000_000_000;
    private const ulong Mask_8_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_100_000_000_000_000_000_000;
    private const ulong Mask_7_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_100_000_000_000_000_000;
    private const ulong Mask_6_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_100_000_000_000_000;
    private const ulong Mask_5_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_100_000_000_000;
    private const ulong Mask_4_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_100_000_000;
    private const ulong Mask_3_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_100_000;
    private const ulong Mask_2_x4 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_100;
    private const ulong Mask_Counter /**/ = 0b_000000000_0011_0011_0011_0011_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_Flush1 /* */ = 0b_000000000_1000_1000_1000_1000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_Flush2 /* */ = 0b_000000000_1000_1000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_Flush3 /* */ = 0b_000000000_1000_0000_1000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong Mask_StrA /*   */ = 0b_000000000_0000_0000_0000_0000_001_001_001_001_001_000_000_000_000_000_000_000_000;
    private const ulong Mask_StrK /*   */ = 0b_000000000_0000_0000_0000_0000_000_001_001_001_001_001_000_000_000_000_000_000_000;
    private const ulong Mask_StrQ /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_001_001_001_001_001_000_000_000_000_000_000;
    private const ulong Mask_StrJ /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_001_001_001_001_001_000_000_000_000_000;
    private const ulong Mask_StrT /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_001_001_001_001_001_000_000_000_000;
    private const ulong Mask_Str9 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_001_001_001_001_001_000_000_000;
    private const ulong Mask_Str8 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_001_001_001_001_001_000_000;
    private const ulong Mask_Str7 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_001_001_001_001_001_000;
    private const ulong Mask_Str6 /*   */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_001_001_001_001_001;
    private const ulong Mask_Str5 /*   */ = 0b_000000000_0000_0000_0000_0000_001_000_000_000_000_000_000_000_000_001_001_001_001;
    private const ulong Mask_StrA_x4 /**/ = 0b_000000000_0000_0000_0000_0000_100_100_100_100_100_000_000_000_000_000_000_000_000;
    private const ulong Mask_StrK_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_100_100_100_100_100_000_000_000_000_000_000_000;
    private const ulong Mask_StrQ_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_100_100_100_100_100_000_000_000_000_000_000;
    private const ulong Mask_StrJ_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_100_100_100_100_100_000_000_000_000_000;
    private const ulong Mask_StrT_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_100_100_100_100_100_000_000_000_000;
    private const ulong Mask_Str9_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_100_100_100_100_100_000_000_000;
    private const ulong Mask_Str8_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_100_100_100_100_100_000_000;
    private const ulong Mask_Str7_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_100_100_100_100_100_000;
    private const ulong Mask_Str6_x4 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_100_100_100_100_100;
    private const ulong Mask_Str5_x4 /**/ = 0b_000000000_0000_0000_0000_0000_100_000_000_000_000_000_000_000_000_100_100_100_100;
    private const ulong Mask_Vals /*   */ = 0b_000000000_0000_0000_0000_0000_001_001_001_001_001_001_001_001_001_001_001_001_001;
    private const ulong Mask_Vals_x3 /**/ = 0b_000000000_0000_0000_0000_0000_011_011_011_011_011_011_011_011_011_011_011_011_011;
    private const ulong Mask_Vals_x4 /**/ = 0b_000000000_0000_0000_0000_0000_100_100_100_100_100_100_100_100_100_100_100_100_100;

    #endregion

    public static Winner GetWinner(Dial2 dial)
    {
        var playerCnt = dial.PlayerFlopCounter + dial.Board3.Mask + dial.Board4.Mask;
        var casinoCnt = dial.FlopCounter + dial.Board3.Mask + dial.Board4.Mask + dial.Casino0.Mask + dial.Casino1.Mask;

        var playerHasFlush = (playerCnt & Mask_Flush1) != 0;
        var casinoHasFlush = (casinoCnt & Mask_Flush1) != 0;

        if (playerHasFlush != casinoHasFlush)
            return playerHasFlush ? Winner.Player : Winner.Casino;

        if (playerHasFlush)
        {
            var flushSuitMask = (playerCnt & Mask_Flush2) != 0
                ? (playerCnt & Mask_Flush3) != 0 ? Mask_Spades : Mask_Hearts
                : (playerCnt & Mask_Flush3) != 0 ? Mask_Diamonds : Mask_Clubs;

        }

        throw new NotImplementedException();
    }
}

internal readonly struct Dial2
{
    private readonly Card2[] _cards;

    public Dial2(Card2[] cards)
    {
        Debug.Assert(cards.Length == 11);
        _cards = cards;
    }

    public Card2 Player0 { get => _cards[0]; set => _cards[0] = value; }
    public Card2 Player1 { get => _cards[1]; set => _cards[1] = value; }
    public Card2 Board0 { get => _cards[2]; set => _cards[2] = value; }
    public Card2 Board1 { get => _cards[3]; set => _cards[3] = value; }
    public Card2 Board2 { get => _cards[4]; set => _cards[4] = value; }
    public Card2 Board3 { get => _cards[5]; set => _cards[5] = value; }
    public Card2 Board4 { get => _cards[6]; set => _cards[6] = value; }
    public Card2 Casino0 { get => _cards[7]; set => _cards[7] = value; }
    public Card2 Casino1 { get => _cards[8]; set => _cards[8] = value; }
    public ulong PlayerFlopCounter => _cards[9].Mask;
    public ulong FlopCounter => _cards[10].Mask;
}

internal readonly struct Card2
{
    public ulong Mask { get; }

    private Card2(ulong mask)
    {
        Mask = mask;
    }

    public static readonly Card2 _As = new(_A | _s);
    public static readonly Card2 _Ah = new(_A | _h);
    public static readonly Card2 _Ad = new(_A | _d);
    public static readonly Card2 _Ac = new(_A | _c);
    public static readonly Card2 _Ks = new(_K | _s);
    public static readonly Card2 _Kh = new(_K | _h);
    public static readonly Card2 _Kd = new(_K | _d);
    public static readonly Card2 _Kc = new(_K | _c);
    public static readonly Card2 _Qs = new(_Q | _s);
    public static readonly Card2 _Qh = new(_Q | _h);
    public static readonly Card2 _Qd = new(_Q | _d);
    public static readonly Card2 _Qc = new(_Q | _c);
    public static readonly Card2 _Js = new(_J | _s);
    public static readonly Card2 _Jh = new(_J | _h);
    public static readonly Card2 _Jd = new(_J | _d);
    public static readonly Card2 _Jc = new(_J | _c);
    public static readonly Card2 _Ts = new(_T | _s);
    public static readonly Card2 _Th = new(_T | _h);
    public static readonly Card2 _Td = new(_T | _d);
    public static readonly Card2 _Tc = new(_T | _c);
    public static readonly Card2 _9s = new(_9 | _s);
    public static readonly Card2 _9h = new(_9 | _h);
    public static readonly Card2 _9d = new(_9 | _d);
    public static readonly Card2 _9c = new(_9 | _c);
    public static readonly Card2 _8s = new(_8 | _s);
    public static readonly Card2 _8h = new(_8 | _h);
    public static readonly Card2 _8d = new(_8 | _d);
    public static readonly Card2 _8c = new(_8 | _c);
    public static readonly Card2 _7s = new(_7 | _s);
    public static readonly Card2 _7h = new(_7 | _h);
    public static readonly Card2 _7d = new(_7 | _d);
    public static readonly Card2 _7c = new(_7 | _c);
    public static readonly Card2 _6s = new(_6 | _s);
    public static readonly Card2 _6h = new(_6 | _h);
    public static readonly Card2 _6d = new(_6 | _d);
    public static readonly Card2 _6c = new(_6 | _c);
    public static readonly Card2 _5s = new(_5 | _s);
    public static readonly Card2 _5h = new(_5 | _h);
    public static readonly Card2 _5d = new(_5 | _d);
    public static readonly Card2 _5c = new(_5 | _c);
    public static readonly Card2 _4s = new(_4 | _s);
    public static readonly Card2 _4h = new(_4 | _h);
    public static readonly Card2 _4d = new(_4 | _d);
    public static readonly Card2 _4c = new(_4 | _c);
    public static readonly Card2 _3s = new(_3 | _s);
    public static readonly Card2 _3h = new(_3 | _h);
    public static readonly Card2 _3d = new(_3 | _d);
    public static readonly Card2 _3c = new(_3 | _c);
    public static readonly Card2 _2s = new(_2 | _s);
    public static readonly Card2 _2h = new(_2 | _h);
    public static readonly Card2 _2d = new(_2 | _d);
    public static readonly Card2 _2c = new(_2 | _c);

    //                                   Spad Hear Diam Club  A   K   Q   J   T   9   8   7   6   5   4   3   2
    public const ulong _s = 0b_000000000_0001_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong _h = 0b_000000000_0000_0001_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong _d = 0b_000000000_0000_0000_0001_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong _c = 0b_000000000_0000_0000_0000_0001_000_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong _A = 0b_000000000_0000_0000_0000_0000_001_000_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong _K = 0b_000000000_0000_0000_0000_0000_000_001_000_000_000_000_000_000_000_000_000_000_000;
    public const ulong _Q = 0b_000000000_0000_0000_0000_0000_000_000_001_000_000_000_000_000_000_000_000_000_000;
    public const ulong _J = 0b_000000000_0000_0000_0000_0000_000_000_000_001_000_000_000_000_000_000_000_000_000;
    public const ulong _T = 0b_000000000_0000_0000_0000_0000_000_000_000_000_001_000_000_000_000_000_000_000_000;
    public const ulong _9 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_001_000_000_000_000_000_000_000;
    public const ulong _8 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_001_000_000_000_000_000_000;
    public const ulong _7 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_001_000_000_000_000_000;
    public const ulong _6 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_001_000_000_000_000;
    public const ulong _5 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_001_000_000_000;
    public const ulong _4 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_001_000_000;
    public const ulong _3 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_001_000;
    public const ulong _2 = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_001;
}
