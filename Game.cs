namespace PokerMath;

using System.Diagnostics;
using System.Reflection;
using static PokerMath.Constants;

internal class Game
{
    private const float MaxChanceToFold = 1f / 3;
    private static bool _isChancesInitialized = false;
    private static readonly float[] _allChances = new float[3312400]; // 169 * 19600
    private static readonly int[] _chancesBoard0Shifts = new int[48];

    public static void Start()
    {
        NewGame:
        Console.Clear();
        Console.WriteLine("W`Welcome to Speed Poker game!");
        Console.WriteLine();
        Console.WriteLine(["C`1.", " Real game"]);
        Console.WriteLine(["C`2.", " Gambling - show after fold"]);
        Console.WriteLine("d`3. Training - analyse your moves");
        Console.WriteLine(["C`4.", " Cheating - hint of chances"]);
        Console.WriteLine();
        Console.Write(["Select ", "C`[1]", ", ", "C`[2]", ", ", "d`[3]", " or ", "C`[4]", " mode: "]);
        var showFolded = false;
        var hintChances = false;

        for (; ; )
        {
            var key = Console.ReadKey(intercept: true).KeyChar;

            if (key == '1')
            {
                Console.WriteLine("W`1");
                break;
            }

            if (key == '2')
            {
                Console.WriteLine("W`2");
                showFolded = true;
                break;
            }

            if (key == '4')
            {
                Console.WriteLine("W`4");
                showFolded = true;
                hintChances = true;
                InitChances();
                break;
            }
        }

        Console.WriteLine();
        Console.WriteLine("d` deal raise   win   your   your                     casino");
        Console.WriteLine("d`count  rate  rate  stack   hand    board            hand    result");
        Console.WriteLine();
        var dealCount = 0;
        var raiseCount = 0;
        float raiseRate;
        float winRate;
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
            deck.Pop();
            board[0] = deck.Pop();
            board[1] = deck.Pop();
            board[2] = deck.Pop();

            var pos = Console.CursorPosition;
            raiseRate = dealCount > 0 ? Math.Min(0.999f, (float)raiseCount / dealCount) : 0.5f;
            winRate = dealCount > 0 ? Math.Min(0.999f, (float)(stack - 100) / 6 / dealCount + 0.5f) : 0.5f;
            var stackPad = "".PadLeft(Math.Max(0, 5 - stack.ToString().Length));
            dealCount++;

            if (dealCount > 1)
                Console.WriteLine();

            Console.WriteLine([
                $"d`{dealCount,5}  {raiseRate:.000}  {winRate:.000} ",
                $"{(stack < 20 ? 'r' : 'd')}`{stackPad}$", $"{(stack < 20 ? 'R' : 'W')}`{stack}   ",
                "d`", Val(player[0]), Sut(player[0]), " ", Val(player[1]), Sut(player[1]), "   ",
                "d`", Val(board[0]), Sut(board[0]), " ", Val(board[1]), Sut(board[1]), " ", Val(board[2]), Sut(board[2]), " ",
                "d`·· ··   ·· ··"]);

            Console.WriteLine();

            var raise = false;
            var autoRaise = false;

            if (hintChances)
            {
                var chance = GetChance(player, board);
                autoRaise = chance > MaxChanceToFold;

                Console.Write([
                    "Select ",
                    $"{(autoRaise ? "Wg" : "C")}`[R]", "aise, ",
                    $"{(!autoRaise ? "Wr" : "C")}`[F]", "old or ",
                    "C`[A]", $"uto (", $"{(autoRaise ? 'g' : 'r')}`{chance:.000}", " chance): "]);
            }
            else
                Console.Write(["Select ", "C`[R]", "aise or ", "C`[F]", "old: "]);

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

                if (hintChances && key == ConsoleKey.A)
                {
                    raise = autoRaise;
                    break;
                }

                if (key == ConsoleKey.Escape)
                    goto NewGame;
            }

            if (raise)
                raiseCount++;

            deck.Pop();
            board[3] = deck.Pop();
            deck.Pop();
            board[4] = deck.Pop();
            var winCtx = Utils.GetWinCtx(player, casino, board);

            pos = pos.Write([
                $"d`{dealCount,5}  {raiseRate:.000}  {winRate:.000} ",
                $"{(stack < 20 ? 'r' : 'd')}`{stackPad}$", $"{(stack < 20 ? 'R' : 'W')}`{stack}   ",
                "d`", Val(player[0]), Sut(player[0]), " ", Val(player[1]), Sut(player[1]), "   ",
                "d`", Val(board[0]), Sut(board[0]), " ", Val(board[1]), Sut(board[1]), " ", Val(board[2]), Sut(board[2]), " "]);

