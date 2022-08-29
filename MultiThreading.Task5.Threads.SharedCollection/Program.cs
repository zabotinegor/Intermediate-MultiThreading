/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static readonly Random random = new Random();
        static readonly Semaphore semaphore = new Semaphore(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            var blockingCollection = new BlockingCollection<int>();
            var isChanged = false;
            var resetEvent = new ManualResetEvent(false);

            var creationTask = new Task(() =>
            {
                var count = GetRandom(10, 20);
                for (int i = 0; i < count;)
                {
                    semaphore.WaitOne();

                    if (!isChanged)
                    {
                        Console.WriteLine($"Task {Task.CurrentId} adding item");
                        isChanged = blockingCollection.TryAdd(GetRandom(1, 100));
                        i++;
                    }

                    semaphore.Release();
                }

                blockingCollection.CompleteAdding();
                resetEvent.Set();
            });

            var printingTask = new Task(() =>
            {
                while (!blockingCollection.IsAddingCompleted)
                {
                    semaphore.WaitOne();

                    if (isChanged)
                    {
                        Console.WriteLine($"Task {Task.CurrentId}: " + string.Join("; ", blockingCollection));
                        isChanged = false;
                        Task.Delay(GetRandom(1, 2) * 1000);
                    }

                    semaphore.Release();
                }
            });

            printingTask.Start();
            creationTask.Start();

            resetEvent.WaitOne();

            Console.WriteLine("Program finished!");
            Console.ReadKey();
        }

        private static int GetRandom(int v1, int v2)
        {
            lock (random)
            {
                var result = random.Next(v1, v2);
                return result;
            }
        }
    }
}
