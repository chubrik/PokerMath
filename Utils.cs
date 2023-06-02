namespace PokerMath;

using System.Diagnostics;

internal static class Utils
{
    #region Masks

    //                                           Spad Hear Diam Club  A   K   Q   J   T   9   8   7   6   5   4   3   2
    private const ulong CounterStart /* */ = 0b_000000000_0011_0011_0011_0011_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong HasFlushAMask /**/ = 0b_000000000_1000_1000_1000_1000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong HasFlushBMask /**/ = 0b_000000000_1000_1000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong HasFlushCMask /**/ = 0b_000000000_1000_0000_1000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong StraightAMas2 /**/ = 0b_000000000_0000_0000_0000_0000_001_001_001_001_001_000_000_000_000_000_000_000_000;
    private const ulong StraightKMas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_001_001_001_001_001_000_000_000_000_000_000_000;
    private const ulong StraightQMas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_001_001_001_001_001_000_000_000_000_000_000;
    private const ulong StraightJMas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_001_001_001_001_001_000_000_000_000_000;
    private const ulong StraightTMas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_001_001_001_001_001_000_000_000_000;
    private const ulong Straight9Mas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_001_001_001_001_001_000_000_000;
    private const ulong Straight8Mas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_001_001_001_001_001_000_000;
    private const ulong Straight7Mas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_001_001_001_001_001_000;
    private const ulong Straight6Mas2 /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_001_001_001_001_001;
    private const ulong Straight5Mas2 /**/ = 0b_000000000_0000_0000_0000_0000_001_000_000_000_000_000_000_000_000_001_001_001_001;
    private const ulong StraightAMask /**/ = 0b_000000000_0000_0000_0000_0000_100_100_100_100_100_000_000_000_000_000_000_000_000;
    private const ulong StraightKMask /**/ = 0b_000000000_0000_0000_0000_0000_000_100_100_100_100_100_000_000_000_000_000_000_000;
    private const ulong StraightQMask /**/ = 0b_000000000_0000_0000_0000_0000_000_000_100_100_100_100_100_000_000_000_000_000_000;
    private const ulong StraightJMask /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_100_100_100_100_100_000_000_000_000_000;
    private const ulong StraightTMask /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_100_100_100_100_100_000_000_000_000;
    private const ulong Straight9Mask /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_100_100_100_100_100_000_000_000;
    private const ulong Straight8Mask /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_100_100_100_100_100_000_000;
    private const ulong Straight7Mask /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_100_100_100_100_100_000;
    private const ulong Straight6Mask /**/ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_100_100_100_100_100;
    private const ulong Straight5Mask /**/ = 0b_000000000_0000_0000_0000_0000_100_000_000_000_000_000_000_000_000_100_100_100_100;
    private const ulong GroupMask /*    */ = 0b_000000000_0000_0000_0000_0000_100_100_100_100_100_100_100_100_100_100_100_100_100;
    private const ulong GroupPlusMask /**/ = 0b_000000000_0000_0000_0000_0000_001_001_001_001_001_001_001_001_001_001_001_001_001;
    private const ulong GroupAntiMask /**/ = 0b_000000000_0000_0000_0000_0000_011_011_011_011_011_011_011_011_011_011_011_011_011;
    private const ulong MaskAx4 /*      */ = 0b_000000000_0000_0000_0000_0000_100_000_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong MaskKx4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_100_000_000_000_000_000_000_000_000_000_000_000;
    private const ulong MaskQx4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_100_000_000_000_000_000_000_000_000_000_000;
    private const ulong MaskJx4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_100_000_000_000_000_000_000_000_000_000;
    private const ulong MaskTx4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_100_000_000_000_000_000_000_000_000;
    private const ulong Mask9x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_100_000_000_000_000_000_000_000;
    private const ulong Mask8x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_100_000_000_000_000_000_000;
    private const ulong Mask7x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_100_000_000_000_000_000;
    private const ulong Mask6x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_100_000_000_000_000;
    private const ulong Mask5x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_100_000_000_000;
    private const ulong Mask4x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_100_000_000;
    private const ulong Mask3x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_100_000;
    private const ulong Mask2x4 /*      */ = 0b_000000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_100;

