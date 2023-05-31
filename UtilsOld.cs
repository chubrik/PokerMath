namespace PokerMath;

using System.Diagnostics;

internal class UtilsOld
{
    public static Winner GetWinner(IReadOnlyList<Card> player, IReadOnlyList<Card> casino, IReadOnlyList<Card> board)
    {
        Debug.Assert(player.Count == 2);
        Debug.Assert(casino.Count == 2);
        Debug.Assert(board.Count == 5);

        var playerCtx = GetCtx(playerOrCasino: player, board: board);
        var casinoCtx = GetCtx(playerOrCasino: casino, board: board);

        // Стрит-флеш

        if (playerCtx.StraightFlush.Count != 0)
        {
            if (casinoCtx.StraightFlush.Count != 0)
                return playerCtx.StraightFlush[0].Value == casinoCtx.StraightFlush[0].Value ? Winner.Split
                    : playerCtx.StraightFlush[0].Value > casinoCtx.StraightFlush[0].Value ? Winner.Player : Winner.Casino;
            else
                return Winner.Player;
        }
        else if (casinoCtx.StraightFlush.Count != 0)
            return Winner.Casino;

        // Каре

        if (playerCtx.Group1.Count == 4)
        {
            if (casinoCtx.Group1.Count == 4)
            {
                if (playerCtx.Group1[0].Value != casinoCtx.Group1[0].Value)
                    return playerCtx.Group1[0].Value > casinoCtx.Group1[0].Value ? Winner.Player : Winner.Casino;

                return playerCtx.Rest[0].Value == casinoCtx.Rest[0].Value ? Winner.Split
                    : playerCtx.Rest[0].Value > casinoCtx.Rest[0].Value ? Winner.Player : Winner.Casino;
            }
            else
                return Winner.Player;
        }
        else if (casinoCtx.Group1.Count == 4)
            return Winner.Casino;

        // Фулл-хаус

        if (playerCtx.Group1.Count == 3 && playerCtx.Group2.Count == 2)
        {
            if (casinoCtx.Group1.Count == 3 && casinoCtx.Group2.Count == 2)
            {
                if (playerCtx.Group1[0].Value == casinoCtx.Group1[0].Value)
                    return playerCtx.Group2[0].Value == casinoCtx.Group2[0].Value ? Winner.Split
                        : playerCtx.Group2[0].Value > casinoCtx.Group2[0].Value ? Winner.Player : Winner.Casino;
                else
                    return playerCtx.Group1[0].Value > casinoCtx.Group1[0].Value ? Winner.Player : Winner.Casino;
            }
            else
                return Winner.Player;
        }
        else if (casinoCtx.Group1.Count == 3 && casinoCtx.Group2.Count == 2)
            return Winner.Casino;

        // Флеш

        if (playerCtx.Flush.Count != 0)
        {
            if (casinoCtx.Flush.Count != 0)
            {
                for (var i = 0; i < playerCtx.Flush.Count; i++)
                    if (playerCtx.Flush[i].Value != casinoCtx.Flush[i].Value)
                        return playerCtx.Flush[i].Value > casinoCtx.Flush[i].Value ? Winner.Player : Winner.Casino;

                return Winner.Split;
            }
            else
                return Winner.Player;
        }
        else if (casinoCtx.Flush.Count != 0)
            return Winner.Casino;

        // Стрит

        if (playerCtx.Straight.Count != 0)
        {
            if (casinoCtx.Straight.Count != 0)
                return playerCtx.Straight[0].Value == casinoCtx.Straight[0].Value ? Winner.Split
                    : playerCtx.Straight[0].Value > casinoCtx.Straight[0].Value ? Winner.Player : Winner.Casino;
            else
                return Winner.Player;
        }
        else if (casinoCtx.Straight.Count != 0)
            return Winner.Casino;

        // Сет

        if (playerCtx.Group1.Count == 3)
        {
            Debug.Assert(playerCtx.Group2.Count == 0);
            Debug.Assert(playerCtx.Rest.Count == 2);

            if (casinoCtx.Group1.Count == 3)
            {
                Debug.Assert(casinoCtx.Group2.Count == 0);
                Debug.Assert(casinoCtx.Rest.Count == 2);

                if (playerCtx.Group1[0].Value == casinoCtx.Group1[0].Value)
                {
                    if (playerCtx.Rest[0].Value == casinoCtx.Rest[0].Value)
                        return playerCtx.Rest[1].Value == casinoCtx.Rest[1].Value ? Winner.Split
                            : playerCtx.Rest[1].Value > casinoCtx.Rest[1].Value ? Winner.Player : Winner.Casino;
                    else
                        return playerCtx.Rest[0].Value > casinoCtx.Rest[0].Value ? Winner.Player : Winner.Casino;
                }
                else
                    return playerCtx.Group1[0].Value > casinoCtx.Group1[0].Value ? Winner.Player : Winner.Casino;
            }
            else
                return Winner.Player;
        }
        else if (casinoCtx.Group1.Count == 3)
        {
            Debug.Assert(casinoCtx.Group2.Count == 0);
            Debug.Assert(casinoCtx.Rest.Count == 2);
            return Winner.Casino;
        }

        // Две пары

        if (playerCtx.Group1.Count == 2 && playerCtx.Group2.Count == 2)
        {
            Debug.Assert(playerCtx.Rest.Count == 1);

            if (casinoCtx.Group1.Count == 2 && casinoCtx.Group2.Count == 2)
            {
                Debug.Assert(casinoCtx.Rest.Count == 1);

                if (playerCtx.Group1[0].Value == casinoCtx.Group1[0].Value)
                {
                    if (playerCtx.Group2[0].Value == casinoCtx.Group2[0].Value)
                        return playerCtx.Rest[0].Value == casinoCtx.Rest[0].Value ? Winner.Split
                            : playerCtx.Rest[0].Value > casinoCtx.Rest[0].Value ? Winner.Player : Winner.Casino;
                    else
                        return playerCtx.Group2[0].Value > casinoCtx.Group2[0].Value ? Winner.Player : Winner.Casino;
                }
                else
                    return playerCtx.Group1[0].Value > casinoCtx.Group1[0].Value ? Winner.Player : Winner.Casino;
            }
            else
                return Winner.Player;
        }
        else if (casinoCtx.Group1.Count == 2 && casinoCtx.Group2.Count == 2)
        {
            Debug.Assert(casinoCtx.Rest.Count == 1);
            return Winner.Casino;
        }

        // Пара

        if (playerCtx.Group1.Count == 2)
        {
            Debug.Assert(playerCtx.Group2.Count == 0);
            Debug.Assert(playerCtx.Rest.Count == 3);

            if (casinoCtx.Group1.Count == 2)
            {
                Debug.Assert(casinoCtx.Group2.Count == 0);
                Debug.Assert(casinoCtx.Rest.Count == 3);

                if (playerCtx.Group1[0].Value == casinoCtx.Group1[0].Value)
                {
                    for (var i = 0; i < playerCtx.Rest.Count; i++)
                        if (playerCtx.Rest[i].Value != casinoCtx.Rest[i].Value)
                            return playerCtx.Rest[i].Value > casinoCtx.Rest[i].Value ? Winner.Player : Winner.Casino;

                    return Winner.Split;
                }
                else
                    return playerCtx.Group1[0].Value > casinoCtx.Group1[0].Value ? Winner.Player : Winner.Casino;
            }
            else
                return Winner.Player;
        }
        else if (casinoCtx.Group1.Count == 2)
        {
            Debug.Assert(casinoCtx.Group2.Count == 0);
            Debug.Assert(casinoCtx.Rest.Count == 3);
            return Winner.Casino;
        }

        // Старшая карта

        Debug.Assert(playerCtx.Rest.Count == 5);
        Debug.Assert(casinoCtx.Rest.Count == 5);

        for (var i = 0; i < playerCtx.Rest.Count; i++)
            if (playerCtx.Rest[i].Value != casinoCtx.Rest[i].Value)
                return playerCtx.Rest[i].Value > casinoCtx.Rest[i].Value ? Winner.Player : Winner.Casino;

        return Winner.Split;
    }