            if (raise)
            {
                pos = pos.Write([
                    Val(board[3]), Sut(board[3]), " ", Val(board[4]), Sut(board[4]), "   ",
                    Val(casino[0]), Sut(casino[0]), " ", Val(casino[1]), Sut(casino[1]), "   "]);

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
                        $"d`{board[3]} {board[4]}   " +
                        $"{casino[0]} {casino[1]}   ");

                    if (winCtx.Winner == Winner.Player)
                        pos.Write(["r`fold", $"d` (would win by {WinReason(winCtx)})"]);
                    else if (winCtx.Winner == Winner.Casino)
                        pos.Write(["g`fold", $"d` (would lose by {WinReason(winCtx)})"]);
                    else
                        pos.Write(["r`fold", $"d` (would split {WinReason(winCtx)})"]);
                }
                else
                    pos.Write("d`·· ··   ·· ··   fold");
            }

            pos.Write("\n                                                         ");
            Console.Write("\r                                                ");
            Console.CursorPosition = new(0, Console.CursorTop - (dealCount == 1 ? 1 : 2));
        }

        raiseRate = Math.Min(0.999f, (float)raiseCount / dealCount);
        winRate = (float)(stack - 100) / 6 / dealCount + 0.5f;
        Console.WriteLine([$"d`       {raiseRate:.000}  {winRate:.000}     ", $"r`$", $"R`{stack}"]);
        Console.WriteLine();
        Console.WriteLine("WR` Game over ");
        Console.WriteLine("R`You’ve lost all your money.");
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

    private static float GetChance(IReadOnlyList<Card> player, IReadOnlyList<Card> board)
    {
        Debug.Assert(player.Count == 2);
        Debug.Assert(board.Count == 5);

        if (!_isChancesInitialized)
            throw new InvalidOperationException();

        var suitReplace = new Dictionary<Suit, Suit>
        {
            { Suit.Spades, Suit.Spades },
            { Suit.Hearts, Suit.Hearts },
            { Suit.Diamonds, Suit.Diamonds },
            { Suit.Clubs, Suit.Clubs },
        };

        var sortedPlayer = player.OrderByDescending(x => x.Value).ToList();
        var value0 = (int)sortedPlayer[0].Value;
        var value1 = (int)sortedPlayer[1].Value;

        var handIndex = value0 == value1
            ? 14 - value0
            : 13 + 78 - (value0 - 1) * (value0 - 2) / 2 + (value0 - 1 - value1);

        suitReplace[Suit.Spades] = sortedPlayer[0].Suit;
        suitReplace[sortedPlayer[0].Suit] = Suit.Spades;

        if (sortedPlayer[0].Suit != sortedPlayer[1].Suit)
        {
            if (value0 != value1)
                handIndex += 78;

            suitReplace[suitReplace[Suit.Hearts]] = suitReplace[sortedPlayer[1].Suit];
            suitReplace[sortedPlayer[1].Suit] = Suit.Hearts;
        }

        var normPlayer = sortedPlayer.Select(x => new Card(x.Value, suitReplace[x.Suit]))
                                     .ToList();

        var normBoard = board.Take(3)
                             .Select(x => new Card(x.Value, suitReplace[x.Suit]))
                             .OrderByDescending(x => x.Value)
                             .ThenByDescending(x => x.Suit)
                             .ToList();

        Debug.Assert(AllHands[handIndex][0].Value == normPlayer[0].Value);
        Debug.Assert(AllHands[handIndex][0].Suit == normPlayer[0].Suit);
        Debug.Assert(AllHands[handIndex][1].Value == normPlayer[1].Value);
        Debug.Assert(AllHands[handIndex][1].Suit == normPlayer[1].Suit);

        var player0num = GetCardNum(normPlayer[0]);
        var player1num = GetCardNum(normPlayer[1]);
        var board0num = GetCardNum(normBoard[0]);
        var board1num = GetCardNum(normBoard[1]);
        var board2num = GetCardNum(normBoard[2]);
        Debug.Assert(player0num < player1num);
        Debug.Assert(board0num < board1num);
        Debug.Assert(board1num < board2num);

        var player0isTaken = false;
        var player1isTaken = false;

        var board0Index = board0num;
        var board0VarCount = 48;
        if (board0num > player0num) { board0Index--; player0isTaken = true; }
        if (board0num > player1num) { board0Index--; player1isTaken = true; }
        Debug.Assert(board0Index >= 0 && board0Index < board0VarCount);

        var board1Index = board1num - board0num - 1;
        var board1VarCount = 48 - board0Index;
        if (board1num > player0num && !player0isTaken) { board1Index--; player0isTaken = true; }
        if (board1num > player1num && !player1isTaken) { board1Index--; player1isTaken = true; }
        Debug.Assert(board1Index >= 0 && board1Index < board1VarCount);

        var board2Index = board2num - board1num - 1;
        var board2VarCount = 48 - board1Index;
        if (board2num > player0num && !player0isTaken) board2Index--;
        if (board2num > player1num && !player1isTaken) board2Index--;
        Debug.Assert(board2Index >= 0 && board2Index < board2VarCount);

        var board12VarCount = board1VarCount * (board1VarCount - 1) / 2;
        var handShift = handIndex * FlopCount;
        var board0Shift = _chancesBoard0Shifts[board0Index];
        var board1Shift = board12VarCount - (board1VarCount - board1Index) * (board1VarCount - board1Index - 1) / 2 + board1Index;
        var board2Shift = board2Index;
        var chanceIndex = handShift + board0Shift + board1Shift + board2Shift;

        var chance = _allChances[chanceIndex];
        //var chanceByBruteforce = GetChanceByBruteforce(player, board.ToList());
        //Debug.Assert(chance == chanceByBruteforce);
        return chance;
    }

    private static void InitChances()
    {
        if (_isChancesInitialized)
            return;

        var assembly = Assembly.GetExecutingAssembly() ?? throw new InvalidOperationException();
        var resourceNamePrefix = assembly.GetName().Name + ".Resources.";
        var allChancesIndex = 0;

        for (var handIndex = 0; handIndex < AllHands.Count; handIndex++)
        {
            var resourceName = resourceNamePrefix + handIndex.ToString().PadLeft(3, '0') + ".txt";
            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException();
            using var reader = new StreamReader(stream);
            var allText = reader.ReadToEnd();
            var lines = allText.TrimEnd().Split("\r\n");
            Debug.Assert(lines.Length == FlopCount);

            for (var flopIndex = 0; flopIndex < FlopCount; flopIndex++)
            {
                var line = lines[flopIndex];
                var commaIndex = line.IndexOf(',');
                var winCount = int.Parse(line[..commaIndex]);
                var splitCount = int.Parse(line[(commaIndex + 1)..]);
                var loseCount = FlopDialCount - winCount - splitCount;
                var chance = (float)winCount / (winCount + loseCount);
                _allChances[allChancesIndex++] = chance;
            }
        }

        var prevBoard0Shift = FlopCount;
        var board12Count = 0;

        for (var i = 0; i < _chancesBoard0Shifts.Length; i++)
        {
            board12Count += i + 1;
            var board0Shift = prevBoard0Shift - board12Count;
            _chancesBoard0Shifts[_chancesBoard0Shifts.Length - i - 1] = board0Shift;
            prevBoard0Shift = board0Shift;
        }

        _isChancesInitialized = true;
    }

    private static float GetChanceByBruteforce(IReadOnlyList<Card> player, List<Card> board)
    {
        Debug.Assert(player.Count == 2);
        Debug.Assert(board.Count == 5);

        var allCards = AllCards;
        var usageMap = new bool[67];

        foreach (var card in player)
            usageMap[allCards.Select(x => x.Index).Single(x => x == card.Index)] = true;

        foreach (var card in board.Take(3))
            usageMap[allCards.Select(x => x.Index).Single(x => x == card.Index)] = true;

        var casino = new Card[2];
        var count = 0L;
        var winCount = 0;
        var loseCount = 0;
        var splitCount = 0;

        // Тёрн + ривер
        for (var i3 = 0; i3 < AllCardsCount - 1; i3++)
        {
            var card3 = allCards[i3];
            if (usageMap[card3.Index]) continue;
            usageMap[card3.Index] = true;
            board[3] = card3;

            for (var i4 = i3 + 1; i4 < AllCardsCount; i4++)
            {
                var card4 = allCards[i4];
                if (usageMap[card4.Index]) continue;
                usageMap[card4.Index] = true;
                board[4] = card4;

                // Казино
                for (var i5 = 0; i5 < AllCardsCount - 1; i5++)
                {
                    var card5 = allCards[i5];
                    if (usageMap[card5.Index]) continue;
                    casino[0] = card5;

                    for (var i6 = i5 + 1; i6 < AllCardsCount; i6++)
                    {
                        var card6 = allCards[i6];
                        if (usageMap[card6.Index]) continue;
                        casino[1] = card6;

                        // Игра
                        var winner = Utils.GetWinCtx(player, casino, board).Winner;
                        if (winner == Winner.Player) winCount++;
                        else if (winner == Winner.Casino) loseCount++;
                        else splitCount++;

                        count++;
                    }
                }

                usageMap[card4.Index] = false;
            }

            usageMap[card3.Index] = false;
        }

        Debug.Assert(winCount + loseCount + splitCount == FlopDialCount);
        var chance = (float)winCount / (FlopDialCount - splitCount);
        return chance;
    }

    private static int GetCardNum(Card card)
    {
        var num = (14 - (int)card.Value) * 4 + (4 - (int)card.Suit);
        Debug.Assert(AllCards[num].Value == card.Value);
        Debug.Assert(AllCards[num].Suit == card.Suit);
        return num;
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
            WinKind.HighCard => "high-card",
            _ => throw new InvalidOperationException()
        };
    }
}
