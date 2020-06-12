using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using static LaYumba.Functional.F;

namespace Exercises.Chapter4
{
    static class Exercises
    {
        // 1 Implement Map for ISet<T> and IDictionary<K, T>. (Tip: start by writing down
        // the signature in arrow notation.)
        static ISet<TResult> Map<TSource, TResult>(this ISet<TSource> set, Func<TSource, TResult> f)
        {
            var result = new HashSet<TResult>();

            foreach (var item in set)
                result.Add(f(item));

            return result;
        }

        static IDictionary<K, TResult> Map<K, TSource, TResult>(this IDictionary<K, TSource> dictionary, Func<TSource, TResult> f)
        {
            var result = new Dictionary<K, TResult>();

            foreach (var keyValuePair in dictionary)
                result.Add(keyValuePair.Key, f(keyValuePair.Value));

            return result;
        }

        // 2 Implement Map for Option and IEnumerable in terms of Bind and Return.
        static Option<R> Map<T, R>(this Option<T> option, Func<T, R> f)
            => option.Bind(x => Some(f(x)));

        static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f)
            => ts.Bind(t => List(f(t)));

        // 3 Use Bind and an Option-returning Lookup function (such as the one we defined
        // in chapter 3) to implement GetWorkPermit, shown below. 

        // Then enrich the implementation so that `GetWorkPermit`
        // returns `None` if the work permit has expired.

        static Option<WorkPermit> GetWorkPermit(Dictionary<string, Employee> people, string employeeId)
            => people.Lookup(employeeId).Bind(e => e.WorkPermit).Where(w => w.Expiry > DateTime.Now);

        // 4 Use Bind to implement AverageYearsWorkedAtTheCompany, shown below (only
        // employees who have left should be included).

        static double AverageYearsWorkedAtTheCompany(List<Employee> employees)
            => employees
                .Bind(e => e.LeftOn.Map(l => YearsBetween(e.JoinedOn, l)))
                .Average();

        static double YearsBetween(DateTime a, DateTime b) => (b - a).TotalDays / 365;
    }

    public struct WorkPermit
    {
        public string Number { get; set; }
        public DateTime Expiry { get; set; }
    }

    public class Employee
    {
        public string Id { get; set; }
        public Option<WorkPermit> WorkPermit { get; set; }

        public DateTime JoinedOn { get; }
        public Option<DateTime> LeftOn { get; }
    }
}