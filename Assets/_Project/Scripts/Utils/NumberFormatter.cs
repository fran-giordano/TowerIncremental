using System;
using System.Globalization;

namespace TowerIncremental.Utils
{
    // Formato corto para números grandes: 1234 → "1.23K", 1500000 → "1.5M".
    // Después de T (billones) sigue con letras: aa, ab, … zz. Más allá, notación científica.
    // Siempre trunca (nunca redondea hacia arriba): no muestra más oro del que hay.
    public static class NumberFormatter
    {
        static readonly string[] NamedSuffixes = { "", "K", "M", "B", "T" };
        const int LetterTiers = 26 * 26;
        const double Epsilon = 1e-9;   // compensa errores de punto flotante (1.15 * 100 = 114.999…)

        public static string Format(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                return value.ToString(CultureInfo.InvariantCulture);

            string sign = value < 0 ? "-" : "";
            double abs = Math.Abs(value);

            if (abs < 1000)
                return sign + Math.Floor(abs + Epsilon).ToString("0", CultureInfo.InvariantCulture);

            int tier = (int)Math.Floor(Math.Log10(abs) / 3);
            double scaled = abs / Math.Pow(1000, tier);
            if (scaled >= 1000 - Epsilon) { tier++; scaled /= 1000; }
            else if (scaled < 1) { tier--; scaled *= 1000; }

            string suffix = Suffix(tier);
            if (suffix == null)
                return sign + abs.ToString("0.00e0", CultureInfo.InvariantCulture);

            return sign + Truncate(scaled) + suffix;
        }

        // Tres cifras significativas como máximo, sin ceros de más: 1.23, 12.3, 123.
        static string Truncate(double scaled)
        {
            if (scaled < 10)  return (Math.Floor(scaled * 100 + Epsilon) / 100).ToString("0.##", CultureInfo.InvariantCulture);
            if (scaled < 100) return (Math.Floor(scaled * 10 + Epsilon) / 10).ToString("0.#", CultureInfo.InvariantCulture);
            return Math.Floor(scaled + Epsilon).ToString("0", CultureInfo.InvariantCulture);
        }

        static string Suffix(int tier)
        {
            if (tier < NamedSuffixes.Length) return NamedSuffixes[tier];

            int index = tier - NamedSuffixes.Length;
            if (index >= LetterTiers) return null;
            return new string(new[] { (char)('a' + index / 26), (char)('a' + index % 26) });
        }
    }
}
