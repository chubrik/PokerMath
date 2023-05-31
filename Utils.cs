namespace PokerMath;

using System.Diagnostics;

internal static class Utils
{
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

        if (playerCtx.ValueCount == 5)
        {
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

        if (playerCtx.ValueCount == 4)
        {
            if (playerCtx.Value0 != casinoCtx.Value0)
                return playerCtx.Value0 > casinoCtx.Value0 ? Winner.Player : Winner.Casino;

            if (playerCtx.Value1 != casinoCtx.Value1)
                return playerCtx.Value1 > casinoCtx.Value1 ? Winner.Player : Winner.Casino;

            if (playerCtx.Value2 != casinoCtx.Value2)
                return playerCtx.Value2 > casinoCtx.Value2 ? Winner.Player : Winner.Casino;

            return playerCtx.Value3 == casinoCtx.Value3 ? Winner.Split
                 : playerCtx.Value3 > casinoCtx.Value3 ? Winner.Player : Winner.Casino;
        }

        if (playerCtx.ValueCount == 3)
        {
            if (playerCtx.Value0 != casinoCtx.Value0)
                return playerCtx.Value0 > casinoCtx.Value0 ? Winner.Player : Winner.Casino;

            if (playerCtx.Value1 != casinoCtx.Value1)
                return playerCtx.Value1 > casinoCtx.Value1 ? Winner.Player : Winner.Casino;

            return playerCtx.Value2 == casinoCtx.Value2 ? Winner.Split
                 : playerCtx.Value2 > casinoCtx.Value2 ? Winner.Player : Winner.Casino;
        }

        if (playerCtx.ValueCount == 1)
            return playerCtx.Value0 == casinoCtx.Value0 ? Winner.Split
                : playerCtx.Value0 > casinoCtx.Value0 ? Winner.Player : Winner.Casino;

        if (playerCtx.ValueCount == 2)
        {
            if (playerCtx.Value0 != casinoCtx.Value0)
                return playerCtx.Value0 > casinoCtx.Value0 ? Winner.Player : Winner.Casino;

            return playerCtx.Value1 == casinoCtx.Value1 ? Winner.Split
                 : playerCtx.Value1 > casinoCtx.Value1 ? Winner.Player : Winner.Casino;
        }

        throw new InvalidOperationException();
    }

