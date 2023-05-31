using PokerMath;
using System.Diagnostics;
using static PokerMath.Constants;
using static PokerMath.Utils;

const int FlopGameCount = 1070190; // (47 * 46 / 2) * (45 * 44 / 2)

foreach (var player in AllHands)
{
    var allCards = GetAllCards();
    var usageMap = allCards.ToDictionary(x => x, x => false);

    foreach (var card in player)
        usageMap[allCards.Single(x => x.Value == card.Value && x.Suit == card.Suit)] = true;

    var board = new Card[5];
    var casino = new Card[2];

    // Флоп
    for (var i0 = 0; i0 < AllCardsCount - 2; i0++)
    {
        var card0 = allCards[i0];
        if (usageMap[card0]) continue;
        usageMap[card0] = true;
        board[0] = card0;

        for (var i1 = i0 + 1; i1 < AllCardsCount - 1; i1++)
        {
            var card1 = allCards[i1];
            if (usageMap[card1]) continue;
            usageMap[card1] = true;
            board[1] = card1;

            for (var i2 = i1 + 1; i2 < AllCardsCount; i2++)
            {
                var card2 = allCards[i2];
                if (usageMap[card2]) continue;
                usageMap[card2] = true;
                board[2] = card2;

                var winCount = 0;
                var loseCount = 0;
                var splitCount = 0;

                // Тёрн + ривер
                for (var i3 = 0; i3 < AllCardsCount - 1; i3++)
                {
                    var card3 = allCards[i3];
                    if (usageMap[card3]) continue;
                    usageMap[card3] = true;
                    board[3] = card3;

                    for (var i4 = i3 + 1; i4 < AllCardsCount; i4++)
                    {
                        var card4 = allCards[i4];
                        if (usageMap[card4]) continue;
                        usageMap[card4] = true;
                        board[4] = card4;

                        // Казино
                        for (var i5 = 0; i5 < AllCardsCount - 1; i5++)
                        {
                            var card5 = allCards[i5];
                            if (usageMap[card5]) continue;
                            //usageMap[card5] = true;
                            casino[0] = card5;

                            for (var i6 = i5 + 1; i6 < AllCardsCount; i6++)
                            {
                                var card6 = allCards[i6];
                                if (usageMap[card6]) continue;
                                //usageMap[card6] = true;
                                casino[1] = card6;

                                // Игра
                                var winner = GetWinner(player, casino, board);
                                if (winner == Winner.Player) winCount++;
                                else if (winner == Winner.Casino) loseCount++;
                                else splitCount++;

                                //card6.InUse = false;
                            }

                            //card5.InUse = false;
                        }

                        usageMap[card4] = false;
                    }

                    usageMap[card3] = false;
                }

                usageMap[card2] = false;

                Debug.Assert(winCount + loseCount + splitCount == FlopGameCount);

                Console.WriteLine(
                    $"{player[0]} {player[1]} - " +
                    $"{board[0]} {board[1]} {board[2]} ... - " +
                    $"wins: {winCount / (float)(FlopGameCount - splitCount):0.00000}, " +
                    $"ties: {splitCount / (float)FlopGameCount:0.00000}");
            }

            usageMap[card1] = false;
        }

        usageMap[card0] = false;
    }
}
