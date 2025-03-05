using System.Collections.Concurrent;

namespace playground_csharp
{
    internal class UseOfConcurrentCollections
    {
        private ConcurrentDictionary<string, string> asyncCapitals = new ConcurrentDictionary<string, string>();

        private void AddParis()
        {
            bool success = asyncCapitals.TryAdd("France", "Paris");
            string who = Task.CurrentId.HasValue ? ($"TaskID: {Task.CurrentId.Value}") : "Main Thread";
            Console.WriteLine($"{who} {(success ? "added" : "did not add")} the element.");
        }

        public void executionDictionary()
        {
            /*This section, there are two ways which call the same method, one sync another async, but the dictionary is the same, then the object knows how to deal with this two concurrency without throwing an error of key. */
            Task.Factory.StartNew(AddParis).Wait();
            AddParis();

            /* The following exemplo shows how to add or update and handling the old value if it was an update.*/
            asyncCapitals["Brazil"] = "Brasilia"; //added as usual
            asyncCapitals.AddOrUpdate("Brazil", "São Paulo",
                (k, old) => old + " --> São Paulo"); //added with verification of insert or update with a function as outrput with the old value, useful for many threads using the same dictionary.
            Console.WriteLine($"The capital of Brazil is {asyncCapitals["Brazil"]}");

            /*Similarly but with a get if exist or just insert. */
            var capOfArgentina = asyncCapitals.GetOrAdd("Argentina", "Buenos Aires");
            Console.WriteLine($"the capital of Argentina is {capOfArgentina}");

            /*As showed before with update, this example shows the removed value.*/
            const string toRemove = "Argentina";
            string removed;

            var didRemoved = asyncCapitals.TryRemove(toRemove, out removed);
            if (didRemoved)
            {
                Console.WriteLine($"The capital {removed} was removed");
            }
            else
            {
                Console.WriteLine($"Fail to remove the capital {removed}");
            }
        }

        /*Similarly to dictionary, this object has the fearture of try, then it will be able to handle multiple process trying to access at the same time. */
        public void executionConcurrentQueue()
        {
            var queue = new ConcurrentQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            int result;

            if (queue.TryDequeue(out result))
            {
                Console.WriteLine($" removed element {result}");
            }

            if (queue.TryPeek(out result))
            {
                Console.WriteLine($"front element is {result}");
            }
        }

        /*in this case the default value of the queue is zero, so we are trying to get 5 elements in the object, this five elements could be get by another process, in normal queue this current process would get an error outOfRangeException, but this treatment will bring the default value 0 if the queue went out of range. */
        public void executionStack()
        {
            var stack = new ConcurrentStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(2);

            int result;

            if (stack.TryPeek(out result))
                Console.WriteLine($"{result} is on top");

            if (stack.TryPop(out result))
                Console.WriteLine($"{result} was popped");

            var items = new int[5];


            if (stack.TryPopRange(items, 0, 5) > 0)
            {
                var text = string.Join(",", items.Select(x => x.ToString()));
                Console.WriteLine($"Popped these items: {text}");
            }
        }


        /*This example demonstrates that the object knows how to deal with many threads adding and peeking the itens in a sequence by processId and item, even if it's happening concurently.*/
        public void executionConcurentBag()
        {
            var bag = new ConcurrentBag<int>();
            var tasks = new List<Task>();

            for (int i = 0; i < 5; i++)
            {
                var toAdd = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    bag.Add(toAdd);
                    Console.WriteLine($"{Task.CurrentId} has added {toAdd}");
                    int result;
                    if (bag.TryPeek(out result))
                        Console.WriteLine($"{Task.CurrentId} has peeked the value {result}");
                }));
            }
            Task.WaitAll(tasks.ToArray());

            // In this example, the value will change everytime I run it, because of the assyncorunous process.
            int last;
            if (bag.TryTake(out last))
            {
                Console.WriteLine($"I got {last}");
            }
        }

    }

    /*
     The blocking collection its a wrpper from a collection with some plus features to deal with concurrency and consuming and producing concept assyncronously.
     */
    internal class SimpleBlockingCollection
    {
        BlockingCollection<int> messages = new BlockingCollection<int>();
        CancellationTokenSource cts = new CancellationTokenSource();
        Random random = new Random();

        public void executionBlockingCollection()
        {
            var producer = Task.Factory.StartNew(runProducer);
            var consumer = Task.Factory.StartNew(runConsumer);

            try
            {
                Task.WaitAll(new[] { producer, consumer });
            }
            catch (AggregateException ex)
            {
                ex.Handle(x => true);
            }

            Console.ReadKey();
            cts.Cancel();
        }

        private void runConsumer()
        {
            //here its the magix, this method will execute in the loop when an item was added, like subscribing in a rabbit.
            foreach (var message in messages.GetConsumingEnumerable())
            {
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"-{message}\t");
                Thread.Sleep(random.Next(1000));
            }
        }
        private void runProducer()
        {
            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();
                int i = random.Next(100);
                messages.Add(i);
                Thread.Sleep(random.Next(1000));
            }
        }
    }
}