    private static WinCtx GetCtx(IReadOnlyList<Card> playerOrCasino, IReadOnlyList<Card> board)
    {
        //                                           Spad Hear Diam Club  A   K   Q   J   T   9   8   7   6   5   4   3   2
        const ulong CounterStart /* */ = 0b_00000000_0011_0011_0011_0011_000_000_000_000_000_000_000_000_000_000_000_000_000;
        const ulong HasFlushAMask /**/ = 0b_00000000_1000_1000_1000_1000_000_000_000_000_000_000_000_000_000_000_000_000_000;
        const ulong HasFlushBMask /**/ = 0b_00000000_1000_1000_0000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
        const ulong HasFlushCMask /**/ = 0b_00000000_1000_0000_1000_0000_000_000_000_000_000_000_000_000_000_000_000_000_000;
        const ulong StraightAMas2 /**/ = 0b_00000000_0000_0000_0000_0000_001_001_001_001_001_000_000_000_000_000_000_000_000;
        const ulong StraightKMas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_001_001_001_001_001_000_000_000_000_000_000_000;
        const ulong StraightQMas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_000_001_001_001_001_001_000_000_000_000_000_000;
        const ulong StraightJMas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_001_001_001_001_001_000_000_000_000_000;
        const ulong StraightTMas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_001_001_001_001_001_000_000_000_000;
        const ulong Straight9Mas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_001_001_001_001_001_000_000_000;
        const ulong Straight8Mas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_000_001_001_001_001_001_000_000;
        const ulong Straight7Mas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_000_000_001_001_001_001_001_000;
        const ulong Straight6Mas2 /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_001_001_001_001_001;
        const ulong Straight5Mas2 /**/ = 0b_00000000_0000_0000_0000_0000_001_000_000_000_000_000_000_000_000_001_001_001_001;
        const ulong StraightAMask /**/ = 0b_00000000_0000_0000_0000_0000_100_100_100_100_100_000_000_000_000_000_000_000_000;
        const ulong StraightKMask /**/ = 0b_00000000_0000_0000_0000_0000_000_100_100_100_100_100_000_000_000_000_000_000_000;
        const ulong StraightQMask /**/ = 0b_00000000_0000_0000_0000_0000_000_000_100_100_100_100_100_000_000_000_000_000_000;
        const ulong StraightJMask /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_100_100_100_100_100_000_000_000_000_000;
        const ulong StraightTMask /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_100_100_100_100_100_000_000_000_000;
        const ulong Straight9Mask /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_100_100_100_100_100_000_000_000;
        const ulong Straight8Mask /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_000_100_100_100_100_100_000_000;
        const ulong Straight7Mask /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_000_000_100_100_100_100_100_000;
        const ulong Straight6Mask /**/ = 0b_00000000_0000_0000_0000_0000_000_000_000_000_000_000_000_000_100_100_100_100_100;
        const ulong Straight5Mask /**/ = 0b_00000000_0000_0000_0000_0000_100_000_000_000_000_000_000_000_000_100_100_100_100;
        const ulong GroupMask /*    */ = 0b_00000000_0000_0000_0000_0000_100_100_100_100_100_100_100_100_100_100_100_100_100;
        const ulong GroupPlusMask /**/ = 0b_00000000_0000_0000_0000_0000_001_001_001_001_001_001_001_001_001_001_001_001_001;
        const ulong GroupAntiMask /**/ = 0b_00000000_0000_0000_0000_0000_011_011_011_011_011_011_011_011_011_011_011_011_011;

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
            if ((flush & StraightAMas2) == StraightAMas2) return WinCtx.StraightFlush(Value._A);
            if ((flush & StraightKMas2) == StraightKMas2) return WinCtx.StraightFlush(Value._K);
            if ((flush & StraightQMas2) == StraightQMas2) return WinCtx.StraightFlush(Value._Q);
            if ((flush & StraightJMas2) == StraightJMas2) return WinCtx.StraightFlush(Value._J);
            if ((flush & StraightTMas2) == StraightTMas2) return WinCtx.StraightFlush(Value._T);
            if ((flush & Straight9Mas2) == Straight9Mas2) return WinCtx.StraightFlush(Value._9);
            if ((flush & Straight8Mas2) == Straight8Mas2) return WinCtx.StraightFlush(Value._8);
            if ((flush & Straight7Mas2) == Straight7Mas2) return WinCtx.StraightFlush(Value._7);
            if ((flush & Straight6Mas2) == Straight6Mas2) return WinCtx.StraightFlush(Value._6);
            if ((flush & Straight5Mas2) == Straight5Mas2) return WinCtx.StraightFlush(Value._5);

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

            return WinCtx.Flush(flushValues);
        }

        var shiftCounter = counter;
        var groups4 = shiftCounter & GroupMask;
        shiftCounter = (shiftCounter & GroupAntiMask) + GroupPlusMask;
        var groups3 = shiftCounter & GroupMask;
        shiftCounter = (shiftCounter & GroupAntiMask) + GroupPlusMask;
        var groups2 = shiftCounter & GroupMask;
        shiftCounter = (shiftCounter & GroupAntiMask) + GroupPlusMask;
        var groups1 = shiftCounter & GroupMask;
        groups4 >>= 2;
        groups3 >>= 2;
        groups2 >>= 2;
        groups1 >>= 2;
        var groups321 = groups3 | groups2 | groups1;

