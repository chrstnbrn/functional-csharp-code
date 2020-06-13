using LaYumba.Functional;
using System;
using static LaYumba.Functional.F;

namespace Exercises.Chapter7
{
    static class Exercises
    {
        // 1. Partial application with a binary arithmethic function:
        // Write a function `Remainder`, that calculates the remainder of 
        // integer division(and works for negative input values!).

        static Func<int, int, Option<int>> Remainder = (int dividend, int divisor)
            => dividend == 0 ? None : Some(dividend - divisor * (dividend / divisor));

        // Notice how the expected order of parameters is not the
        // one that is most likely to be required by partial application
        // (you are more likely to partially apply the divisor).

        // Write an `ApplyR` function, that gives the rightmost parameter to
        // a given binary function (try to write it without looking at the implementation for `Apply`).
        // Write the signature of `ApplyR` in arrow notation, both in curried and non-curried form
        // ApplyR: ((T1, T2) -> R, T2) -> (T1 -> R)
        // ApplyR curried: (T1 -> T2 -> R) -> T2 -> T1 -> R

        static Func<T1, R> ApplyR<T1, T2, R>(this Func<T1, T2, R> f, T2 t2)
            => t1 => f(t1, t2);

        // Use `ApplyR` to create a function that returns the
        // remainder of dividing any number by 5. 
        static Func<int, Option<int>> RemainderWhenDividingBy5 => Remainder.ApplyR(5);

        // Write an overload of `ApplyR` that gives the rightmost argument to a ternary function
        static Func<T1, T2, R> ApplyR<T1, T2, T3, R>(this Func<T1, T2, T3, R> f, T3 t3)
            => (t1, t2) => f(t1, t2, t3);

        // 2. Let's move on to ternary functions. Define a class `PhoneNumber` with 3
        // fields: number type(home, mobile, ...), country code('it', 'uk', ...), and number.
        // `CountryCode` should be a custom type with implicit conversion to and from string.
        class PhoneNumber
        {
            public NumberType NumberType { get; private set; }
            public CountryCode CountryCode { get; private set; }
            public string Number { get; private set; }

            public static Func<CountryCode, NumberType, string, PhoneNumber> CreatePhoneNumber = (CountryCode countryCode, NumberType numberType, string number)
                => new PhoneNumber
                {
                    NumberType = numberType,
                    CountryCode = countryCode,
                    Number = number
                };

            public static Func<NumberType, string, PhoneNumber> CreateUkPhoneNumber => CreatePhoneNumber.Apply(new CountryCode("UK"));
            public static Func<string, PhoneNumber> CreateUkMobilePhoneNumber => CreateUkPhoneNumber.Apply(NumberType.Mobile);
        }

        enum NumberType { Home, Mobile, Work }

        class CountryCode
        {
            private string value;

            public CountryCode(string value)
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(nameof(value));
                this.value = value;
            }

            public static implicit operator CountryCode(string s) => new CountryCode(s);
            public static implicit operator string(CountryCode c) => c.value;
        }

        // Now define a ternary function that creates a new number, given values for these fields.
        // What's the signature of your factory function? 

        // Use partial application to create a binary function that creates a UK number, 
        // and then again to create a unary function that creates a UK mobile


        // 3. Functions everywhere. You may still have a feeling that objects are ultimately 
        // more powerful than functions. Surely, a logger object should expose methods 
        // for related operations such as Debug, Info, Error? 
        // To see that this is not necessarily so, challenge yourself to write 
        // a very simple logging mechanism without defining any classes or structs. 
        // You should still be able to inject a Log value into a consumer class/function, 
        // exposing operations like Debug, Info, and Error, like so:

        static void ConsumeLog(Log log)
           => log.Info("look! no objects!");

        static void Main() => ConsumeLog(ConsoleLogger);

        enum Level { Debug, Info, Error }

        delegate void Log(Level level, string message);

        static Log ConsoleLogger = (Level level, string message) => Console.WriteLine($"{level}: {message}}");

        static void Debug(this Log log, string message) => log(Level.Debug, message);
        static void Info(this Log log, string message) => log(Level.Info, message);
        static void Error(this Log log, string message) => log(Level.Error, message);
    }
}
