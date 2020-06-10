using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercises.Chapter1
{
    static class Exercises
    {
        // 1. Write a function that negates a given predicate: whenvever the given predicate
        // evaluates to `true`, the resulting function evaluates to `false`, and vice versa.
        public static Func<T, bool> Negate<T>(Func<T, bool> predicate) => x => !predicate(x);

        // 2. Write a method that uses quicksort to sort a `List<int>` (return a new list,
        // rather than sorting it in place).

        // 3. Generalize your implementation to take a `List<T>`, and additionally a 
        // `Comparison<T>` delegate.
        public static List<T> Sort<T>(List<T> values, Comparison<T> comparison)
        {
            if (!values.Any()) return new List<T>();

            var pivot = values.First();
            var rest = values.Skip(1);

            var small = rest.Where(x => comparison(x, pivot) <= 0).ToList();
            var large = rest.Where(x => comparison(x, pivot) > 0).ToList();

            return Sort(small, comparison).Append(pivot).Concat(Sort(large, comparison)).ToList();
        }

        // 4. In this chapter, you've seen a `Using` function that takes an `IDisposable`
        // and a function of type `Func<TDisp, R>`. Write an overload of `Using` that
        // takes a `Func<IDisposable>` as first
        // parameter, instead of the `IDisposable`. (This can be used to fix warnings
        // given by some code analysis tools about instantiating an `IDisposable` and
        // not disposing it.)

        public static R Using<TDisp, R>(Func<TDisp> createDisposable, Func<TDisp, R> f) where TDisp : IDisposable
        {
            using (var disposable = createDisposable()) return f(disposable);
        }
    }
}