        // Каре
        if (groups4 != 0)
        {
            var four = (groups4 & Card.MaskA) != 0 ? Value._A
                     : (groups4 & Card.MaskK) != 0 ? Value._K
                     : (groups4 & Card.MaskQ) != 0 ? Value._Q
                     : (groups4 & Card.MaskJ) != 0 ? Value._J
                     : (groups4 & Card.MaskT) != 0 ? Value._T
                     : (groups4 & Card.Mask9) != 0 ? Value._9
                     : (groups4 & Card.Mask8) != 0 ? Value._8
                     : (groups4 & Card.Mask7) != 0 ? Value._7
                     : (groups4 & Card.Mask6) != 0 ? Value._6
                     : (groups4 & Card.Mask5) != 0 ? Value._5
                     : (groups4 & Card.Mask4) != 0 ? Value._4
                     : (groups4 & Card.Mask3) != 0 ? Value._3
                     : (groups4 & Card.Mask2) != 0 ? Value._2
                     : throw new InvalidOperationException();

            if ((groups321 & Card.MaskA) != 0) return WinCtx.Four(four, Value._A);
            if ((groups321 & Card.MaskK) != 0) return WinCtx.Four(four, Value._K);
            if ((groups321 & Card.MaskQ) != 0) return WinCtx.Four(four, Value._Q);
            if ((groups321 & Card.MaskJ) != 0) return WinCtx.Four(four, Value._J);
            if ((groups321 & Card.MaskT) != 0) return WinCtx.Four(four, Value._T);
            if ((groups321 & Card.Mask9) != 0) return WinCtx.Four(four, Value._9);
            if ((groups321 & Card.Mask8) != 0) return WinCtx.Four(four, Value._8);
            if ((groups321 & Card.Mask7) != 0) return WinCtx.Four(four, Value._7);
            if ((groups321 & Card.Mask6) != 0) return WinCtx.Four(four, Value._6);
            if ((groups321 & Card.Mask5) != 0) return WinCtx.Four(four, Value._5);
            if ((groups321 & Card.Mask4) != 0) return WinCtx.Four(four, Value._4);
            if ((groups321 & Card.Mask3) != 0) return WinCtx.Four(four, Value._3);
            if ((groups321 & Card.Mask2) != 0) return WinCtx.Four(four, Value._2);
            throw new InvalidOperationException();
        }

        // Стрит
        var values = counter + GroupAntiMask;
        if ((values & StraightAMask) == StraightAMask) return WinCtx.Straight(Value._A);
        if ((values & StraightKMask) == StraightKMask) return WinCtx.Straight(Value._K);
        if ((values & StraightQMask) == StraightQMask) return WinCtx.Straight(Value._Q);
        if ((values & StraightJMask) == StraightJMask) return WinCtx.Straight(Value._J);
        if ((values & StraightTMask) == StraightTMask) return WinCtx.Straight(Value._T);
        if ((values & Straight9Mask) == Straight9Mask) return WinCtx.Straight(Value._9);
        if ((values & Straight8Mask) == Straight8Mask) return WinCtx.Straight(Value._8);
        if ((values & Straight7Mask) == Straight7Mask) return WinCtx.Straight(Value._7);
        if ((values & Straight6Mask) == Straight6Mask) return WinCtx.Straight(Value._6);
        if ((values & Straight5Mask) == Straight5Mask) return WinCtx.Straight(Value._5);

