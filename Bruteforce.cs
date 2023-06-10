namespace PokerMath;

using System.Diagnostics;
using static PokerMath.Constants;

internal static class Bruteforce
{
    public const int FlopGameCount = 1070190; // (47 * 46 / 2) * (45 * 44 / 2)

    public static void Start()
    {
        for (var handIndex = 0; handIndex < AllHands.Count; handIndex++)
        {
            if (File.Exists($"{handIndex.ToString().PadLeft(3, '0')}.txt"))
                continue;

            var player = AllHands[handIndex];
            var allCards = AllCards;
            var usageMap = new bool[67];

            foreach (var card in player)
                usageMap[allCards.Select(x => x.Index).Single(x => x == card.Index)] = true;

            var board = new Card[5];
            var casino = new Card[2];
            var prevTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Флоп
            for (var i0 = 0; i0 < AllCardsCount - 2; i0++)
            {
                var card0 = allCards[i0];
                if (usageMap[card0.Index]) continue;
                usageMap[card0.Index] = true;
                board[0] = card0;

                for (var i1 = i0 + 1; i1 < AllCardsCount - 1; i1++)
                {
                    var card1 = allCards[i1];
                    if (usageMap[card1.Index]) continue;
                    usageMap[card1.Index] = true;
                    board[1] = card1;

                    for (var i2 = i1 + 1; i2 < AllCardsCount; i2++)
                    {
                        var card2 = allCards[i2];
                        if (usageMap[card2.Index]) continue;
                        usageMap[card2.Index] = true;
                        board[2] = card2;

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
                                    }
                                }

                                usageMap[card4.Index] = false;
                            }

                            usageMap[card3.Index] = false;
                        }

                        usageMap[card2.Index] = false;

                        Debug.Assert(winCount + loseCount + splitCount == FlopGameCount);

                        var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        var elapsed = time - prevTime;
                        prevTime = time;

                        Console.WriteLine(
                            $"{player[0]} {player[1]} - " +
                            $"{board[0]} {board[1]} {board[2]} ... - " +
                            $"wins: {winCount / (float)(FlopGameCount - splitCount):0.00000}, ",
                            $"d`ties: {splitCount / (float)FlopGameCount:0.00000}, " +
                            $"time: {elapsed} ms");

                        File.AppendAllText(
                            $"{handIndex.ToString().PadLeft(3, '0')}.txt",
                            $"{winCount},{splitCount}\r\n");
                    }

                    usageMap[card1.Index] = false;
                }

                usageMap[card0.Index] = false;
            }
        }
    }
}
