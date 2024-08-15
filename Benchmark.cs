namespace PokerMath;

using System.Diagnostics;
using static PokerMath.Constants;

internal static class Benchmark
{
    private const int TickPerSec = 10000000;
    private const int DialGroupCount = 1000000;
    private const int Million = 1000000;

    public static void Start()
    {
        var cards = AllCards.ToArray();
        var dialCount = 0;
        var msPerDialSum = 0d;

        for (var groupCount = 0; ; groupCount++)
        {
            var ctxs = new List<Ctx>(DialGroupCount);

            for (var i = 0; i < DialGroupCount; i++)
            {
                cards.Shuffle();
                var player = new[] { cards[0], cards[1] };
                var dealer = new[] { cards[2], cards[3] };
                var board = new[] { cards[4], cards[5], cards[6], cards[7], cards[8] };
                var ctx = new Ctx(player, dealer, board);
                ctxs.Add(ctx);
            }

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < DialGroupCount; i++)
            {
                var ctx = ctxs[i];
                Utils.GetWinCtx(ctx.Player, ctx.Dealer, ctx.Board);
            }

            var elapsedMs = sw.Elapsed;
            var msPerDial = (double)TickPerSec * DialGroupCount / elapsedMs.Ticks / Million;
            dialCount += DialGroupCount;

            var dialCountStr = ((double)dialCount / Million).ToString("0.0").PadLeft(4);
            var log = $"{dialCountStr} M   {msPerDial:0.000} M/s";

            if (groupCount > 0)
            {
                msPerDialSum += msPerDial;
                log += $"   {msPerDialSum / groupCount:0.000} M/s";
            }

            Console.WriteLine(log);
        }
    }

    private sealed class Ctx
    {
        public IReadOnlyList<Card> Player { get; }
        public IReadOnlyList<Card> Dealer { get; }
        public IReadOnlyList<Card> Board { get; }

        public Ctx(IReadOnlyList<Card> player, IReadOnlyList<Card> dealer, IReadOnlyList<Card> board)
        {
            Debug.Assert(player.Count == 2);
            Debug.Assert(dealer.Count == 2);
            Debug.Assert(board.Count == 5);

            Player = player;
            Dealer = dealer;
            Board = board;
        }
    }
}
