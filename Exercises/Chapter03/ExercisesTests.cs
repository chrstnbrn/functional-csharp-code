using LaYumba.Functional;
using static LaYumba.Functional.F;
using NUnit.Framework;
using System.Collections.Generic;
using static Exercises.Chapter3.Exercises;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Linq;

namespace Exercises.Chapter3
{
    [TestFixture]
    public class ExercisesTests
    {
        public enum TestEnum { A, B, C }

        public static IEnumerable<TestCaseData> ParseTestCaseData {
            get
            {
                yield return new TestCaseData("A").Returns(Some(TestEnum.A));
                yield return new TestCaseData("B").Returns(Some(TestEnum.B));
                yield return new TestCaseData("C").Returns(Some(TestEnum.C));
                yield return new TestCaseData("X").Returns(None);
            }    
        }

        [TestCaseSource(nameof(ParseTestCaseData))]
        public Option<TestEnum> Parse_ReturnsCorrectOptionValue(string value)
        {
            return Parse<TestEnum>(value);
        }

        public static IEnumerable<TestCaseData> LookupTestCaseData
        {
            get
            {
                yield return new TestCaseData(Enumerable.Empty<int>()).Returns(None);
                yield return new TestCaseData(new[] { 1, 3, 5 }).Returns(None);
                yield return new TestCaseData(new[] {1, 2, 3, 4}).Returns(Some(2));
            }
        }

        [TestCaseSource(nameof(LookupTestCaseData))]
        public Option<int> Lookup_IsEven_ReturnsFirstEvenNumber(IEnumerable<int> ts)
        {
            Func<int, bool> isEven = x => x % 2 == 0;
            return Exercises.Lookup(ts, isEven);
        }

        public static IEnumerable<TestCaseData> CreateEmailTestCaseData
        {
            get
            {
                yield return new TestCaseData("myname@gmail.com").Returns(Some("myname@gmail.com"));
                yield return new TestCaseData("notavalidemail@").Returns(None);
            }
        }

        [TestCaseSource(nameof(CreateEmailTestCaseData))]
        public Option<string> Email_Create_ReturnsEmailIfValid(string email)
        {
            return Email.Create(email).Map(s => (string)s);
        }
    }
}
