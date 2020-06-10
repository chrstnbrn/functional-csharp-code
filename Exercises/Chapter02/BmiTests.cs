using NUnit.Framework;
using System;

namespace Exercises.Chapter2
{
    [TestFixture]
    public class BmiTests
    {
        [TestCase(1.8, 80, ExpectedResult = "healthy weight")]
        [TestCase(1.8, 50, ExpectedResult = "underweight")]
        [TestCase(1.8, 100, ExpectedResult = "overweight")]
        [TestCase(1.6, 50, ExpectedResult = "healthy weight")]
        [TestCase(1.6, 80, ExpectedResult = "overweight")]
        [TestCase(1.6, 40, ExpectedResult = "underweight")]
        public string RateWeight_ReturnsExpectedResult(double height, double weight)
        {
            return Bmi.RateWeight(height, weight);
        }

        [TestCase(1.8, 80, ExpectedResult = "healthy weight")]
        [TestCase(1.8, 50, ExpectedResult = "underweight")]
        [TestCase(1.8, 100, ExpectedResult = "overweight")]
        [TestCase(1.6, 50, ExpectedResult = "healthy weight")]
        [TestCase(1.6, 80, ExpectedResult = "overweight")]
        [TestCase(1.6, 40, ExpectedResult = "underweight")]
        public string Run_ReturnsExpectedResult(double height, double weight)
        {
            return Bmi.Run(() => height, () => weight);
        }
    }
}
