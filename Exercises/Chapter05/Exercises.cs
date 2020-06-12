using Examples.Chapter3;
using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercises.Chapter5
{
    public static class Exercises
    {
        // 1. Without looking at any code or documentation (or intllisense), write the function signatures of
        // `OrderByDescending`, `Take` and `Average`, which we used to implement `AverageEarningsOfRichestQuartile`:

        // OrderByDescending: (IEnumerable<T>, Func<T, K>) -> IEnumerable<T>
        // Take: (IEnumerable<T>, int) -> IEnumerable<T>
        // Average: IEnumerable<decimal> -> decimal

        static decimal AverageEarningsOfRichestQuartile(List<Person> population)
           => population
              .OrderByDescending(p => p.Earnings)
              .Take(population.Count / 4)
              .Select(p => p.Earnings)
              .Average();

        // 2 Check your answer with the MSDN documentation: https://docs.microsoft.com/
        // en-us/dotnet/api/system.linq.enumerable. How is Average different?

        // 3 Implement a general purpose Compose function that takes two unary functions
        // and returns the composition of the two.
        static Func<T, R2> Compose<T, R1, R2>(this Func<R1, R2> g, Func<T, R1> f) => x => g(f(x));
    }
}
