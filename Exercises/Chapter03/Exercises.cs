//using System.Configuration;
using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;
using static LaYumba.Functional.F;

namespace Exercises.Chapter3
{
    public static class Exercises
    {
        // 1 Write a generic function that takes a string and parses it as a value of an enum. It
        // should be usable as follows:

        // Enum.Parse<DayOfWeek>("Friday") // => Some(DayOfWeek.Friday)
        // Enum.Parse<DayOfWeek>("Freeday") // => None
        public static Option<TEnum> Parse<TEnum>(string s) where TEnum : struct
          => System.Enum.TryParse<TEnum>(s, out var value) ? Some(value) : None;

        // 2 Write a Lookup function that will take an IEnumerable and a predicate, and
        // return the first element in the IEnumerable that matches the predicate, or None
        // if no matching element is found. Write its signature in arrow notation:

        // bool isOdd(int i) => i % 2 == 1;
        // new List<int>().Lookup(isOdd) // => None
        // new List<int> { 1 }.Lookup(isOdd) // => Some(1)
        public static Option<T> Lookup<T>(this IEnumerable<T> ts, Func<T, bool> f)
        {
            foreach (var t in ts)
                if (f(t)) return t;

            return None;
        }

        // 3 Write a type Email that wraps an underlying string, enforcing that it’s in a valid
        // format. Ensure that you include the following:
        // - A smart constructor
        // - Implicit conversion to string, so that it can easily be used with the typical API
        // for sending emails

        public class Email
        {
            private string Value { get; }

            private Email(string value)
            {
                if (!IsValid(value)) throw new ArgumentException($"'{value}' is not a valid email.");
                Value = value;
            }

            public static Option<Email> Create(string value) => IsValid(value) ? Some(new Email(value)) : None;

            public static implicit operator string(Email email) => email.Value;

            private static bool IsValid(string value)
            {
                try
                {
                    new MailAddress(value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // 4 Take a look at the extension methods defined on IEnumerable inSystem.LINQ.Enumerable.
        // Which ones could potentially return nothing, or throw some
        // kind of not-found exception, and would therefore be good candidates for
        // returning an Option<T> instead?
    }

    // 5.  Write implementations for the methods in the `AppConfig` class
    // below. (For both methods, a reasonable one-line method body is possible.
    // Assume settings are of type string, numeric or date.) Can this
    // implementation help you to test code that relies on settings in a
    // `.config` file?
    public class AppConfig
    {
        NameValueCollection source;

        //public AppConfig() : this(ConfigurationManager.AppSettings) { }

        public AppConfig(NameValueCollection source)
        {
            this.source = source;
        }

        public Option<T> Get<T>(string name)
         => source[name] == null ? None : Some((T)Convert.ChangeType(source[name], typeof(T)));

        public T Get<T>(string name, T defaultValue)
         => Get<T>(name).Match(() => defaultValue, v => v);

    }
}
