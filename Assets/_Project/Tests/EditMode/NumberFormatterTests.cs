using NUnit.Framework;
using TowerIncremental.Utils;

namespace TowerIncremental.Tests
{
    public class NumberFormatterTests
    {
        [TestCase(0, "0")]
        [TestCase(7.9, "7")]
        [TestCase(999, "999")]
        [TestCase(1000, "1K")]
        [TestCase(1150, "1.15K")]
        [TestCase(1234, "1.23K")]
        [TestCase(12345, "12.3K")]
        [TestCase(123456, "123K")]
        [TestCase(999999, "999K")]
        [TestCase(1e6, "1M")]
        [TestCase(1.5e9, "1.5B")]
        [TestCase(1e12, "1T")]
        [TestCase(1e15, "1aa")]
        [TestCase(2.5e18, "2.5ab")]
        [TestCase(-1500, "-1.5K")]
        public void Format_DevuelveFormatoCorto(double value, string expected)
        {
            Assert.AreEqual(expected, NumberFormatter.Format(value));
        }
    }
}
