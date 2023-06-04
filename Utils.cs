namespace PokerMath;

using System.Diagnostics;

internal static class Utils
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

    public static WinCtx GetWinCtx(IReadOnlyList<Card> player, IReadOnlyList<Card> casino, IReadOnlyList<Card> board)
    {
        Debug.Assert(player.Count == 2);
        Debug.Assert(casino.Count == 2);
        Debug.Assert(board.Count == 5);

        var playerCtx = GetCtx(player: player, board: board);
        var casinoCtx = GetCtx(player: casino, board: board);

        if (playerCtx.Kind > casinoCtx.Kind)
            return new WinCtx(Winner.Player, playerCtx, casinoCtx);

        if (playerCtx.Kind < casinoCtx.Kind)
            return new WinCtx(Winner.Casino, playerCtx, casinoCtx);

        if (playerCtx.Value0 != casinoCtx.Value0)
            return new WinCtx(playerCtx.Value0 > casinoCtx.Value0 ? Winner.Player : Winner.Casino, playerCtx, casinoCtx);

        if (playerCtx.Value1 != casinoCtx.Value1)
            return new WinCtx(playerCtx.Value1 > casinoCtx.Value1 ? Winner.Player : Winner.Casino, playerCtx, casinoCtx);

        if (playerCtx.Value2 != casinoCtx.Value2)
            return new WinCtx(playerCtx.Value2 > casinoCtx.Value2 ? Winner.Player : Winner.Casino, playerCtx, casinoCtx);

        if (playerCtx.Value3 != casinoCtx.Value3)
            return new WinCtx(playerCtx.Value3 > casinoCtx.Value3 ? Winner.Player : Winner.Casino, playerCtx, casinoCtx);

        return new WinCtx(
            playerCtx.Value4 == casinoCtx.Value4 ? Winner.Split
                : playerCtx.Value4 > casinoCtx.Value4 ? Winner.Player : Winner.Casino,
            playerCtx, casinoCtx);
    }

    private static PlayerCtx GetCtx(IReadOnlyList<Card> player, IReadOnlyList<Card> board)
    {
        var counter = Mask_Counter + player[0].Mask + player[1].Mask +
                      board[0].Mask + board[1].Mask + board[2].Mask + board[3].Mask + board[4].Mask;

        var hasFlush = (counter & Mask_Flush1) != 0;

        // Флеш / стрит-флеш
        if (hasFlush)
        {
            var flushSuitMask = (counter & Mask_Flush2) != 0
                ? (counter & Mask_Flush3) != 0 ? Mask_Spades : Mask_Hearts
                : (counter & Mask_Flush3) != 0 ? Mask_Diamonds : Mask_Clubs;

            var flush = 0UL;
            if ((player[0].Mask & flushSuitMask) != 0) flush |= player[0].Mask;
            if ((player[1].Mask & flushSuitMask) != 0) flush |= player[1].Mask;
            if ((board[0].Mask & flushSuitMask) != 0) flush |= board[0].Mask;
            if ((board[1].Mask & flushSuitMask) != 0) flush |= board[1].Mask;
            if ((board[2].Mask & flushSuitMask) != 0) flush |= board[2].Mask;
            if ((board[3].Mask & flushSuitMask) != 0) flush |= board[3].Mask;
            if ((board[4].Mask & flushSuitMask) != 0) flush |= board[4].Mask;

            // Стрит-флеш
            if ((flush & Mask_StrA) == Mask_StrA) return new PlayerCtx(WinKind.RoyalFlush);
            if ((flush & Mask_StrK) == Mask_StrK) return new PlayerCtx(WinKind.StraightFlush, Value._K);
            if ((flush & Mask_StrQ) == Mask_StrQ) return new PlayerCtx(WinKind.StraightFlush, Value._Q);
            if ((flush & Mask_StrJ) == Mask_StrJ) return new PlayerCtx(WinKind.StraightFlush, Value._J);
            if ((flush & Mask_StrT) == Mask_StrT) return new PlayerCtx(WinKind.StraightFlush, Value._T);
            if ((flush & Mask_Str9) == Mask_Str9) return new PlayerCtx(WinKind.StraightFlush, Value._9);
            if ((flush & Mask_Str8) == Mask_Str8) return new PlayerCtx(WinKind.StraightFlush, Value._8);
            if ((flush & Mask_Str7) == Mask_Str7) return new PlayerCtx(WinKind.StraightFlush, Value._7);
            if ((flush & Mask_Str6) == Mask_Str6) return new PlayerCtx(WinKind.StraightFlush, Value._6);
            if ((flush & Mask_Str5) == Mask_Str5) return new PlayerCtx(WinKind.StraightFlush, Value._5);

            // Флеш
            var flushValues = new Value[7];
            var flushValuesIndex = 0;
            if ((flush & Mask_A) != 0) flushValues[flushValuesIndex++] = Value._A;
            if ((flush & Mask_K) != 0) flushValues[flushValuesIndex++] = Value._K;
            if ((flush & Mask_Q) != 0) flushValues[flushValuesIndex++] = Value._Q;
            if ((flush & Mask_J) != 0) flushValues[flushValuesIndex++] = Value._J;
            if ((flush & Mask_T) != 0) flushValues[flushValuesIndex++] = Value._T;
            if ((flush & Mask_9) != 0) flushValues[flushValuesIndex++] = Value._9;
            if ((flush & Mask_8) != 0) flushValues[flushValuesIndex++] = Value._8;
            if ((flush & Mask_7) != 0) flushValues[flushValuesIndex++] = Value._7;
            if ((flush & Mask_6) != 0) flushValues[flushValuesIndex++] = Value._6;
            if ((flush & Mask_5) != 0) flushValues[flushValuesIndex++] = Value._5;
            if ((flush & Mask_4) != 0) flushValues[flushValuesIndex++] = Value._4;
            if ((flush & Mask_3) != 0) flushValues[flushValuesIndex++] = Value._3;
            if ((flush & Mask_2) != 0) flushValues[flushValuesIndex++] = Value._2;

            return new PlayerCtx(WinKind.Flush, flushValues[0], flushValues[1], flushValues[2], flushValues[3], flushValues[4]);
        }

        var shiftCounter = counter;
        var groups4 = shiftCounter & Mask_Vals_x4;
        shiftCounter = (shiftCounter & Mask_Vals_x3) + Mask_Vals;
        var groups3 = shiftCounter & Mask_Vals_x4;
        shiftCounter = (shiftCounter & Mask_Vals_x3) + Mask_Vals;
        var groups2 = shiftCounter & Mask_Vals_x4;
        shiftCounter = (shiftCounter & Mask_Vals_x3) + Mask_Vals;
        var groups1 = shiftCounter & Mask_Vals_x4;

        // Каре
        if (groups4 != 0)
        {
            var four = (groups4 & Mask_A_x4) != 0 ? Value._A
                     : (groups4 & Mask_K_x4) != 0 ? Value._K
                     : (groups4 & Mask_Q_x4) != 0 ? Value._Q
                     : (groups4 & Mask_J_x4) != 0 ? Value._J
                     : (groups4 & Mask_T_x4) != 0 ? Value._T
                     : (groups4 & Mask_9_x4) != 0 ? Value._9
                     : (groups4 & Mask_8_x4) != 0 ? Value._8
                     : (groups4 & Mask_7_x4) != 0 ? Value._7
                     : (groups4 & Mask_6_x4) != 0 ? Value._6
                     : (groups4 & Mask_5_x4) != 0 ? Value._5
                     : (groups4 & Mask_4_x4) != 0 ? Value._4
                     : (groups4 & Mask_3_x4) != 0 ? Value._3
                     : (groups4 & Mask_2_x4) != 0 ? Value._2
                     : throw new InvalidOperationException();

            var groups321 = groups3 | groups2 | groups1;

            if ((groups321 & Mask_A_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._A);
            if ((groups321 & Mask_K_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._K);
            if ((groups321 & Mask_Q_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._Q);
            if ((groups321 & Mask_J_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._J);
            if ((groups321 & Mask_T_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._T);
            if ((groups321 & Mask_9_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._9);
            if ((groups321 & Mask_8_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._8);
            if ((groups321 & Mask_7_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._7);
            if ((groups321 & Mask_6_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._6);
            if ((groups321 & Mask_5_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._5);
            if ((groups321 & Mask_4_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._4);
            if ((groups321 & Mask_3_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._3);
            if ((groups321 & Mask_2_x4) != 0) return new PlayerCtx(WinKind.FourOfKind, four, Value._2);
            throw new InvalidOperationException();
        }

        // Стрит
        var values = counter + Mask_Vals_x3;
        if ((values & Mask_StrA_x4) == Mask_StrA_x4) return new PlayerCtx(WinKind.Straight, Value._A);
        if ((values & Mask_StrK_x4) == Mask_StrK_x4) return new PlayerCtx(WinKind.Straight, Value._K);
        if ((values & Mask_StrQ_x4) == Mask_StrQ_x4) return new PlayerCtx(WinKind.Straight, Value._Q);
        if ((values & Mask_StrJ_x4) == Mask_StrJ_x4) return new PlayerCtx(WinKind.Straight, Value._J);
        if ((values & Mask_StrT_x4) == Mask_StrT_x4) return new PlayerCtx(WinKind.Straight, Value._T);
        if ((values & Mask_Str9_x4) == Mask_Str9_x4) return new PlayerCtx(WinKind.Straight, Value._9);
        if ((values & Mask_Str8_x4) == Mask_Str8_x4) return new PlayerCtx(WinKind.Straight, Value._8);
        if ((values & Mask_Str7_x4) == Mask_Str7_x4) return new PlayerCtx(WinKind.Straight, Value._7);
        if ((values & Mask_Str6_x4) == Mask_Str6_x4) return new PlayerCtx(WinKind.Straight, Value._6);
        if ((values & Mask_Str5_x4) == Mask_Str5_x4) return new PlayerCtx(WinKind.Straight, Value._5);

        // Сет / Фулл-хаус
        if (groups3 != 0)
        {
            Value three;
            if ((groups3 & Mask_A_x4) != 0) { three = Value._A; goto FullHouse_K; }
            if ((groups3 & Mask_K_x4) != 0) { three = Value._K; goto FullHouse_Q; }
            if ((groups3 & Mask_Q_x4) != 0) { three = Value._Q; goto FullHouse_J; }
            if ((groups3 & Mask_J_x4) != 0) { three = Value._J; goto FullHouse_T; }
            if ((groups3 & Mask_T_x4) != 0) { three = Value._T; goto FullHouse_9; }
            if ((groups3 & Mask_9_x4) != 0) { three = Value._9; goto FullHouse_8; }
            if ((groups3 & Mask_8_x4) != 0) { three = Value._8; goto FullHouse_7; }
            if ((groups3 & Mask_7_x4) != 0) { three = Value._7; goto FullHouse_6; }
            if ((groups3 & Mask_6_x4) != 0) { three = Value._6; goto FullHouse_5; }
            if ((groups3 & Mask_5_x4) != 0) { three = Value._5; goto FullHouse_4; }
            if ((groups3 & Mask_4_x4) != 0) { three = Value._4; goto FullHouse_3; }
            if ((groups3 & Mask_3_x4) != 0) { three = Value._3; goto FullHouse_2; }
            if ((groups3 & Mask_2_x4) != 0) { three = Value._2; goto FullHouse_1; }
            throw new InvalidOperationException();

            // Фулл-хаус
            FullHouse_K: if ((groups3 & Mask_K_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._K);
            FullHouse_Q: if ((groups3 & Mask_Q_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._Q);
            FullHouse_J: if ((groups3 & Mask_J_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._J);
            FullHouse_T: if ((groups3 & Mask_T_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._T);
            FullHouse_9: if ((groups3 & Mask_9_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._9);
            FullHouse_8: if ((groups3 & Mask_8_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._8);
            FullHouse_7: if ((groups3 & Mask_7_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._7);
            FullHouse_6: if ((groups3 & Mask_6_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._6);
            FullHouse_5: if ((groups3 & Mask_5_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._5);
            FullHouse_4: if ((groups3 & Mask_4_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._4);
            FullHouse_3: if ((groups3 & Mask_3_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._3);
            FullHouse_2: if ((groups3 & Mask_2_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._2);
            FullHouse_1:
            
            if ((groups2 & Mask_A_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._A);
            if ((groups2 & Mask_K_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._K);
            if ((groups2 & Mask_Q_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._Q);
            if ((groups2 & Mask_J_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._J);
            if ((groups2 & Mask_T_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._T);
            if ((groups2 & Mask_9_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._9);
            if ((groups2 & Mask_8_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._8);
            if ((groups2 & Mask_7_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._7);
            if ((groups2 & Mask_6_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._6);
            if ((groups2 & Mask_5_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._5);
            if ((groups2 & Mask_4_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._4);
            if ((groups2 & Mask_3_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._3);
            if ((groups2 & Mask_2_x4) != 0) return new PlayerCtx(WinKind.FullHouse, three, Value._2);

            // Сет
            Value rest0;
            if ((groups1 & Mask_A_x4) != 0) { rest0 = Value._A; goto Three_K; }
            if ((groups1 & Mask_K_x4) != 0) { rest0 = Value._K; goto Three_Q; }
            if ((groups1 & Mask_Q_x4) != 0) { rest0 = Value._Q; goto Three_J; }
            if ((groups1 & Mask_J_x4) != 0) { rest0 = Value._J; goto Three_T; }
            if ((groups1 & Mask_T_x4) != 0) { rest0 = Value._T; goto Three_9; }
            if ((groups1 & Mask_9_x4) != 0) { rest0 = Value._9; goto Three_8; }
            if ((groups1 & Mask_8_x4) != 0) { rest0 = Value._8; goto Three_7; }
            if ((groups1 & Mask_7_x4) != 0) { rest0 = Value._7; goto Three_6; }
            if ((groups1 & Mask_6_x4) != 0) { rest0 = Value._6; goto Three_5; }
            if ((groups1 & Mask_5_x4) != 0) { rest0 = Value._5; goto Three_4; }
            if ((groups1 & Mask_4_x4) != 0) { rest0 = Value._4; goto Three_3; }
            if ((groups1 & Mask_3_x4) != 0) { rest0 = Value._3; goto Three_2; }
            throw new InvalidOperationException();

            Three_K: if ((groups1 & Mask_K_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._K);
            Three_Q: if ((groups1 & Mask_Q_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._Q);
            Three_J: if ((groups1 & Mask_J_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._J);
            Three_T: if ((groups1 & Mask_T_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._T);
            Three_9: if ((groups1 & Mask_9_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._9);
            Three_8: if ((groups1 & Mask_8_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._8);
            Three_7: if ((groups1 & Mask_7_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._7);
            Three_6: if ((groups1 & Mask_6_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._6);
            Three_5: if ((groups1 & Mask_5_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._5);
            Three_4: if ((groups1 & Mask_4_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._4);
            Three_3: if ((groups1 & Mask_3_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._3);
            Three_2: if ((groups1 & Mask_2_x4) != 0) return new PlayerCtx(WinKind.ThreeOfKind, three, rest0, Value._2);
            throw new InvalidOperationException();
        }

        // Пара / две пары
        if (groups2 != 0)
        {
            Value pair0;
            Value pair1;
            Value rest;
            if ((groups2 & Mask_A_x4) != 0) { pair0 = Value._A; goto TwoPair1_K; }
            if ((groups2 & Mask_K_x4) != 0) { pair0 = Value._K; goto TwoPair1_Q; }
            if ((groups2 & Mask_Q_x4) != 0) { pair0 = Value._Q; goto TwoPair1_J; }
            if ((groups2 & Mask_J_x4) != 0) { pair0 = Value._J; goto TwoPair1_T; }
            if ((groups2 & Mask_T_x4) != 0) { pair0 = Value._T; goto TwoPair1_9; }
            if ((groups2 & Mask_9_x4) != 0) { pair0 = Value._9; goto TwoPair1_8; }
            if ((groups2 & Mask_8_x4) != 0) { pair0 = Value._8; goto TwoPair1_7; }
            if ((groups2 & Mask_7_x4) != 0) { pair0 = Value._7; goto TwoPair1_6; }
            if ((groups2 & Mask_6_x4) != 0) { pair0 = Value._6; goto TwoPair1_5; }
            if ((groups2 & Mask_5_x4) != 0) { pair0 = Value._5; goto TwoPair1_4; }
            if ((groups2 & Mask_4_x4) != 0) { pair0 = Value._4; goto TwoPair1_3; }
            if ((groups2 & Mask_3_x4) != 0) { pair0 = Value._3; goto TwoPair1_2; }
            if ((groups2 & Mask_2_x4) != 0) { pair0 = Value._2; goto Pair; }
            throw new InvalidOperationException();

            // Две пары
            TwoPair1_K: if ((groups2 & Mask_K_x4) != 0) { pair1 = Value._K; goto TwoPair2_Q; }
            TwoPair1_Q: if ((groups2 & Mask_Q_x4) != 0) { pair1 = Value._Q; goto TwoPair2_J; }
            TwoPair1_J: if ((groups2 & Mask_J_x4) != 0) { pair1 = Value._J; goto TwoPair2_T; }
            TwoPair1_T: if ((groups2 & Mask_T_x4) != 0) { pair1 = Value._T; goto TwoPair2_9; }
            TwoPair1_9: if ((groups2 & Mask_9_x4) != 0) { pair1 = Value._9; goto TwoPair2_8; }
            TwoPair1_8: if ((groups2 & Mask_8_x4) != 0) { pair1 = Value._8; goto TwoPair2_7; }
            TwoPair1_7: if ((groups2 & Mask_7_x4) != 0) { pair1 = Value._7; goto TwoPair2_6; }
            TwoPair1_6: if ((groups2 & Mask_6_x4) != 0) { pair1 = Value._6; goto TwoPair2_5; }
            TwoPair1_5: if ((groups2 & Mask_5_x4) != 0) { pair1 = Value._5; goto TwoPair2_4; }
            TwoPair1_4: if ((groups2 & Mask_4_x4) != 0) { pair1 = Value._4; goto TwoPair2_3; }
            TwoPair1_3: if ((groups2 & Mask_3_x4) != 0) { pair1 = Value._3; goto TwoPair2_2; }
            TwoPair1_2: if ((groups2 & Mask_2_x4) != 0) { pair1 = Value._2; goto TwoPair2_1; }
            goto Pair;

            TwoPair2_Q: if ((groups2 & Mask_Q_x4) != 0) { rest = Value._Q; goto TwoPair3; }
            TwoPair2_J: if ((groups2 & Mask_J_x4) != 0) { rest = Value._J; goto TwoPair3; }
            TwoPair2_T: if ((groups2 & Mask_T_x4) != 0) { rest = Value._T; goto TwoPair3; }
            TwoPair2_9: if ((groups2 & Mask_9_x4) != 0) { rest = Value._9; goto TwoPair3; }
            TwoPair2_8: if ((groups2 & Mask_8_x4) != 0) { rest = Value._8; goto TwoPair3; }
            TwoPair2_7: if ((groups2 & Mask_7_x4) != 0) { rest = Value._7; goto TwoPair3; }
            TwoPair2_6: if ((groups2 & Mask_6_x4) != 0) { rest = Value._6; goto TwoPair3; }
            TwoPair2_5: if ((groups2 & Mask_5_x4) != 0) { rest = Value._5; goto TwoPair3; }
            TwoPair2_4: if ((groups2 & Mask_4_x4) != 0) { rest = Value._4; goto TwoPair3; }
            TwoPair2_3: if ((groups2 & Mask_3_x4) != 0) { rest = Value._3; goto TwoPair3; }
            TwoPair2_2: if ((groups2 & Mask_2_x4) != 0) { rest = Value._2; goto TwoPair3; }
            TwoPair2_1: rest = Value._2;

            TwoPair3:
            if ((groups1 & Mask_A_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._A ? rest : Value._A);
            if ((groups1 & Mask_K_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._K ? rest : Value._K);
            if ((groups1 & Mask_Q_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._Q ? rest : Value._Q);
            if ((groups1 & Mask_J_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._J ? rest : Value._J);
            if ((groups1 & Mask_T_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._T ? rest : Value._T);
            if ((groups1 & Mask_9_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._9 ? rest : Value._9);
            if ((groups1 & Mask_8_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._8 ? rest : Value._8);
            if ((groups1 & Mask_7_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._7 ? rest : Value._7);
            if ((groups1 & Mask_6_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._6 ? rest : Value._6);
            if ((groups1 & Mask_5_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._5 ? rest : Value._5);
            if ((groups1 & Mask_4_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._4 ? rest : Value._4);
            if ((groups1 & Mask_3_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._3 ? rest : Value._3);
            if ((groups1 & Mask_2_x4) != 0) return new PlayerCtx(WinKind.TwoPair, pair0, pair1, rest > Value._2 ? rest : Value._2);
            throw new InvalidOperationException();

            // Пара
            Pair:
            Value rest0;
            Value rest1;
            if ((groups1 & Mask_A_x4) != 0) { rest0 = Value._A; goto Pair1_K; }
            if ((groups1 & Mask_K_x4) != 0) { rest0 = Value._K; goto Pair1_Q; }
            if ((groups1 & Mask_Q_x4) != 0) { rest0 = Value._Q; goto Pair1_J; }
            if ((groups1 & Mask_J_x4) != 0) { rest0 = Value._J; goto Pair1_T; }
            if ((groups1 & Mask_T_x4) != 0) { rest0 = Value._T; goto Pair1_9; }
            if ((groups1 & Mask_9_x4) != 0) { rest0 = Value._9; goto Pair1_8; }
            if ((groups1 & Mask_8_x4) != 0) { rest0 = Value._8; goto Pair1_7; }
            if ((groups1 & Mask_7_x4) != 0) { rest0 = Value._7; goto Pair1_6; }
            if ((groups1 & Mask_6_x4) != 0) { rest0 = Value._6; goto Pair1_5; }
            if ((groups1 & Mask_5_x4) != 0) { rest0 = Value._5; goto Pair1_4; }
            if ((groups1 & Mask_4_x4) != 0) { rest0 = Value._4; goto Pair1_3; }
            throw new InvalidOperationException();

            Pair1_K: if ((groups1 & Mask_K_x4) != 0) { rest1 = Value._K; goto Pair2_Q; }
            Pair1_Q: if ((groups1 & Mask_Q_x4) != 0) { rest1 = Value._Q; goto Pair2_J; }
            Pair1_J: if ((groups1 & Mask_J_x4) != 0) { rest1 = Value._J; goto Pair2_T; }
            Pair1_T: if ((groups1 & Mask_T_x4) != 0) { rest1 = Value._T; goto Pair2_9; }
            Pair1_9: if ((groups1 & Mask_9_x4) != 0) { rest1 = Value._9; goto Pair2_8; }
            Pair1_8: if ((groups1 & Mask_8_x4) != 0) { rest1 = Value._8; goto Pair2_7; }
            Pair1_7: if ((groups1 & Mask_7_x4) != 0) { rest1 = Value._7; goto Pair2_6; }
            Pair1_6: if ((groups1 & Mask_6_x4) != 0) { rest1 = Value._6; goto Pair2_5; }
            Pair1_5: if ((groups1 & Mask_5_x4) != 0) { rest1 = Value._5; goto Pair2_4; }
            Pair1_4: if ((groups1 & Mask_4_x4) != 0) { rest1 = Value._4; goto Pair2_3; }
            Pair1_3: if ((groups1 & Mask_3_x4) != 0) { rest1 = Value._3; goto Pair2_2; }
            throw new InvalidOperationException();

            Pair2_Q: if ((groups1 & Mask_Q_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._Q);
            Pair2_J: if ((groups1 & Mask_J_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._J);
            Pair2_T: if ((groups1 & Mask_T_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._T);
            Pair2_9: if ((groups1 & Mask_9_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._9);
            Pair2_8: if ((groups1 & Mask_8_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._8);
            Pair2_7: if ((groups1 & Mask_7_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._7);
            Pair2_6: if ((groups1 & Mask_6_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._6);
            Pair2_5: if ((groups1 & Mask_5_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._5);
            Pair2_4: if ((groups1 & Mask_4_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._4);
            Pair2_3: if ((groups1 & Mask_3_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._3);
            Pair2_2: if ((groups1 & Mask_2_x4) != 0) return new PlayerCtx(WinKind.Pair, pair0, rest0, rest1, Value._2);
            throw new InvalidOperationException();
        }

        // Старшая карта
        var highValues = new Value[7];
        var highValuesIndex = 0;
        if ((groups1 & Mask_A_x4) != 0) highValues[highValuesIndex++] = Value._A;
        if ((groups1 & Mask_K_x4) != 0) highValues[highValuesIndex++] = Value._K;
        if ((groups1 & Mask_Q_x4) != 0) highValues[highValuesIndex++] = Value._Q;
        if ((groups1 & Mask_J_x4) != 0) highValues[highValuesIndex++] = Value._J;
        if ((groups1 & Mask_T_x4) != 0) highValues[highValuesIndex++] = Value._T;
        if ((groups1 & Mask_9_x4) != 0) highValues[highValuesIndex++] = Value._9;
        if ((groups1 & Mask_8_x4) != 0) highValues[highValuesIndex++] = Value._8;
        if ((groups1 & Mask_7_x4) != 0) highValues[highValuesIndex++] = Value._7;
        if ((groups1 & Mask_6_x4) != 0) highValues[highValuesIndex++] = Value._6;
        if ((groups1 & Mask_5_x4) != 0) highValues[highValuesIndex++] = Value._5;
        if ((groups1 & Mask_4_x4) != 0) highValues[highValuesIndex++] = Value._4;
        if ((groups1 & Mask_3_x4) != 0) highValues[highValuesIndex++] = Value._3;
        if ((groups1 & Mask_2_x4) != 0) highValues[highValuesIndex++] = Value._2;

        return new PlayerCtx(WinKind.HighCard, highValues[0], highValues[1], highValues[2], highValues[3], highValues[4]);
    }
}

internal readonly struct PlayerCtx
{
    public WinKind Kind { get; }
    public Value Value0 { get; }
    public Value Value1 { get; }
    public Value Value2 { get; }
    public Value Value3 { get; }
    public Value Value4 { get; }

    public PlayerCtx(
        WinKind kind,
        Value value0 = default,
        Value value1 = default,
        Value value2 = default,
        Value value3 = default,
        Value value4 = default)
    {
        Kind = kind;
        Value0 = value0;
        Value1 = value1;
        Value2 = value2;
        Value3 = value3;
        Value4 = value4;
    }
}

internal enum WinKind : byte
{
    HighCard = 1,
    Pair,
    TwoPair,
    ThreeOfKind,
    Straight,
    Flush,
    FullHouse,
    FourOfKind,
    StraightFlush,
    RoyalFlush,
}

internal readonly struct WinCtx
{
    public Winner Winner { get; }
    public PlayerCtx Player { get; }
    public PlayerCtx Casino { get; }

    public WinCtx(Winner winner, PlayerCtx player, PlayerCtx casino)
    {
        Winner = winner;
        Player = player;
        Casino = casino;
    }
}

internal enum Winner
{
    Player = 1,
    Casino,
    Split
}
