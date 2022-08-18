/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
namespace MultiThreading.Task1._100Tasks
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            HundredTasks();

            Console.ReadLine();
        }

        static void HundredTasks()
        {
            // feel free to add your code here
            var taskArray = new Task[TaskAmount];

            for (var i = 0; i < TaskAmount; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    for (var j = 0; j < MaxIterationsCount; j++)
                    {
                        Output(Task.CurrentId ?? -1, j);
                    }
                });
            }

            Task.WhenAll(taskArray);
        }

        static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