    #endregion

    public static Winner GetWinner(IReadOnlyList<Card> player, IReadOnlyList<Card> casino, IReadOnlyList<Card> board)
    {
        Debug.Assert(player.Count == 2);
        Debug.Assert(casino.Count == 2);
        Debug.Assert(board.Count == 5);

        var playerCtx = GetCtx(playerOrCasino: player, board: board);
        var casinoCtx = GetCtx(playerOrCasino: casino, board: board);

        if (playerCtx.Kind > casinoCtx.Kind)
            return Winner.Player;

        if (playerCtx.Kind < casinoCtx.Kind)
            return Winner.Casino;

        if (playerCtx.Value0 != casinoCtx.Value0)
            return playerCtx.Value0 > casinoCtx.Value0 ? Winner.Player : Winner.Casino;

        if (playerCtx.Value1 != casinoCtx.Value1)
            return playerCtx.Value1 > casinoCtx.Value1 ? Winner.Player : Winner.Casino;

        if (playerCtx.Value2 != casinoCtx.Value2)
            return playerCtx.Value2 > casinoCtx.Value2 ? Winner.Player : Winner.Casino;

        if (playerCtx.Value3 != casinoCtx.Value3)
            return playerCtx.Value3 > casinoCtx.Value3 ? Winner.Player : Winner.Casino;

        return playerCtx.Value4 == casinoCtx.Value4 ? Winner.Split
             : playerCtx.Value4 > casinoCtx.Value4 ? Winner.Player : Winner.Casino;
    }

