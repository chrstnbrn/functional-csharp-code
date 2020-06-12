using LaYumba.Functional;
using System;
using System.Linq;
using System.Net.Mail;
using static LaYumba.Functional.F;

namespace Exercises.Chapter6
{
    static class Exercises
    {
        // 1. Write a `ToOption` extension method to convert an `Either` into an
        // `Option`. Then write a `ToEither` method to convert an `Option` into an
        // `Either`, with a suitable parameter that can be invoked to obtain the
        // appropriate `Left` value, if the `Option` is `None`. (Tip: start by writing
        // the function signatures in arrow notation)

        // ToOption: Either<L, R> -> Option<R>
        // ToEither: (Option<T>, Func<L>) -> Either<L, T>

        static Option<R> ToOption<L, R>(this Either<L, R> e) => e.Match(l => None, Some);

        static Either<L, T> ToEither<L, T>(this Option<T> t, Func<L> f)
            => t.Match<Either<L, T>>(() => f(), x => x);

        // 2. Take a workflow where 2 or more functions that return an `Option`
        // are chained using `Bind`.

        // Then change the first one of the functions to return an `Either`.

        // This should cause compilation to fail. Since `Either` can be
        // converted into an `Option` as we have done in the previous exercise,
        // write extension overloads for `Bind`, so that
        // functions returning `Either` and `Option` can be chained with `Bind`,
        // yielding an `Option`.

        static Option<string> ChainFunctions()
        {
            return GetEmail(0).Bind(Validate);
        }

        static Either<string, string> GetEmail(int index)
        {
            var emails = new[] { "myname@gmail.com", "invalid" };
            return 0 <= index && index < emails.Length
                ? Right(emails.ElementAt(index))
                : (Either<string, string>)Left($"No email for index {index}.");
        }

        static Option<string> Validate(string email)
        {
            try
            {
                new MailAddress(email);
                return email;
            }
            catch
            {
                return None;
            }
        }

        static Option<R2> Bind<R1, R2, L>(this Either<L, R1> e, Func<R1, Option<R2>> f) => e.ToOption().Bind(f);

        static Option<R2> Bind<R1, R2, L>(this Option<R1> o, Func<R1, Either<L, R2>> f) => o.Match(() => None, v => f(v).ToOption());


        // 3. Write a function `Safely` of type ((() → R), (Exception → L)) → Either<L, R> that will
        // run the given function in a `try/catch`, returning an appropriately
        // populated `Either`.
        static Either<L, R> Safely<L, R>(this Func<R> f, Func<Exception, L> left)
        {
            try
            {
                return f();
            }
            catch (Exception e)
            {
                return left(e);
            }
        }

        // 4. Write a function `Try` of type (() → T) → Exceptional<T> that will
        // run the given function in a `try/catch`, returning an appropriately
        // populated `Exceptional`.
        static Exceptional<T> Try<T>(this Func<T> f)
        {
            try
            {
                return f();
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