    private static WinCtx GetCtx(IReadOnlyList<Card> playerOrCasino, IReadOnlyList<Card> board)
    {
        const int lastCardIndex = 6;
        var cards = playerOrCasino.Concat(board).OrderByDescending(x => x.Value).ToList();
        var spades = new List<Card>(7);
        var hearts = new List<Card>(7);
        var diamonds = new List<Card>(7);
        var clubs = new List<Card>(7);
        var straightFlush = new List<Card>(5);
        var straight = new List<Card>(5) { cards[0] };
        var flush = new List<Card>(5);
        var groups = new List<List<Card>>(7) { new List<Card>(4) { cards[0] } };
        var maxGroupSize = 1;
        var maxGroupIndex = 0;

        for (var i = 0; i <= lastCardIndex; i++)
        {
            var card = cards[i];

            var suitList = card.Suit switch
            {
                Suit.Spades => spades,
                Suit.Hearts => hearts,
                Suit.Diamonds => diamonds,
                Suit.Clubs => clubs,
                _ => throw new InvalidOperationException()
            };

            suitList.Add(card);

            if (suitList.Count == 5)
                flush = suitList;

            if (i == 0)
                continue;

            var diff = cards[i - 1].Value - card.Value;

            if (diff == 0)
            {
                var groupIndex = groups.Count - 1;
                var group = groups[groupIndex];
                group.Add(card);

                if (group.Count > maxGroupSize)
                {
                    maxGroupSize++;
                    maxGroupIndex = groupIndex;
                }
            }
            else
            {
                groups.Add(new List<Card>(4) { card });

                if (diff == 1)
                {
                    if (straight.Count != 5)
                        straight.Add(card);
                }
                else if (straight.Count != 5)
                {
                    straight.Clear();
                    straight.Add(card);
                }
            }
        }

        if (straight.Count == 4 && straight[3].Value == Value._2 && cards[0].Value == Value._A)
            straight.Add(cards[0]);

        else if (straight.Count != 5)
            straight.Clear();

        Debug.Assert(maxGroupSize <= 4);
        IReadOnlyList<Card> group1 = Array.Empty<Card>();
        IReadOnlyList<Card> group2 = Array.Empty<Card>();
        var rest = new List<Card>(5);

        if (flush.Count >= 5)
        {
            straightFlush.Add(flush[0]);

            for (var i = 1; i < flush.Count; i++)
            {
                var card = flush[i];

                if (flush[i - 1].Value - card.Value == 1)
                {
                    straightFlush.Add(card);

                    if (straightFlush.Count == 5)
                        goto Return;
                }
                else
                {
                    straightFlush.Clear();
                    straightFlush.Add(card);
                }
            }

            if (straightFlush.Count == 4 && straightFlush[3].Value == Value._2 && flush[0].Value == Value._A)
                straightFlush.Add(flush[0]);

            else if (straightFlush.Count != 5)
                straightFlush.Clear();
        }

        if (maxGroupSize == 4)
        {
            group1 = groups[maxGroupIndex];
            rest = new List<Card> { groups[maxGroupIndex == 0 ? 1 : 0][0] };
        }
        else if (maxGroupSize == 3)
        {
            group1 = groups[maxGroupIndex];

            for (var i = 0; i < groups.Count; i++)
                if (i != maxGroupIndex)
                {
                    var group = groups[i];

                    if (group.Count == 3)
                    {
                        group2 = group.Take(2).ToList();
                        break;
                    }
                    else if (group.Count == 2)
                    {
                        group2 = group;
                        break;
                    }
                    else
                        rest.Add(group[0]);
                }

            rest = rest.Take(5 - group1.Count - group2.Count).ToList();
        }
        else if (maxGroupSize == 2)
        {
            group1 = groups[maxGroupIndex];

            for (var i = 0; i < groups.Count; i++)
                if (i != maxGroupIndex)
                {
                    var group = groups[i];

                    if (group.Count == 2 && group2.Count == 0)
                        group2 = group;
                    else
                        rest.Add(group[0]);
                }

            rest = rest.Take(5 - group1.Count - group2.Count).ToList();
        }
        else
        {
            group1 = Array.Empty<Card>();
            rest = cards.Take(5).ToList();
        }

    Return:

        flush = flush.Take(5).ToList();
        Debug.Assert(straightFlush.Count == 5 || straightFlush.Count == 0);
        Debug.Assert(flush.Count == 5 || flush.Count == 0);
        Debug.Assert(straight.Count == 5 || straight.Count == 0);
        Debug.Assert(group1.Count >= group2.Count);

        return new WinCtx(
            straightFlush: straightFlush,
            flush: flush,
            straight: straight,
            group1: group1,
            group2: group2,
            rest: rest
        );
    }

    private sealed class WinCtx
    {
        public IReadOnlyList<Card> StraightFlush;
        public IReadOnlyList<Card> Flush;
        public IReadOnlyList<Card> Straight;
        public IReadOnlyList<Card> Group1;
        public IReadOnlyList<Card> Group2;
        public IReadOnlyList<Card> Rest;

        public WinCtx(
            IReadOnlyList<Card> straightFlush,
            IReadOnlyList<Card> flush,
            IReadOnlyList<Card> straight,
            IReadOnlyList<Card> group1,
            IReadOnlyList<Card> group2,
            IReadOnlyList<Card> rest)
        {
            StraightFlush = straightFlush;
            Flush = flush;
            Straight = straight;
            Group1 = group1;
            Group2 = group2;
            Rest = rest;
        }
    }
}