        // Сет / Фулл-хаус
        if (groups3 != 0)
        {
            Value three;
            if ((groups3 & Card.MaskA) != 0) { three = Value._A; goto FullHouse_K; }
            if ((groups3 & Card.MaskK) != 0) { three = Value._K; goto FullHouse_Q; }
            if ((groups3 & Card.MaskQ) != 0) { three = Value._Q; goto FullHouse_J; }
            if ((groups3 & Card.MaskJ) != 0) { three = Value._J; goto FullHouse_T; }
            if ((groups3 & Card.MaskT) != 0) { three = Value._T; goto FullHouse_9; }
            if ((groups3 & Card.Mask9) != 0) { three = Value._9; goto FullHouse_8; }
            if ((groups3 & Card.Mask8) != 0) { three = Value._8; goto FullHouse_7; }
            if ((groups3 & Card.Mask7) != 0) { three = Value._7; goto FullHouse_6; }
            if ((groups3 & Card.Mask6) != 0) { three = Value._6; goto FullHouse_5; }
            if ((groups3 & Card.Mask5) != 0) { three = Value._5; goto FullHouse_4; }
            if ((groups3 & Card.Mask4) != 0) { three = Value._4; goto FullHouse_3; }
            if ((groups3 & Card.Mask3) != 0) { three = Value._3; goto FullHouse_2; }
            if ((groups3 & Card.Mask2) != 0) { three = Value._2; goto FullHouse_1; }
            throw new InvalidOperationException();

            // Фулл-хаус
            FullHouse_K: if ((groups3 & Card.MaskK) != 0) return WinCtx.FullHouse(three, Value._K);
            FullHouse_Q: if ((groups3 & Card.MaskQ) != 0) return WinCtx.FullHouse(three, Value._Q);
            FullHouse_J: if ((groups3 & Card.MaskJ) != 0) return WinCtx.FullHouse(three, Value._J);
            FullHouse_T: if ((groups3 & Card.MaskT) != 0) return WinCtx.FullHouse(three, Value._T);
            FullHouse_9: if ((groups3 & Card.Mask9) != 0) return WinCtx.FullHouse(three, Value._9);
            FullHouse_8: if ((groups3 & Card.Mask8) != 0) return WinCtx.FullHouse(three, Value._8);
            FullHouse_7: if ((groups3 & Card.Mask7) != 0) return WinCtx.FullHouse(three, Value._7);
            FullHouse_6: if ((groups3 & Card.Mask6) != 0) return WinCtx.FullHouse(three, Value._6);
            FullHouse_5: if ((groups3 & Card.Mask5) != 0) return WinCtx.FullHouse(three, Value._5);
            FullHouse_4: if ((groups3 & Card.Mask4) != 0) return WinCtx.FullHouse(three, Value._4);
            FullHouse_3: if ((groups3 & Card.Mask3) != 0) return WinCtx.FullHouse(three, Value._3);
            FullHouse_2: if ((groups3 & Card.Mask2) != 0) return WinCtx.FullHouse(three, Value._2);
            FullHouse_1:

            if ((groups2 & Card.MaskA) != 0) return WinCtx.FullHouse(three, Value._A);
            if ((groups2 & Card.MaskK) != 0) return WinCtx.FullHouse(three, Value._K);
            if ((groups2 & Card.MaskQ) != 0) return WinCtx.FullHouse(three, Value._Q);
            if ((groups2 & Card.MaskJ) != 0) return WinCtx.FullHouse(three, Value._J);
            if ((groups2 & Card.MaskT) != 0) return WinCtx.FullHouse(three, Value._T);
            if ((groups2 & Card.Mask9) != 0) return WinCtx.FullHouse(three, Value._9);
            if ((groups2 & Card.Mask8) != 0) return WinCtx.FullHouse(three, Value._8);
            if ((groups2 & Card.Mask7) != 0) return WinCtx.FullHouse(three, Value._7);
            if ((groups2 & Card.Mask6) != 0) return WinCtx.FullHouse(three, Value._6);
            if ((groups2 & Card.Mask5) != 0) return WinCtx.FullHouse(three, Value._5);
            if ((groups2 & Card.Mask4) != 0) return WinCtx.FullHouse(three, Value._4);
            if ((groups2 & Card.Mask3) != 0) return WinCtx.FullHouse(three, Value._3);
            if ((groups2 & Card.Mask2) != 0) return WinCtx.FullHouse(three, Value._2);

            // Сет
            Value rest0;
            if ((groups1 & Card.MaskA) != 0) { rest0 = Value._A; goto ThreeRest_K; }
            if ((groups1 & Card.MaskK) != 0) { rest0 = Value._K; goto ThreeRest_Q; }
            if ((groups1 & Card.MaskQ) != 0) { rest0 = Value._Q; goto ThreeRest_J; }
            if ((groups1 & Card.MaskJ) != 0) { rest0 = Value._J; goto ThreeRest_T; }
            if ((groups1 & Card.MaskT) != 0) { rest0 = Value._T; goto ThreeRest_9; }
            if ((groups1 & Card.Mask9) != 0) { rest0 = Value._9; goto ThreeRest_8; }
            if ((groups1 & Card.Mask8) != 0) { rest0 = Value._8; goto ThreeRest_7; }
            if ((groups1 & Card.Mask7) != 0) { rest0 = Value._7; goto ThreeRest_6; }
            if ((groups1 & Card.Mask6) != 0) { rest0 = Value._6; goto ThreeRest_5; }
            if ((groups1 & Card.Mask5) != 0) { rest0 = Value._5; goto ThreeRest_4; }
            if ((groups1 & Card.Mask4) != 0) { rest0 = Value._4; goto ThreeRest_3; }
            if ((groups1 & Card.Mask3) != 0) { rest0 = Value._3; goto ThreeRest_2; }
            throw new InvalidOperationException();

            ThreeRest_K: if ((groups1 & Card.MaskK) != 0) return WinCtx.Three(three, rest0, Value._K);
            ThreeRest_Q: if ((groups1 & Card.MaskQ) != 0) return WinCtx.Three(three, rest0, Value._Q);
            ThreeRest_J: if ((groups1 & Card.MaskJ) != 0) return WinCtx.Three(three, rest0, Value._J);
            ThreeRest_T: if ((groups1 & Card.MaskT) != 0) return WinCtx.Three(three, rest0, Value._T);
            ThreeRest_9: if ((groups1 & Card.Mask9) != 0) return WinCtx.Three(three, rest0, Value._9);
            ThreeRest_8: if ((groups1 & Card.Mask8) != 0) return WinCtx.Three(three, rest0, Value._8);
            ThreeRest_7: if ((groups1 & Card.Mask7) != 0) return WinCtx.Three(three, rest0, Value._7);
            ThreeRest_6: if ((groups1 & Card.Mask6) != 0) return WinCtx.Three(three, rest0, Value._6);
            ThreeRest_5: if ((groups1 & Card.Mask5) != 0) return WinCtx.Three(three, rest0, Value._5);
            ThreeRest_4: if ((groups1 & Card.Mask4) != 0) return WinCtx.Three(three, rest0, Value._4);
            ThreeRest_3: if ((groups1 & Card.Mask3) != 0) return WinCtx.Three(three, rest0, Value._3);
            ThreeRest_2: if ((groups1 & Card.Mask2) != 0) return WinCtx.Three(three, rest0, Value._2);
            throw new InvalidOperationException();
        }

