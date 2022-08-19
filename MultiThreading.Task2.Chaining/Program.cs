/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
namespace MultiThreading.Task2.Chaining
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    class Program
    {
        static readonly Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code

            var array = new int[10];

            Task.Run(async () =>
            {
                await Fill(array);
                await Multiplay(array);
                await Sort(array);
                await Avarage(array);
            });

            Console.ReadKey();
        }

        private static Task Avarage(int[] array)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Avarage = " + array.DefaultIfEmpty(0).Average());
            });
        }

        private static Task Sort(int[] array)
        {
            return Task.Run(() =>
            {
                Array.Sort(array);

                Console.WriteLine("Sorted array: " + string.Join("; ", array));
            });
        }

        private static Task Multiplay(int[] array)
        {
            return Task.Run(() =>
            {
                var x = GetRandom();

                Console.WriteLine($"Multiplay value: {x}");

                for (int i = 0; i < array.Length; i++)
                {
                    array[i] *= x;
                }

                Console.WriteLine(string.Join("; ", array));
            });
        }

        private static int GetRandom()
        {
            lock (random)
            {
                return random.Next(1, 20);
            }
        }

        private static Task Fill(int[] array)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = GetRandom();
                }

                Console.WriteLine("Array: " + string.Join("; ", array));
            });
        }
    }
}
