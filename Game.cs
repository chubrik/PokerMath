namespace PokerMath;

internal class Game
{
    public static void Start()
    {
        NewGame:
        var showFolded = false;
        Console.Clear();
        Console.WriteLine("W`Welcome to Speed Poker game!");
        Console.WriteLine();
        Console.WriteLine("C`1.", " Normal game");
        Console.WriteLine("C`2.", " Show after fold");
        Console.WriteLine();
        Console.Write("Select ", "C`[1]", " or ", "C`[2]", " mode: ");

        for (; ; )
        {
            var key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.D1)
            {
                Console.WriteLine("W`1");
                break;
            }

            if (key == ConsoleKey.D2)
            {
                Console.WriteLine("W`2");
                showFolded = true;
                break;
            }
        }

        Console.WriteLine();
        Console.WriteLine("d` deal    win    your    your                       casino");
        Console.WriteLine("d`count   rate   stack    hand     board             hand     result");
        Console.WriteLine();
        var dealCount = 0;
        var stack = 100;

        while (stack >= 3)
        {
            var deck = new Deck();
            var player = new Card[2];
            var casino = new Card[2];
            var board = new Card[5];

            player[0] = deck.Pop();
            casino[0] = deck.Pop();
            player[1] = deck.Pop();
            casino[1] = deck.Pop();
            board[0] = deck.Pop();
            board[1] = deck.Pop();
            board[2] = deck.Pop();

            var stackPad = "".PadLeft(Math.Max(0, 5 - stack.ToString().Length));
            var pos = Console.CursorPosition;

            var rate = (dealCount > 0 ? (float)(stack - 100) / 6 / dealCount : 0) + 0.5;
            dealCount++;

            if (dealCount > 1)
                Console.WriteLine();

            Console.WriteLine(
                $"d` {dealCount,4}   {rate:0.000} ",
                $"{(stack < 20 ? 'r' : 'd')}`{stackPad}$", $"{(stack < 20 ? 'R' : 'W')}`{stack}    ",
                "d`", Val(player[0]), Sut(player[0]), " ", Val(player[1]), Sut(player[1]), "    ",
                "d`", Val(board[0]), Sut(board[0]), " ", Val(board[1]), Sut(board[1]), " ", Val(board[2]), Sut(board[2]), " ",
                "d`·· ··    ·· ··");

            Console.WriteLine();
            Console.Write("Select ", "C`[R]", "aise or ", "C`[F]", "old: ");
            var raise = false;

            for (; ; )
            {
                var key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.R)
                {
                    raise = true;
                    break;
                }

                if (key == ConsoleKey.F)
                    break;

                if (key == ConsoleKey.Escape)
                    goto NewGame;
            }

            board[3] = deck.Pop();
            board[4] = deck.Pop();
            var winCtx = Utils.GetWinCtx(player, casino, board);

            pos = pos.Write(
                $"d` {dealCount,4}   {rate:0.000} ",
                $"{(stack < 20 ? 'r' : 'd')}`{stackPad}$", $"{(stack < 20 ? 'R' : 'W')}`{stack}    ",
                "d`", Val(player[0]), Sut(player[0]), " ", Val(player[1]), Sut(player[1]), "    ",
                "d`", Val(board[0]), Sut(board[0]), " ", Val(board[1]), Sut(board[1]), " ", Val(board[2]), Sut(board[2]), " ");

            if (raise)
            {
                pos = pos.Write(
                    Val(board[3]), Sut(board[3]), " ", Val(board[4]), Sut(board[4]), "    ",
                    Val(casino[0]), Sut(casino[0]), " ", Val(casino[1]), Sut(casino[1]), "    ");

                if (winCtx.Winner == Winner.Player)
                {
                    stack += 3;
                    pos.Write($"G`win by {WinReason(winCtx)}");
                }
                else if (winCtx.Winner == Winner.Casino)
                {
                    stack -= 3;
                    pos.Write($"R`lose by {WinReason(winCtx)}");
                }
                else
                    pos.Write($"Y`split {WinReason(winCtx)}");
            }
            else
            {
                stack--;

                if (showFolded)
                {
                    pos = pos.Write(
                        $"d`{board[3]} {board[4]}    " +
                        $"{casino[0]} {casino[1]}    ",
                        "d`fold ");

                    if (winCtx.Winner == Winner.Player)
                        pos.Write($"r`(would win by {WinReason(winCtx)})");
                    else if (winCtx.Winner == Winner.Casino)
                        pos.Write($"g`(would lose by {WinReason(winCtx)})");
                    else
                        pos.Write($"y`(would split {WinReason(winCtx)})");
                }
                else
                    pos.Write("d`·· ··    ·· ··    fold");
            }

            pos.Write("\n                                                        ");
            Console.Write("\r                         ");
            Console.CursorPosition = new(0, Console.CursorTop - (dealCount == 1 ? 1 : 2));
        }

        Console.WriteLine($"r`                  $", $"R`{stack}");
        Console.WriteLine("WR` Game over ");
        Console.WriteLine("R`You’ve lost all your money!");
        Console.WriteLine();
        Console.Write("Repeat? [y/n]: ");

        for (; ; )
        {
            var key = Console.ReadKey(intercept: true).Key;

            if (key == ConsoleKey.Y)
                goto NewGame;

            if (key == ConsoleKey.N)
            {
                Console.WriteLine("R`No");
                break;
            }
        }
    }

    private static string Val(Card card)
    {
        var value = card.Value switch
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

        return $"W`{value}";
    }

    private static string Sut(Card card)
    {
        var color = card.Suit switch
        {
            Suit.Spades => "Y",
            Suit.Hearts => "R",
            Suit.Diamonds => "B",
            Suit.Clubs => "G",
            _ => throw new InvalidOperationException()
        };

        var suit = card.Suit switch
        {
            Suit.Spades => "♠",
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            _ => throw new InvalidOperationException()
        };

        return $"{color}`{suit}";
    }

    private static string WinReason(WinCtx ctx)
    {
        var player = ctx.Player;
        var casino = ctx.Casino;
        var winnerOrSplitCtx = ctx.Winner == Winner.Player ? player : casino;

        return winnerOrSplitCtx.Kind switch
        {
            WinKind.RoyalFlush => "royal-flush",
            WinKind.StraightFlush => "streight-flush",
            WinKind.FourOfKind => "four-of-kind",
            WinKind.FullHouse => "full-house",
            WinKind.Flush => "flush",
            WinKind.Straight => "straight",
            WinKind.ThreeOfKind => "three-of-kind",
            WinKind.TwoPair => "two-pair",
            WinKind.Pair => "pair",
            WinKind.HighCaed => "high-card",
            _ => throw new InvalidOperationException()
        };
    }
}
