using System;

namespace Exercises
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Func<string> prompt(string promptMessage) => () => { Console.Write(promptMessage); return Console.ReadLine(); };
            Func<double> promptDouble(string promptMessage) => () => double.Parse(prompt(promptMessage)());

            var result = Chapter2.Bmi.Run(
                promptDouble("Please enter your height in m: "),
                promptDouble("Please enter your weight in kg: "));

            Console.WriteLine(result);
        }
    }
}
