namespace PokerMath;

public static class ListExtensions
{
    private static readonly Random _random = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        var count = list.Count;
        var lastIndex = count - 1;
        int newIndex;

        for (var index = 0; index < lastIndex; index++)
        {
            newIndex = _random.Next(index, count);
            (list[index], list[newIndex]) = (list[newIndex], list[index]);
        }
    }
}
