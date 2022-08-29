/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static Random random = new Random();
        static object locker = new object();
        static Semaphore semaphore = new Semaphore(5, 5);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code

            int state = random.Next(10, 20);
            Console.WriteLine($"Initial state: {state}\nUsing threads and lock");

            var thread = new Thread(() => GenerateThreads(state));
            thread.Start();
            thread.Join();

            state = random.Next(10, 20);
            Console.WriteLine($"Initial state: {state}\nUsing threadpool and semaphore");
            
            var resetEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(arg =>
            {
                GenerateThreadsWithThreadPool(state, resetEvent);
            });

            resetEvent.WaitOne();

            Console.WriteLine("Programm finished!");
            Console.ReadKey();
        }

        static void GenerateThreadsWithThreadPool(int count, ManualResetEvent resetEvent)
        {
            if (count == 0)
            {
                resetEvent.Set();
                return;
            }

            semaphore.WaitOne();

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} received state = {count}");

            Interlocked.Decrement(ref count);

            Thread.Sleep(random.Next(1, 2) * 1000);

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} stay state = {count}");
            
            ThreadPool.QueueUserWorkItem(arg =>
            {
                GenerateThreadsWithThreadPool(count, resetEvent);
            });

            semaphore.Release();            
        }

        static void GenerateThreads(int count)
        {
            if (count == 0)
            {
                return;
            }

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} received state = {count}");

            Interlocked.Decrement(ref count);

            int sleepInt;

            lock (locker)
            {
                sleepInt = random.Next(1, 2) * 1000;
            }

            Thread.Sleep(sleepInt);

            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} stay state = {count}");

            var thread = new Thread(() => GenerateThreads(count));
            thread.Start();

            thread.Join();
        }
    }
}
