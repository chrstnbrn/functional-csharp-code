using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Exercises.Chapter1.Exercises;

namespace Exercises.Chapter1
{
    [TestFixture]
    class ExercisesTests
    {
        [Test]
        [TestCase(0, ExpectedResult = false)]
        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = false)]
        public bool Negate_IsEven_ReturnsExpectedResult(int value)
        {
            Func<int, bool> isEven = i => i % 2 == 0;
            var isOdd = Negate(isEven);

            return isOdd(value);
        }

        static IEnumerable<TestCaseData> SortNumbersAcending
        {
            get
            {
                yield return new TestCaseData(new List<int>()).Returns(new List<int>());
                yield return new TestCaseData(new List<int> { 1 }).Returns(new List<int> { 1 });
                yield return new TestCaseData(new List<int> { 1, 2, 3 }).Returns(new List<int> { 1, 2, 3 });
                yield return new TestCaseData(new List<int> { 2, 1, 3 }).Returns(new List<int> { 1, 2, 3 });
            }
        }

        [Test]
        [TestCaseSource(nameof(SortNumbersAcending))]
        public List<int> Sort_NumbersDefaultOrder_ReturnsSortedList(List<int> values)
        {
            return Sort(values, Comparer<int>.Default.Compare);
        }

        static IEnumerable<TestCaseData> SortNumbersDescending
        {
            get
            {
                yield return new TestCaseData(new List<int>()).Returns(new List<int>());
                yield return new TestCaseData(new List<int> { 1 }).Returns(new List<int> { 1 });
                yield return new TestCaseData(new List<int> { 1, 2, 3 }).Returns(new List<int> { 3, 2, 1 });
                yield return new TestCaseData(new List<int> { 2, 1, 3 }).Returns(new List<int> { 3, 2, 1 });
            }
        }

        [Test]
        [TestCaseSource(nameof(SortNumbersDescending))]
        public List<int> Sort_NumbersDescending_ReturnsSortedList(List<int> values)
        {
            return Sort(values, (x, y) => y - x);
        }   

        static IEnumerable<TestCaseData> SortStringsAcending
        {
            get
            {
                yield return new TestCaseData(new List<string>()).Returns(new List<string>());
                yield return new TestCaseData(new List<string> { "a" }).Returns(new List<string> { "a" });
                yield return new TestCaseData(new List<string> { "a", "B", "c" }).Returns(new List<string> { "a", "B", "c" });
                yield return new TestCaseData(new List<string> { "B", "c", "a" }).Returns(new List<string> { "a", "B", "c" });
            }
        }

        [Test]
        [TestCaseSource(nameof(SortStringsAcending))]
        public List<string> Sort_StringsAcsending_ReturnsSortedList(List<string> values)
        {
            return Sort(values, StringComparer.InvariantCultureIgnoreCase.Compare);
        }   
    }
}