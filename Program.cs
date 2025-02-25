using playground_csharp;
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(":::::::::::::::Starting the task async game!");

        //buildSimpleTaskWithCancelation();

        //buildSimpleTaskWithCancelationSource();

        //buildSimpleBalanceCalculation();

        builderSimpleReadLock();

        Console.WriteLine("The end of the task");
        Console.ReadKey();

    }

    static void buildSimpleTaskWithCancelation()
    {
        var (task, cts) = playground_csharp.TaskService.SimpleTaskWithCancelation();

        cts.Token.Register(() =>
        {
            Console.WriteLine("got it!! task was cancelled");
        });

        task.Start();

        Console.WriteLine(":::::::::::::::Type to cancel");
        Console.ReadKey();
        cts.Cancel();
    }

    static void buildSimpleTaskWithCancelationSource()
    {
        var (task, planned, preventive, emergency) = playground_csharp.TaskService.SimpleTaskWithCancelationSource();

        planned.Token.Register(() => Console.WriteLine("Cancelled by planned"));
        preventive.Token.Register(() => Console.WriteLine("Cancelled by preventive"));
        emergency.Token.Register(() => Console.WriteLine("Cancelled by emergency"));

        task.Start();

        Console.ReadKey();
        emergency.Cancel();
    }

    static void buildSimpleBalanceCalculation()
    {
        //  CriticalSection.SimpleBalanceCalculation(800, 800);
        MutexSection.SimpleBalanceCalculation(1, 1);
    }

    static void builderSimpleReadLock() {
        ReaderWriterLocker.SimpleReadLock();
    }
}
