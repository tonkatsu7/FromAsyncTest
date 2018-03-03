using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace FromAsyncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var source = Observable
                            .Interval(TimeSpan.FromSeconds(5))
                            .Do(x => Console.WriteLine($"IDX={x}"));

            source
                .Select(x => Observable.FromAsync(() => SomethingAsync(Convert.ToInt32(x))))
                .Concat()
                .Subscribe(x => Console.WriteLine($"OUTPUT={x} has {x.ToString().Length}"));

            Console.ReadKey();
        }

        private static async Task<string> SomethingAsync(int param)
        {
            await Task.Delay(1000);

            Console.WriteLine($"SomethingAsync={param}");

            return $"The parameter is {NumberToWords(param)}";
        }

        /// <summary>
        ///  Taken from <see href="https://stackoverflow.com/questions/2729752/converting-numbers-in-to-words-c-sharp">StackOverflow</see>
        /// </summary>
        /// <param name="number">Just an integer</param>
        /// <returns>The integer spelt as a word</returns>
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}
