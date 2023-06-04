namespace PokerMath;

using System.Collections;

public static class Extensions
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

    public static string Bits(this ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        var bits = new BitArray(bytes);

        string b(int index) => bits?[index] == true ? "1" : "0";

        return $"{b(63)}{b(62)}{b(61)}{b(60)}{b(59)}{b(58)}{b(57)}{b(56)}{b(55)}_" +
               $"{b(54)}{b(53)}{b(52)}{b(51)}_{b(50)}{b(49)}{b(48)}{b(47)}_" +
               $"{b(46)}{b(45)}{b(44)}{b(43)}_{b(42)}{b(41)}{b(40)}{b(39)}_" +
               $"{b(38)}{b(37)}{b(36)}_{b(35)}{b(34)}{b(33)}_{b(32)}{b(31)}{b(30)}_" +
               $"{b(29)}{b(28)}{b(27)}_{b(26)}{b(25)}{b(24)}_{b(23)}{b(22)}{b(21)}_" +
               $"{b(20)}{b(19)}{b(18)}_{b(17)}{b(16)}{b(15)}_{b(14)}{b(13)}{b(12)}_" +
               $"{b(11)}{b(10)}{b(9)}_{b(8)}{b(7)}{b(6)}_{b(5)}{b(4)}{b(3)}_" +
               $"{b(2)}{b(1)}{b(0)}";
    }
}