        // Пара / две пары
        if (groups2 != 0)
        {
            Value pair0;
            Value pair1;
            Value rest;
            if ((groups2 & Card.MaskA) != 0) { pair0 = Value._A; goto TwoPairs1_K; }
            if ((groups2 & Card.MaskK) != 0) { pair0 = Value._K; goto TwoPairs1_Q; }
            if ((groups2 & Card.MaskQ) != 0) { pair0 = Value._Q; goto TwoPairs1_J; }
            if ((groups2 & Card.MaskJ) != 0) { pair0 = Value._J; goto TwoPairs1_T; }
            if ((groups2 & Card.MaskT) != 0) { pair0 = Value._T; goto TwoPairs1_9; }
            if ((groups2 & Card.Mask9) != 0) { pair0 = Value._9; goto TwoPairs1_8; }
            if ((groups2 & Card.Mask8) != 0) { pair0 = Value._8; goto TwoPairs1_7; }
            if ((groups2 & Card.Mask7) != 0) { pair0 = Value._7; goto TwoPairs1_6; }
            if ((groups2 & Card.Mask6) != 0) { pair0 = Value._6; goto TwoPairs1_5; }
            if ((groups2 & Card.Mask5) != 0) { pair0 = Value._5; goto TwoPairs1_4; }
            if ((groups2 & Card.Mask4) != 0) { pair0 = Value._4; goto TwoPairs1_3; }
            if ((groups2 & Card.Mask3) != 0) { pair0 = Value._3; goto TwoPairs1_2; }
            if ((groups2 & Card.Mask2) != 0) { pair0 = Value._2; goto Pair; }
            throw new InvalidOperationException();

            // Две пары
            TwoPairs1_K: if ((groups2 & Card.MaskK) != 0) { pair1 = Value._K; goto TwoPairs2_Q; }
            TwoPairs1_Q: if ((groups2 & Card.MaskQ) != 0) { pair1 = Value._Q; goto TwoPairs2_J; }
            TwoPairs1_J: if ((groups2 & Card.MaskJ) != 0) { pair1 = Value._J; goto TwoPairs2_T; }
            TwoPairs1_T: if ((groups2 & Card.MaskT) != 0) { pair1 = Value._T; goto TwoPairs2_9; }
            TwoPairs1_9: if ((groups2 & Card.Mask9) != 0) { pair1 = Value._9; goto TwoPairs2_8; }
            TwoPairs1_8: if ((groups2 & Card.Mask8) != 0) { pair1 = Value._8; goto TwoPairs2_7; }
            TwoPairs1_7: if ((groups2 & Card.Mask7) != 0) { pair1 = Value._7; goto TwoPairs2_6; }
            TwoPairs1_6: if ((groups2 & Card.Mask6) != 0) { pair1 = Value._6; goto TwoPairs2_5; }
            TwoPairs1_5: if ((groups2 & Card.Mask5) != 0) { pair1 = Value._5; goto TwoPairs2_4; }
            TwoPairs1_4: if ((groups2 & Card.Mask4) != 0) { pair1 = Value._4; goto TwoPairs2_3; }
            TwoPairs1_3: if ((groups2 & Card.Mask3) != 0) { pair1 = Value._3; goto TwoPairs2_2; }
            TwoPairs1_2: if ((groups2 & Card.Mask2) != 0) { pair1 = Value._2; goto TwoPairs2_1; }
            goto Pair;

            TwoPairs2_Q: if ((groups2 & Card.MaskQ) != 0) { rest = Value._Q; goto TwoPairs; }
            TwoPairs2_J: if ((groups2 & Card.MaskJ) != 0) { rest = Value._J; goto TwoPairs; }
            TwoPairs2_T: if ((groups2 & Card.MaskT) != 0) { rest = Value._T; goto TwoPairs; }
            TwoPairs2_9: if ((groups2 & Card.Mask9) != 0) { rest = Value._9; goto TwoPairs; }
            TwoPairs2_8: if ((groups2 & Card.Mask8) != 0) { rest = Value._8; goto TwoPairs; }
            TwoPairs2_7: if ((groups2 & Card.Mask7) != 0) { rest = Value._7; goto TwoPairs; }
            TwoPairs2_6: if ((groups2 & Card.Mask6) != 0) { rest = Value._6; goto TwoPairs; }
            TwoPairs2_5: if ((groups2 & Card.Mask5) != 0) { rest = Value._5; goto TwoPairs; }
            TwoPairs2_4: if ((groups2 & Card.Mask4) != 0) { rest = Value._4; goto TwoPairs; }
            TwoPairs2_3: if ((groups2 & Card.Mask3) != 0) { rest = Value._3; goto TwoPairs; }
            TwoPairs2_2: if ((groups2 & Card.Mask2) != 0) { rest = Value._2; goto TwoPairs; }
            TwoPairs2_1: rest = Value._2;

            TwoPairs:
            if ((groups1 & Card.MaskA) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._A ? rest : Value._A);
            if ((groups1 & Card.MaskK) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._K ? rest : Value._K);
            if ((groups1 & Card.MaskQ) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._Q ? rest : Value._Q);
            if ((groups1 & Card.MaskJ) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._J ? rest : Value._J);
            if ((groups1 & Card.MaskT) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._T ? rest : Value._T);
            if ((groups1 & Card.Mask9) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._9 ? rest : Value._9);
            if ((groups1 & Card.Mask8) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._8 ? rest : Value._8);
            if ((groups1 & Card.Mask7) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._7 ? rest : Value._7);
            if ((groups1 & Card.Mask6) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._6 ? rest : Value._6);
            if ((groups1 & Card.Mask5) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._5 ? rest : Value._5);
            if ((groups1 & Card.Mask4) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._4 ? rest : Value._4);
            if ((groups1 & Card.Mask3) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._3 ? rest : Value._3);
            if ((groups1 & Card.Mask2) != 0) return WinCtx.TwoPairs(pair0, pair1, rest > Value._2 ? rest : Value._2);
            throw new InvalidOperationException();

            // Пара
            Pair:
            Value rest0;
            Value rest1;
            if ((groups1 & Card.MaskA) != 0) { rest0 = Value._A; goto PairRest1_K; }
            if ((groups1 & Card.MaskK) != 0) { rest0 = Value._K; goto PairRest1_Q; }
            if ((groups1 & Card.MaskQ) != 0) { rest0 = Value._Q; goto PairRest1_J; }
            if ((groups1 & Card.MaskJ) != 0) { rest0 = Value._J; goto PairRest1_T; }
            if ((groups1 & Card.MaskT) != 0) { rest0 = Value._T; goto PairRest1_9; }
            if ((groups1 & Card.Mask9) != 0) { rest0 = Value._9; goto PairRest1_8; }
            if ((groups1 & Card.Mask8) != 0) { rest0 = Value._8; goto PairRest1_7; }
            if ((groups1 & Card.Mask7) != 0) { rest0 = Value._7; goto PairRest1_6; }
            if ((groups1 & Card.Mask6) != 0) { rest0 = Value._6; goto PairRest1_5; }
            if ((groups1 & Card.Mask5) != 0) { rest0 = Value._5; goto PairRest1_4; }
            if ((groups1 & Card.Mask4) != 0) { rest0 = Value._4; goto PairRest1_3; }
            throw new InvalidOperationException();

            PairRest1_K: if ((groups1 & Card.MaskK) != 0) { rest1 = Value._K; goto PairRest2_Q; }
            PairRest1_Q: if ((groups1 & Card.MaskQ) != 0) { rest1 = Value._Q; goto PairRest2_J; }
            PairRest1_J: if ((groups1 & Card.MaskJ) != 0) { rest1 = Value._J; goto PairRest2_T; }
            PairRest1_T: if ((groups1 & Card.MaskT) != 0) { rest1 = Value._T; goto PairRest2_9; }
            PairRest1_9: if ((groups1 & Card.Mask9) != 0) { rest1 = Value._9; goto PairRest2_8; }
            PairRest1_8: if ((groups1 & Card.Mask8) != 0) { rest1 = Value._8; goto PairRest2_7; }
            PairRest1_7: if ((groups1 & Card.Mask7) != 0) { rest1 = Value._7; goto PairRest2_6; }
            PairRest1_6: if ((groups1 & Card.Mask6) != 0) { rest1 = Value._6; goto PairRest2_5; }
            PairRest1_5: if ((groups1 & Card.Mask5) != 0) { rest1 = Value._5; goto PairRest2_4; }
            PairRest1_4: if ((groups1 & Card.Mask4) != 0) { rest1 = Value._4; goto PairRest2_3; }
            PairRest1_3: if ((groups1 & Card.Mask3) != 0) { rest1 = Value._3; goto PairRest2_2; }
            throw new InvalidOperationException();

            PairRest2_Q: if ((groups1 & Card.MaskQ) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._Q);
            PairRest2_J: if ((groups1 & Card.MaskJ) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._J);
            PairRest2_T: if ((groups1 & Card.MaskT) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._T);
            PairRest2_9: if ((groups1 & Card.Mask9) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._9);
            PairRest2_8: if ((groups1 & Card.Mask8) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._8);
            PairRest2_7: if ((groups1 & Card.Mask7) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._7);
            PairRest2_6: if ((groups1 & Card.Mask6) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._6);
            PairRest2_5: if ((groups1 & Card.Mask5) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._5);
            PairRest2_4: if ((groups1 & Card.Mask4) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._4);
            PairRest2_3: if ((groups1 & Card.Mask3) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._3);
            PairRest2_2: if ((groups1 & Card.Mask2) != 0) return WinCtx.Pair(pair0, rest0, rest1, Value._2);
            throw new InvalidOperationException();
        }

