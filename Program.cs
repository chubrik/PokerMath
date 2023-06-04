using PokerMath;

if (args.Length != 1)
    throw new ArgumentOutOfRangeException(nameof(args));

switch (args[0])
{
    case "benchmark":
        Benchmark.Start();
        return;

    case "bruteforce":
        Btuteforce.Start();
        return;

    case "game":
        Game.Start();
        return;

    default:
        throw new ArgumentOutOfRangeException(nameof(args));
}
