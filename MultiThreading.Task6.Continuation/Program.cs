/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code

            Console.WriteLine("Option a");
            var taskA = Task.Run(() => DateTime.Today.DayOfWeek);
            var taskB = taskA.ContinueWith(antecedent => Console.WriteLine($"Today is {antecedent.Result}."));

            Task.WaitAll(taskA, taskB);

            // todo: implement
            Console.WriteLine("Option b");

            // todo: implement
            Console.WriteLine("Option c");

            Console.WriteLine("Option d");

            using var cts = new CancellationTokenSource();
            var token = cts.Token;
            var timer = new Timer(Elapsed, cts, 5000, Timeout.Infinite);

            var taskC = Task.Run(() => 
            {
                Console.WriteLine($"Task C started");

                for (int i = 0; ; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation requested in antecedent...");
                        token.ThrowIfCancellationRequested();
                    }

                    Task.Delay(1000);
                }
            }, token);
            // not outside of threadpool
            var taskD = taskC.ContinueWith(antecedent => 
            {
                Console.WriteLine("Task D started");
            }, TaskContinuationOptions.OnlyOnCanceled);

            Task.WaitAny(taskD);

            Console.WriteLine("Program finished");
            Console.ReadKey();
        }

        private static void Elapsed(object state)
        {
            if (state is CancellationTokenSource cts)
            {
                Console.WriteLine("Cancellation request issued...");
                cts.Cancel();                
            }
        }
    }
}
