using LaYumba.Functional;
using System;
using static LaYumba.Functional.F;
using Unit = System.ValueTuple;

namespace Exercises.Chapter08
{
    static class Exercises
    {
        // 1. Implement Apply for Either and Exceptional
        static Either<L, R> Apply<L, T, R>(this Either<L, Func<T, R>> eitherF, Either<L, T> eitherT)
            => eitherT.Bind(t => eitherF.Bind<L, Func<T, R>, R>(f => f(t)));

        static Exceptional<R> Apply<T, R>(this Exceptional<Func<T, R>> exceptionalF, Exceptional<T> exceptionalT)
            => exceptionalT.Bind(t => exceptionalF.Bind<Func<T, R>, R>(f => f(t)));

        // 2. Implement the query pattern for Either and Exceptional.
        // Try to write down the signatures for Select and SelectMany without looking at any examples.
        // For the implementation, just follow the types—if it type checks, it’s probably right!
        static Either<L, R> Select<L, T, R>(this Either<L, T> e, Func<T, R> f)
            => e.Match<Either<L, R>>(l =>l, t => f(t));

        static Either<L, R> SelectMany<L, T, R>(this Either<L, T> e, Func<T, Either<L, R>> f)
            => e.Match(l => l, t => f(t));

        static Exceptional<R> Select<T, R>(this Exceptional<T> e, Func<T, R> f)
            => e.Match(x => x, t => Exceptional(f(t)));

        static Exceptional<R> Select<T, R>(this Exceptional<T> e, Func<T, Exceptional<R>> f)
            => e.Match(x => x, t => f(t));

        // 3. Come up with a scenario in which various Either-returning operations are chained with Bind.
        // (If you’re short of ideas, you can use the favorite-dish example from chapter 6.)
        // Rewrite the code using a LINQ expression.

        class Reason { }
        class Ingredients { }
        class Food { }

        static Func<Either<Reason, Unit>> WakeUpEarly;
        static Func<Unit, Either<Reason, Ingredients>> ShopForIngredients;
        static Func<Ingredients, Either<Reason, Food>> CookRecipe;

        static Action<Food> EnjoyTogether;
        static Action<Reason> ComplainAbout;
        static Action OrderPizza;

        static void Start()
        {
            WakeUpEarly()
               .Bind(ShopForIngredients)
               .Bind(CookRecipe)
               .Match(
                  Right: dish => EnjoyTogether(dish),
                  Left: reason =>
                  {
                      ComplainAbout(reason);
                      OrderPizza();
                  });

            var result = from x in WakeUpEarly()
                         from ingredients in ShopForIngredients(x)
                         from dish in CookRecipe(ingredients)
                         select dish;

            result.Match(
                Right: dish => EnjoyTogether(dish),
                Left: reason => {
                    ComplainAbout(reason);
                    OrderPizza();
                });

        }
    }
}
