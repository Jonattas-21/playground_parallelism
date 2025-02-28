using playground_csharp;
using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(":::::::::::::::Starting the task async game!:::::::::::::::");
        string output = "";
        showInto(ref output);

        bool run = true;

        while (run)
        {
            switch (output)
            {
                case "1":
                    buildSimpleTaskWithCancelation();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "2":
                    buildSimpleTaskWithCancelationSource();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "3":
                    buildSimpleBalanceCalculation();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "4":
                    builderSimpleReadLock();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "5":
                    UseOfConcurrentCollections useOfConcurrentDictionary = new UseOfConcurrentCollections();
                    useOfConcurrentDictionary.executionDictionary();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "exit":
                    run = false;
                    output = string.Empty;
                    break;
                default:
                    break;
            }
        }

        Console.WriteLine("The end of the task");
    }

    private static void showInto(ref string output)
    {
        Console.WriteLine("Type the number of one of the algorithms bellow");
        Console.WriteLine("1:    async task example with cancellation token");
        Console.WriteLine("2:    async task example with many cancellation token");
        Console.WriteLine("3:    simple credit/debit async operations");
        Console.WriteLine("4:    example with read/write lock");
        Console.WriteLine("5:    using concurrent dictionary"); //todo
        Console.WriteLine("exit: close the app");

        output = Console.ReadLine();
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

    static void builderSimpleReadLock()
    {
        ReaderWriterLocker.SimpleReadLock();
    }
}