        // Старшая карта
        var singleValues = new Value[7];
        var singleValuesIndex = 0;
        if ((groups1 & Card.MaskA) != 0) singleValues[singleValuesIndex++] = Value._A;
        if ((groups1 & Card.MaskK) != 0) singleValues[singleValuesIndex++] = Value._K;
        if ((groups1 & Card.MaskQ) != 0) singleValues[singleValuesIndex++] = Value._Q;
        if ((groups1 & Card.MaskJ) != 0) singleValues[singleValuesIndex++] = Value._J;
        if ((groups1 & Card.MaskT) != 0) singleValues[singleValuesIndex++] = Value._T;
        if ((groups1 & Card.Mask9) != 0) singleValues[singleValuesIndex++] = Value._9;
        if ((groups1 & Card.Mask8) != 0) singleValues[singleValuesIndex++] = Value._8;
        if ((groups1 & Card.Mask7) != 0) singleValues[singleValuesIndex++] = Value._7;
        if ((groups1 & Card.Mask6) != 0) singleValues[singleValuesIndex++] = Value._6;
        if ((groups1 & Card.Mask5) != 0) singleValues[singleValuesIndex++] = Value._5;
        if ((groups1 & Card.Mask4) != 0) singleValues[singleValuesIndex++] = Value._4;
        if ((groups1 & Card.Mask3) != 0) singleValues[singleValuesIndex++] = Value._3;
        if ((groups1 & Card.Mask2) != 0) singleValues[singleValuesIndex++] = Value._2;