    private static WinCtx GetCtx(IReadOnlyList<Card> playerOrCasino, IReadOnlyList<Card> board)
    {
        var counter = CounterStart + playerOrCasino[0].Mask + playerOrCasino[1].Mask +
                      board[0].Mask + board[1].Mask + board[2].Mask + board[3].Mask + board[4].Mask;

        var hasFlush = (counter & HasFlushAMask) != 0;

        // Флеш / стрит-флеш
        if (hasFlush)
        {
            var flushSuitMask = (counter & HasFlushBMask) != 0
                ? (counter & HasFlushCMask) != 0 ? Card.MaskS : Card.MaskH
                : (counter & HasFlushCMask) != 0 ? Card.MaskD : Card.MaskC;

            var flush = 0UL;
            if ((playerOrCasino[0].Mask & flushSuitMask) != 0) flush |= playerOrCasino[0].Mask;
            if ((playerOrCasino[1].Mask & flushSuitMask) != 0) flush |= playerOrCasino[1].Mask;
            if ((board[0].Mask & flushSuitMask) != 0) flush |= board[0].Mask;
            if ((board[1].Mask & flushSuitMask) != 0) flush |= board[1].Mask;
            if ((board[2].Mask & flushSuitMask) != 0) flush |= board[2].Mask;
            if ((board[3].Mask & flushSuitMask) != 0) flush |= board[3].Mask;
            if ((board[4].Mask & flushSuitMask) != 0) flush |= board[4].Mask;

            // Стрит-флеш
            if ((flush & StraightAMas2) == StraightAMas2) return new WinCtx(WinKind.StraightFlush, Value._A);
            if ((flush & StraightKMas2) == StraightKMas2) return new WinCtx(WinKind.StraightFlush, Value._K);
            if ((flush & StraightQMas2) == StraightQMas2) return new WinCtx(WinKind.StraightFlush, Value._Q);
            if ((flush & StraightJMas2) == StraightJMas2) return new WinCtx(WinKind.StraightFlush, Value._J);
            if ((flush & StraightTMas2) == StraightTMas2) return new WinCtx(WinKind.StraightFlush, Value._T);
            if ((flush & Straight9Mas2) == Straight9Mas2) return new WinCtx(WinKind.StraightFlush, Value._9);
            if ((flush & Straight8Mas2) == Straight8Mas2) return new WinCtx(WinKind.StraightFlush, Value._8);
            if ((flush & Straight7Mas2) == Straight7Mas2) return new WinCtx(WinKind.StraightFlush, Value._7);
            if ((flush & Straight6Mas2) == Straight6Mas2) return new WinCtx(WinKind.StraightFlush, Value._6);
            if ((flush & Straight5Mas2) == Straight5Mas2) return new WinCtx(WinKind.StraightFlush, Value._5);

            // Флеш
            var flushValues = new Value[7];
            var flushValuesIndex = 0;
            if ((flush & Card.MaskA) != 0) flushValues[flushValuesIndex++] = Value._A;
            if ((flush & Card.MaskK) != 0) flushValues[flushValuesIndex++] = Value._K;
            if ((flush & Card.MaskQ) != 0) flushValues[flushValuesIndex++] = Value._Q;
            if ((flush & Card.MaskJ) != 0) flushValues[flushValuesIndex++] = Value._J;
            if ((flush & Card.MaskT) != 0) flushValues[flushValuesIndex++] = Value._T;
            if ((flush & Card.Mask9) != 0) flushValues[flushValuesIndex++] = Value._9;
            if ((flush & Card.Mask8) != 0) flushValues[flushValuesIndex++] = Value._8;
            if ((flush & Card.Mask7) != 0) flushValues[flushValuesIndex++] = Value._7;
            if ((flush & Card.Mask6) != 0) flushValues[flushValuesIndex++] = Value._6;
            if ((flush & Card.Mask5) != 0) flushValues[flushValuesIndex++] = Value._5;
            if ((flush & Card.Mask4) != 0) flushValues[flushValuesIndex++] = Value._4;
            if ((flush & Card.Mask3) != 0) flushValues[flushValuesIndex++] = Value._3;
            if ((flush & Card.Mask2) != 0) flushValues[flushValuesIndex++] = Value._2;

            return new WinCtx(WinKind.Flush, flushValues[0], flushValues[1], flushValues[2], flushValues[3], flushValues[4]);
        }

        var shiftCounter = counter;
        var groups4 = shiftCounter & GroupMask;
        shiftCounter = (shiftCounter & GroupAntiMask) + GroupPlusMask;
        var groups3 = shiftCounter & GroupMask;
        shiftCounter = (shiftCounter & GroupAntiMask) + GroupPlusMask;
        var groups2 = shiftCounter & GroupMask;
        shiftCounter = (shiftCounter & GroupAntiMask) + GroupPlusMask;
        var groups1 = shiftCounter & GroupMask;
        var groups321 = groups3 | groups2 | groups1;

        // Каре
        if (groups4 != 0)
        {
            var four = (groups4 & MaskAx4) != 0 ? Value._A
                     : (groups4 & MaskKx4) != 0 ? Value._K
                     : (groups4 & MaskQx4) != 0 ? Value._Q
                     : (groups4 & MaskJx4) != 0 ? Value._J
                     : (groups4 & MaskTx4) != 0 ? Value._T
                     : (groups4 & Mask9x4) != 0 ? Value._9
                     : (groups4 & Mask8x4) != 0 ? Value._8
                     : (groups4 & Mask7x4) != 0 ? Value._7
                     : (groups4 & Mask6x4) != 0 ? Value._6
                     : (groups4 & Mask5x4) != 0 ? Value._5
                     : (groups4 & Mask4x4) != 0 ? Value._4
                     : (groups4 & Mask3x4) != 0 ? Value._3
                     : (groups4 & Mask2x4) != 0 ? Value._2
                     : throw new InvalidOperationException();

            if ((groups321 & MaskAx4) != 0) return new WinCtx(WinKind.Four, four, Value._A);
            if ((groups321 & MaskKx4) != 0) return new WinCtx(WinKind.Four, four, Value._K);
            if ((groups321 & MaskQx4) != 0) return new WinCtx(WinKind.Four, four, Value._Q);
            if ((groups321 & MaskJx4) != 0) return new WinCtx(WinKind.Four, four, Value._J);
            if ((groups321 & MaskTx4) != 0) return new WinCtx(WinKind.Four, four, Value._T);
            if ((groups321 & Mask9x4) != 0) return new WinCtx(WinKind.Four, four, Value._9);
            if ((groups321 & Mask8x4) != 0) return new WinCtx(WinKind.Four, four, Value._8);
            if ((groups321 & Mask7x4) != 0) return new WinCtx(WinKind.Four, four, Value._7);
            if ((groups321 & Mask6x4) != 0) return new WinCtx(WinKind.Four, four, Value._6);
            if ((groups321 & Mask5x4) != 0) return new WinCtx(WinKind.Four, four, Value._5);
            if ((groups321 & Mask4x4) != 0) return new WinCtx(WinKind.Four, four, Value._4);
            if ((groups321 & Mask3x4) != 0) return new WinCtx(WinKind.Four, four, Value._3);
            if ((groups321 & Mask2x4) != 0) return new WinCtx(WinKind.Four, four, Value._2);
            throw new InvalidOperationException();
        }

        // Стрит
        var values = counter + GroupAntiMask;
        if ((values & StraightAMask) == StraightAMask) return new WinCtx(WinKind.Straight, Value._A);
        if ((values & StraightKMask) == StraightKMask) return new WinCtx(WinKind.Straight, Value._K);
        if ((values & StraightQMask) == StraightQMask) return new WinCtx(WinKind.Straight, Value._Q);
        if ((values & StraightJMask) == StraightJMask) return new WinCtx(WinKind.Straight, Value._J);
        if ((values & StraightTMask) == StraightTMask) return new WinCtx(WinKind.Straight, Value._T);
        if ((values & Straight9Mask) == Straight9Mask) return new WinCtx(WinKind.Straight, Value._9);
        if ((values & Straight8Mask) == Straight8Mask) return new WinCtx(WinKind.Straight, Value._8);
        if ((values & Straight7Mask) == Straight7Mask) return new WinCtx(WinKind.Straight, Value._7);
        if ((values & Straight6Mask) == Straight6Mask) return new WinCtx(WinKind.Straight, Value._6);
        if ((values & Straight5Mask) == Straight5Mask) return new WinCtx(WinKind.Straight, Value._5);

        // Сет / Фулл-хаус
        if (groups3 != 0)
        {
            Value three;
            if ((groups3 & MaskAx4) != 0) { three = Value._A; goto FullHouse_K; }
            if ((groups3 & MaskKx4) != 0) { three = Value._K; goto FullHouse_Q; }
            if ((groups3 & MaskQx4) != 0) { three = Value._Q; goto FullHouse_J; }
            if ((groups3 & MaskJx4) != 0) { three = Value._J; goto FullHouse_T; }
            if ((groups3 & MaskTx4) != 0) { three = Value._T; goto FullHouse_9; }
            if ((groups3 & Mask9x4) != 0) { three = Value._9; goto FullHouse_8; }
            if ((groups3 & Mask8x4) != 0) { three = Value._8; goto FullHouse_7; }
            if ((groups3 & Mask7x4) != 0) { three = Value._7; goto FullHouse_6; }
            if ((groups3 & Mask6x4) != 0) { three = Value._6; goto FullHouse_5; }
            if ((groups3 & Mask5x4) != 0) { three = Value._5; goto FullHouse_4; }
            if ((groups3 & Mask4x4) != 0) { three = Value._4; goto FullHouse_3; }
            if ((groups3 & Mask3x4) != 0) { three = Value._3; goto FullHouse_2; }
            if ((groups3 & Mask2x4) != 0) { three = Value._2; goto FullHouse_1; }
            throw new InvalidOperationException();

            // Фулл-хаус
            FullHouse_K: if ((groups3 & MaskKx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._K);
            FullHouse_Q: if ((groups3 & MaskQx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._Q);
            FullHouse_J: if ((groups3 & MaskJx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._J);
            FullHouse_T: if ((groups3 & MaskTx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._T);
            FullHouse_9: if ((groups3 & Mask9x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._9);
            FullHouse_8: if ((groups3 & Mask8x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._8);
            FullHouse_7: if ((groups3 & Mask7x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._7);
            FullHouse_6: if ((groups3 & Mask6x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._6);
            FullHouse_5: if ((groups3 & Mask5x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._5);
            FullHouse_4: if ((groups3 & Mask4x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._4);
            FullHouse_3: if ((groups3 & Mask3x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._3);
            FullHouse_2: if ((groups3 & Mask2x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._2);
            FullHouse_1:

            if ((groups2 & MaskAx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._A);
            if ((groups2 & MaskKx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._K);
            if ((groups2 & MaskQx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._Q);
            if ((groups2 & MaskJx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._J);
            if ((groups2 & MaskTx4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._T);
            if ((groups2 & Mask9x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._9);
            if ((groups2 & Mask8x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._8);
            if ((groups2 & Mask7x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._7);
            if ((groups2 & Mask6x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._6);
            if ((groups2 & Mask5x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._5);
            if ((groups2 & Mask4x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._4);
            if ((groups2 & Mask3x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._3);
            if ((groups2 & Mask2x4) != 0) return new WinCtx(WinKind.FullHouse, three, Value._2);

            // Сет
            Value rest0;
            if ((groups1 & MaskAx4) != 0) { rest0 = Value._A; goto ThreeRest_K; }
            if ((groups1 & MaskKx4) != 0) { rest0 = Value._K; goto ThreeRest_Q; }
            if ((groups1 & MaskQx4) != 0) { rest0 = Value._Q; goto ThreeRest_J; }
            if ((groups1 & MaskJx4) != 0) { rest0 = Value._J; goto ThreeRest_T; }
            if ((groups1 & MaskTx4) != 0) { rest0 = Value._T; goto ThreeRest_9; }
            if ((groups1 & Mask9x4) != 0) { rest0 = Value._9; goto ThreeRest_8; }
            if ((groups1 & Mask8x4) != 0) { rest0 = Value._8; goto ThreeRest_7; }
            if ((groups1 & Mask7x4) != 0) { rest0 = Value._7; goto ThreeRest_6; }
            if ((groups1 & Mask6x4) != 0) { rest0 = Value._6; goto ThreeRest_5; }
            if ((groups1 & Mask5x4) != 0) { rest0 = Value._5; goto ThreeRest_4; }
            if ((groups1 & Mask4x4) != 0) { rest0 = Value._4; goto ThreeRest_3; }
            if ((groups1 & Mask3x4) != 0) { rest0 = Value._3; goto ThreeRest_2; }
            throw new InvalidOperationException();

            ThreeRest_K: if ((groups1 & MaskKx4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._K);
            ThreeRest_Q: if ((groups1 & MaskQx4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._Q);
            ThreeRest_J: if ((groups1 & MaskJx4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._J);
            ThreeRest_T: if ((groups1 & MaskTx4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._T);
            ThreeRest_9: if ((groups1 & Mask9x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._9);
            ThreeRest_8: if ((groups1 & Mask8x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._8);
            ThreeRest_7: if ((groups1 & Mask7x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._7);
            ThreeRest_6: if ((groups1 & Mask6x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._6);
            ThreeRest_5: if ((groups1 & Mask5x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._5);
            ThreeRest_4: if ((groups1 & Mask4x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._4);
            ThreeRest_3: if ((groups1 & Mask3x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._3);
            ThreeRest_2: if ((groups1 & Mask2x4) != 0) return new WinCtx(WinKind.Three, three, rest0, Value._2);
            throw new InvalidOperationException();
        }

        // Пара / две пары
        if (groups2 != 0)
        {
            Value pair0;
            Value pair1;
            Value rest;
            if ((groups2 & MaskAx4) != 0) { pair0 = Value._A; goto TwoPairs1_K; }
            if ((groups2 & MaskKx4) != 0) { pair0 = Value._K; goto TwoPairs1_Q; }
            if ((groups2 & MaskQx4) != 0) { pair0 = Value._Q; goto TwoPairs1_J; }
            if ((groups2 & MaskJx4) != 0) { pair0 = Value._J; goto TwoPairs1_T; }
            if ((groups2 & MaskTx4) != 0) { pair0 = Value._T; goto TwoPairs1_9; }
            if ((groups2 & Mask9x4) != 0) { pair0 = Value._9; goto TwoPairs1_8; }
            if ((groups2 & Mask8x4) != 0) { pair0 = Value._8; goto TwoPairs1_7; }
            if ((groups2 & Mask7x4) != 0) { pair0 = Value._7; goto TwoPairs1_6; }
            if ((groups2 & Mask6x4) != 0) { pair0 = Value._6; goto TwoPairs1_5; }
            if ((groups2 & Mask5x4) != 0) { pair0 = Value._5; goto TwoPairs1_4; }
            if ((groups2 & Mask4x4) != 0) { pair0 = Value._4; goto TwoPairs1_3; }
            if ((groups2 & Mask3x4) != 0) { pair0 = Value._3; goto TwoPairs1_2; }
            if ((groups2 & Mask2x4) != 0) { pair0 = Value._2; goto Pair; }
            throw new InvalidOperationException();

            // Две пары
            TwoPairs1_K: if ((groups2 & MaskKx4) != 0) { pair1 = Value._K; goto TwoPairs2_Q; }
            TwoPairs1_Q: if ((groups2 & MaskQx4) != 0) { pair1 = Value._Q; goto TwoPairs2_J; }
            TwoPairs1_J: if ((groups2 & MaskJx4) != 0) { pair1 = Value._J; goto TwoPairs2_T; }
            TwoPairs1_T: if ((groups2 & MaskTx4) != 0) { pair1 = Value._T; goto TwoPairs2_9; }
            TwoPairs1_9: if ((groups2 & Mask9x4) != 0) { pair1 = Value._9; goto TwoPairs2_8; }
            TwoPairs1_8: if ((groups2 & Mask8x4) != 0) { pair1 = Value._8; goto TwoPairs2_7; }
            TwoPairs1_7: if ((groups2 & Mask7x4) != 0) { pair1 = Value._7; goto TwoPairs2_6; }
            TwoPairs1_6: if ((groups2 & Mask6x4) != 0) { pair1 = Value._6; goto TwoPairs2_5; }
            TwoPairs1_5: if ((groups2 & Mask5x4) != 0) { pair1 = Value._5; goto TwoPairs2_4; }
            TwoPairs1_4: if ((groups2 & Mask4x4) != 0) { pair1 = Value._4; goto TwoPairs2_3; }
            TwoPairs1_3: if ((groups2 & Mask3x4) != 0) { pair1 = Value._3; goto TwoPairs2_2; }
            TwoPairs1_2: if ((groups2 & Mask2x4) != 0) { pair1 = Value._2; goto TwoPairs2_1; }
            goto Pair;

            TwoPairs2_Q: if ((groups2 & MaskQx4) != 0) { rest = Value._Q; goto TwoPairs; }
            TwoPairs2_J: if ((groups2 & MaskJx4) != 0) { rest = Value._J; goto TwoPairs; }
            TwoPairs2_T: if ((groups2 & MaskTx4) != 0) { rest = Value._T; goto TwoPairs; }
            TwoPairs2_9: if ((groups2 & Mask9x4) != 0) { rest = Value._9; goto TwoPairs; }
            TwoPairs2_8: if ((groups2 & Mask8x4) != 0) { rest = Value._8; goto TwoPairs; }
            TwoPairs2_7: if ((groups2 & Mask7x4) != 0) { rest = Value._7; goto TwoPairs; }
            TwoPairs2_6: if ((groups2 & Mask6x4) != 0) { rest = Value._6; goto TwoPairs; }
            TwoPairs2_5: if ((groups2 & Mask5x4) != 0) { rest = Value._5; goto TwoPairs; }
            TwoPairs2_4: if ((groups2 & Mask4x4) != 0) { rest = Value._4; goto TwoPairs; }
            TwoPairs2_3: if ((groups2 & Mask3x4) != 0) { rest = Value._3; goto TwoPairs; }
            TwoPairs2_2: if ((groups2 & Mask2x4) != 0) { rest = Value._2; goto TwoPairs; }
            TwoPairs2_1: rest = Value._2;

            TwoPairs:
            if ((groups1 & MaskAx4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._A ? rest : Value._A);
            if ((groups1 & MaskKx4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._K ? rest : Value._K);
            if ((groups1 & MaskQx4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._Q ? rest : Value._Q);
            if ((groups1 & MaskJx4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._J ? rest : Value._J);
            if ((groups1 & MaskTx4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._T ? rest : Value._T);
            if ((groups1 & Mask9x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._9 ? rest : Value._9);
            if ((groups1 & Mask8x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._8 ? rest : Value._8);
            if ((groups1 & Mask7x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._7 ? rest : Value._7);
            if ((groups1 & Mask6x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._6 ? rest : Value._6);
            if ((groups1 & Mask5x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._5 ? rest : Value._5);
            if ((groups1 & Mask4x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._4 ? rest : Value._4);
            if ((groups1 & Mask3x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._3 ? rest : Value._3);
            if ((groups1 & Mask2x4) != 0) return new WinCtx(WinKind.TwoPairs, pair0, pair1, rest > Value._2 ? rest : Value._2);
            throw new InvalidOperationException();

            // Пара
            Pair:
            Value rest0;
            Value rest1;
            if ((groups1 & MaskAx4) != 0) { rest0 = Value._A; goto PairRest1_K; }
            if ((groups1 & MaskKx4) != 0) { rest0 = Value._K; goto PairRest1_Q; }
            if ((groups1 & MaskQx4) != 0) { rest0 = Value._Q; goto PairRest1_J; }
            if ((groups1 & MaskJx4) != 0) { rest0 = Value._J; goto PairRest1_T; }
            if ((groups1 & MaskTx4) != 0) { rest0 = Value._T; goto PairRest1_9; }
            if ((groups1 & Mask9x4) != 0) { rest0 = Value._9; goto PairRest1_8; }
            if ((groups1 & Mask8x4) != 0) { rest0 = Value._8; goto PairRest1_7; }
            if ((groups1 & Mask7x4) != 0) { rest0 = Value._7; goto PairRest1_6; }
            if ((groups1 & Mask6x4) != 0) { rest0 = Value._6; goto PairRest1_5; }
            if ((groups1 & Mask5x4) != 0) { rest0 = Value._5; goto PairRest1_4; }
            if ((groups1 & Mask4x4) != 0) { rest0 = Value._4; goto PairRest1_3; }
            throw new InvalidOperationException();

            PairRest1_K: if ((groups1 & MaskKx4) != 0) { rest1 = Value._K; goto PairRest2_Q; }
            PairRest1_Q: if ((groups1 & MaskQx4) != 0) { rest1 = Value._Q; goto PairRest2_J; }
            PairRest1_J: if ((groups1 & MaskJx4) != 0) { rest1 = Value._J; goto PairRest2_T; }
            PairRest1_T: if ((groups1 & MaskTx4) != 0) { rest1 = Value._T; goto PairRest2_9; }
            PairRest1_9: if ((groups1 & Mask9x4) != 0) { rest1 = Value._9; goto PairRest2_8; }
            PairRest1_8: if ((groups1 & Mask8x4) != 0) { rest1 = Value._8; goto PairRest2_7; }
            PairRest1_7: if ((groups1 & Mask7x4) != 0) { rest1 = Value._7; goto PairRest2_6; }
            PairRest1_6: if ((groups1 & Mask6x4) != 0) { rest1 = Value._6; goto PairRest2_5; }
            PairRest1_5: if ((groups1 & Mask5x4) != 0) { rest1 = Value._5; goto PairRest2_4; }
            PairRest1_4: if ((groups1 & Mask4x4) != 0) { rest1 = Value._4; goto PairRest2_3; }
            PairRest1_3: if ((groups1 & Mask3x4) != 0) { rest1 = Value._3; goto PairRest2_2; }
            throw new InvalidOperationException();

            PairRest2_Q: if ((groups1 & MaskQx4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._Q);
            PairRest2_J: if ((groups1 & MaskJx4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._J);
            PairRest2_T: if ((groups1 & MaskTx4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._T);
            PairRest2_9: if ((groups1 & Mask9x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._9);
            PairRest2_8: if ((groups1 & Mask8x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._8);
            PairRest2_7: if ((groups1 & Mask7x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._7);
            PairRest2_6: if ((groups1 & Mask6x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._6);
            PairRest2_5: if ((groups1 & Mask5x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._5);
            PairRest2_4: if ((groups1 & Mask4x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._4);
            PairRest2_3: if ((groups1 & Mask3x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._3);
            PairRest2_2: if ((groups1 & Mask2x4) != 0) return new WinCtx(WinKind.Pair, pair0, rest0, rest1, Value._2);
            throw new InvalidOperationException();
        }

        // Старшая карта
        var singleValues = new Value[7];
        var singleValuesIndex = 0;
        if ((groups1 & MaskAx4) != 0) singleValues[singleValuesIndex++] = Value._A;
        if ((groups1 & MaskKx4) != 0) singleValues[singleValuesIndex++] = Value._K;
        if ((groups1 & MaskQx4) != 0) singleValues[singleValuesIndex++] = Value._Q;
        if ((groups1 & MaskJx4) != 0) singleValues[singleValuesIndex++] = Value._J;
        if ((groups1 & MaskTx4) != 0) singleValues[singleValuesIndex++] = Value._T;
        if ((groups1 & Mask9x4) != 0) singleValues[singleValuesIndex++] = Value._9;
        if ((groups1 & Mask8x4) != 0) singleValues[singleValuesIndex++] = Value._8;
        if ((groups1 & Mask7x4) != 0) singleValues[singleValuesIndex++] = Value._7;
        if ((groups1 & Mask6x4) != 0) singleValues[singleValuesIndex++] = Value._6;
        if ((groups1 & Mask5x4) != 0) singleValues[singleValuesIndex++] = Value._5;
        if ((groups1 & Mask4x4) != 0) singleValues[singleValuesIndex++] = Value._4;
        if ((groups1 & Mask3x4) != 0) singleValues[singleValuesIndex++] = Value._3;
        if ((groups1 & Mask2x4) != 0) singleValues[singleValuesIndex++] = Value._2;

        return new WinCtx(WinKind.Single, singleValues[0], singleValues[1], singleValues[2], singleValues[3], singleValues[4]);
    }

    private readonly struct WinCtx
    {
        public WinKind Kind { get; }
        public Value Value0 { get; }
        public Value Value1 { get; }
        public Value Value2 { get; }
        public Value Value3 { get; }
        public Value Value4 { get; }

        public WinCtx(
            WinKind kind,
            Value value0,
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

    private enum WinKind : byte
    {
        Single,
        Pair,
        TwoPairs,
        Three,
        Straight,
        Flush,
        FullHouse,
        Four,
        StraightFlush
    }
}

internal enum Winner
{
    Player,
    Casino,
    Split
}
