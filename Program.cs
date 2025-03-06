using playground_csharp;
using System;

public class Program
{
    public static async void Main()
    {
        Console.WriteLine(":::::::::::::::Starting the task async game!:::::::::::::::");
        string output = "";
        showInto(ref output);

        bool run = true;
        string explanation = "";
        SampleTaskCoordinator useOfSampleTaskCoordinator = new SampleTaskCoordinator();
        UseOfConcurrentCollections useOfConcurrentCollections = new UseOfConcurrentCollections();

        while (run)
        {
            switch (output)
            {
                case "1":
                    buildSimpleTaskWithCancelation();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    explanation = "This is an exemple of start and cancelation of a task, this can occurs by many reasons the point is, the previous method that creates asn start the task, has the control to cancell it by any reason, this can be usefull because the task once useful, can turn out disposable.";
                    Console.WriteLine(explanation);

                    showInto(ref output);
                    break;
                case "2":
                    buildSimpleTaskWithCancelationSource();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    explanation = "Similarly to a simple cancelation token, this stratecy can have more than one cancelation token, and it can be divided in many possible cancelations reasons, once one of this reason/token is triggered, all the process it's canceled, is the example bellow there are three reasons, planned,  preventive and emergency, and this can be usefull to control one task by many perspective by the code.";
                    Console.WriteLine(explanation);

                    showInto(ref output);
                    break;
                case "3":
                    buildSimpleBalanceCalculation();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    explanation = "In this example three differente things happens using mutex, the first ans second two mutex controls (lock and release) the mutex for the transactions deposit and withdraw, this situations emulate many tasks trying to do math with a number, but one per time to ensure the correct final result, and in the end another bunch of tasks transfer the money to one account to another, the goal here it's to lock two diferents mutex for two entities.";

                    Console.WriteLine(explanation);
                    showInto(ref output);
                    break;
                case "4":
                    builderSimpleReadLock();
                    output = string.Empty;
                    Console.WriteLine("------------> end of the execution");
                    explanation = "This method such as a mutex, controls access to resources, and these access can be coltrolled by exclusivelly write and read for multiples threads, and there is a option for recurrency. This examples illustrate many threads reading the same resource and one thread writing.";
                    Console.WriteLine(explanation);
                    showInto(ref output);
                    break;
                case "5":
                    useOfConcurrentCollections.executionDictionary();
                    output = string.Empty;
                    explanation = "This is a improved dictionary with methods to support concurrency.";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "6":
                    useOfConcurrentCollections.executionConcurrentQueue();
                    output = string.Empty;
                    explanation = "Similarly to dictionary, this object has the fearture of try, then it will be able to handle multiple process trying to access at the same time. ";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "7":
                    useOfConcurrentCollections.executionStack();
                    output = string.Empty;
                    explanation = "in this case the default value of the queue is zero, so we are trying to get 5 elements in the object, this five elements could be get by another process, in normal queue this current process would get an error outOfRangeException, but this treatment will bring the default value 0 if the queue went out of range.";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "8":
                    useOfConcurrentCollections.executionConcurentBag();
                    output = string.Empty;
                    explanation = "This example demonstrates that the object knows how to deal with many threads adding and peeking the itens in a sequence by processId and item, even if it's happening concurently.";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "9":
                    useOfSampleTaskCoordinator.ExecuteContinuation();
                    output = string.Empty;
                    explanation = "This example demonstrates that the object knows how to deal with many threads adding and peeking the itens in a sequence by processId and item, even if it's happening concurently.";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "10":
                    useOfSampleTaskCoordinator.ExecuteBarrierSample();
                    output = string.Empty;
                    explanation = "The barrier concept allows to cooperative runs tasks in paralell, in this example I set up 2 taks to run for the algorithm, and using the method SignalAndWait() if there are 2 tasks signalled the process will run, otherwise it will wait for 2, in other words, the barrier will block all the threads until 2 tasks get signalled.";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "11":
                    useOfSampleTaskCoordinator.ExecuteSemaphoreSlim();
                    output = string.Empty;
                    explanation = "This approach synchronized the threads using a counter  to decrease by wait() ans increase by release(n)";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "12":
                    ParallelUse parallelUse = new ParallelUse();
                    var t1 = new Task(parallelUse.SquareEachValueDefault);
                    var t2 = new Task(parallelUse.SquareEachValueOptmized);
                    t1.Start();
                    t2.Start();
                    Task.WaitAll(t1, t2);

                    output = string.Empty;
                    explanation = "This approach shows two parallel code executing a math operation, however one of them is optimized to execute by chunk, which means it will resuse / share some memory between the chunks";
                    Console.WriteLine(explanation);
                    Console.WriteLine("------------> end of the execution");
                    showInto(ref output);
                    break;
                case "13":
                    //the async creation is guarantee
                    var one = AsyncFactoryObjOne.CreateAsyncObject();

                    //the async creation is guarantee
                    var two = new AsyncFactoryObjTwo();
                    if (two is IAsyncInit ai){
                        await ai.InitTask;
                    }


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
        Console.WriteLine(string.Empty);
        Console.WriteLine(string.Empty);
        Console.WriteLine(string.Empty);
        Console.WriteLine("Type the number of one of the algorithms bellow");
        Console.WriteLine("--------------------------------------------------------");
        Console.WriteLine(" 1:    async task example with cancellation token");
        Console.WriteLine(" 2:    async task example with many cancellation token");
        Console.WriteLine(" 3:    simple credit/debit async operations");
        Console.WriteLine(" 4:    example with read/write lock");
        Console.WriteLine(string.Empty);
        Console.WriteLine("Concurrent collections examples");
        Console.WriteLine("--------------------------------------------------------");
        Console.WriteLine(" 5:    using concurrent dictionary");
        Console.WriteLine(" 6:    using concurrent queue");
        Console.WriteLine(" 7:    using concurrent stack");
        Console.WriteLine(" 8:    using concurrent bag");
        Console.WriteLine(string.Empty);
        Console.WriteLine("Task coordinator examples");
        Console.WriteLine("--------------------------------------------------------");
        Console.WriteLine(" 9:    using continuation to manage task orchestration");
        Console.WriteLine("10:    using Barrier to manage synchronization");
        Console.WriteLine("11:    using SemaphoreSlim to manage multiples");
        Console.WriteLine("12:    using Parallel with chunks");




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