        return WinCtx.Single(singleValues);
    }

    private readonly struct WinCtx
    {
        public WinKind Kind { get; }
        public byte ValueCount { get; }
        public Value Value0 { get; }
        public Value Value1 { get; }
        public Value Value2 { get; }
        public Value Value3 { get; }
        public Value Value4 { get; }

        private WinCtx(WinKind kind, Value value0)
        {
            Kind = kind;
            ValueCount = 1;
            Value0 = value0;
        }

        private WinCtx(WinKind kind, Value value0, Value value1)
        {
            Kind = kind;
            ValueCount = 2;
            Value0 = value0;
            Value1 = value1;
        }

        private WinCtx(WinKind kind, Value value0, Value value1, Value value2)
        {
            Kind = kind;
            ValueCount = 3;
            Value0 = value0;
            Value1 = value1;
            Value2 = value2;
        }

        private WinCtx(WinKind kind, Value value0, Value value1, Value value2, Value value3)
        {
            Kind = kind;
            ValueCount = 4;
            Value0 = value0;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        private WinCtx(WinKind kind, Value value0, Value value1, Value value2, Value value3, Value value4)
        {
            Kind = kind;
            ValueCount = 5;
            Value0 = value0;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }

        public static WinCtx Single(IReadOnlyList<Value> values)
        {
            Debug.Assert(values.Count >= 5);
            return new(WinKind.Single, values[0], values[1], values[2], values[3], values[4]);
        }

        public static WinCtx Pair(Value pair, Value rest0, Value rest1, Value rest2)
        {
            return new(WinKind.Pair, value0: pair, value1: rest0, value2: rest1, value3: rest2);
        }

        public static WinCtx TwoPairs(Value pair0, Value pair1, Value rest)
        {
            return new(WinKind.TwoPairs, value0: pair0, value1: pair1, value2: rest);
        }

        public static WinCtx Three(Value three, Value rest0, Value rest1)
        {
            return new(WinKind.Three, value0: three, value1: rest0, value2: rest1);
        }

        public static WinCtx Straight(Value major)
        {
            return new(WinKind.Straight, value0: major);
        }

        public static WinCtx Flush(IReadOnlyList<Value> values)
        {
            Debug.Assert(values.Count >= 5);
            return new(WinKind.Flush, values[0], values[1], values[2], values[3], values[4]);
        }

        public static WinCtx FullHouse(Value tree, Value two)
        {
            return new(WinKind.FullHouse, value0: tree, value1: two);
        }

        public static WinCtx Four(Value four, Value rest)
        {
            return new(WinKind.Four, value0: four, value1: rest);
        }

        public static WinCtx StraightFlush(Value major)
        {
            return new(WinKind.StraightFlush, value0: major);
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
