/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code

            var list = new List<int>();
            var isChanged = false;

            object locker = new object();

            var secondTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (isChanged)
                    {
                        lock (locker)
                        {
                            isChanged = false;
                            Console.WriteLine(string.Join("; ", list));

                        }
                    }
                }
            });

            var task = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    lock (locker)
                    {
                        list.Add(GetRandomInt());
                        isChanged = true;
                    }
                    //Task.Delay(1000 * 60 * 3); //sec
                }
            });

            Console.ReadKey();
        }

        static int GetRandomInt()
        {
            lock (random)
            {
                return random.Next(0, 10);
            }
        }
    }
}
