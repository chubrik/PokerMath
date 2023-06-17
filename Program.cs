using PokerMath;

if (args.Length != 1)
    throw new ArgumentOutOfRangeException(nameof(args));

switch (args[0])
{
    case "benchmark":
        Benchmark.Start();
        return;

    case "bruteforce":
        Bruteforce.Start();
        return;

    case "game":
        Game.Start();
        return;

    case "statistics":
        Statistics.Start();
        return;

    default:
        throw new ArgumentOutOfRangeException(nameof(args));
}
