namespace PokerMath;

using System.Diagnostics;
using System.Reflection;
using System.Text;
using static PokerMath.Constants;

internal static class Statistics
{
    public static void Start()
    {
        Proceed("hand-chances", 0.4);
    }

    public static void Proceed(string statisticsName, double flopChancePsychoAllowed)
    {
        var resources = LoadResources();
        var table = new Ctx[13, 13];

        for (int handIndex = 0; handIndex < resources.Count; handIndex++)
        {
            var hand = AllHands[handIndex];
            var resource = resources[handIndex];
            var title = Val(hand[0]) + Val(hand[1]);
            int rowIndex;
            int columnIndex;

            if (hand[0].Suit == hand[1].Suit)
            {
                title += 's';
                rowIndex = 14 - (int)hand[0].Value;
                columnIndex = 14 - (int)hand[1].Value;
            }
            else
            {
                if (hand[0].Value != hand[1].Value)
                    title += 'o';

                rowIndex = 14 - (int)hand[1].Value;
                columnIndex = 14 - (int)hand[0].Value;
            }

            var chanceMathSum = 0.0;
            var chancePsychoSum = 0.0;

            foreach (var (winCount, splitCount) in resource)
            {
                var flopChance = (double)winCount / (FlopDialCount - splitCount);
                chanceMathSum += flopChance;

                if (flopChance >= flopChancePsychoAllowed)
                    chancePsychoSum += flopChance;
            }

            var handChanceMath = chanceMathSum / FlopCount;
            var handChancePsycho = chancePsychoSum / FlopCount;
            table[rowIndex, columnIndex] = new Ctx(title, handChanceMath, handChancePsycho);
        }

        var sb = new StringBuilder();

        sb.AppendLine(@"
<style>
table { border-collapse: collapse; }
td { border: 1px solid; padding: 4px 6px; }
big { font-size: 1.8em; font-weight: bold; }
</style>
");

        sb.AppendLine("<table>");

        for (var rowIndex = 0; rowIndex < 13; rowIndex++)
        {
            sb.AppendLine("  <tr>");

            for (var columnIndex = 0; columnIndex < 13; columnIndex++)
            {
                var chanceMath = table[rowIndex, columnIndex].ChanceMath;
                var chancePsycho = table[rowIndex, columnIndex].ChancePsycho;
                var red = 255;
                var green = 255;
                int blue;

                if (chancePsycho >= 0.5)
                    red = blue = (int)((1 - chancePsycho) * 448);
                else
                    green = blue = (int)(chancePsycho * 448);

                sb.AppendLine($"    <td style=\"background-color: #{red:X}{green:X}{blue:X}\">");
                sb.AppendLine($"<big>{table[rowIndex, columnIndex].Title}</big><br>");
                sb.AppendLine($"<b>{chancePsycho:.000}</b> /{chanceMath:.000}<br>");
                sb.AppendLine("</td>");
            }

            sb.AppendLine("  </tr>");
        }

        sb.AppendLine("</table>");
        var html = sb.ToString();

        File.WriteAllText(statisticsName + ".html", html);
    }

    private sealed class Ctx
    {
        public string Title { get; }
        public double ChanceMath { get; }
        public double ChancePsycho { get; }

        public Ctx(string title, double chanceMath, double chancePsycho)
        {
            Title = title;
            ChanceMath = chanceMath;
            ChancePsycho = chancePsycho;
        }
    }

    private static IReadOnlyList<(int, int)[]> LoadResources()
    {
        var result = new List<(int, int)[]>();
        var assembly = Assembly.GetExecutingAssembly() ?? throw new InvalidOperationException();
        var resourceNamePrefix = assembly.GetName().Name + ".Resources.";

        for (var handIndex = 0; handIndex < AllHands.Count; handIndex++)
        {
            var resourceName = resourceNamePrefix + handIndex.ToString().PadLeft(3, '0') + ".txt";
            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException();
            using var reader = new StreamReader(stream);
            var allText = reader.ReadToEnd();
            var lines = allText.TrimEnd().Split("\r\n");
            Debug.Assert(lines.Length == FlopCount);
            var handResult = new (int, int)[FlopCount];

            for (var flopIndex = 0; flopIndex < FlopCount; flopIndex++)
            {
                var line = lines[flopIndex];
                var commaIndex = line.IndexOf(',');
                var winCount = int.Parse(line[..commaIndex]);
                var splitCount = int.Parse(line[(commaIndex + 1)..]);
                handResult[flopIndex] = (winCount, splitCount);
            }

            result.Add(handResult);
        }

        return result;
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

        return value;
    }
}
