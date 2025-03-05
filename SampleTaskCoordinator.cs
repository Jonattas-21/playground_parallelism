
namespace playground_csharp
{
    internal class SampleTaskCoordinator
    {
        /* this examples presents a new way to start a tasks after the cpmpletition of some specific tasks. */
        internal void ExecuteContinuation()
        {
            var task = Task.Factory.StartNew(() => "task one");
            var task2 = Task.Factory.StartNew(() => { return "task two"; });

            var task3 = Task.Factory.ContinueWhenAll(new[] { task, task2 },
                tasks =>
                {
                    Console.WriteLine($"Tasks completed:");
                    foreach (var task in tasks)
                        Console.WriteLine($"- {task.Result}");

                    Console.WriteLine("All tasks done!");
                });

            task3.Wait();
        }

        /*
         The barrier concept allows to cooperative runs tasks in paralell, in this example I set up 2 taks to run for the algorithm, and using the method SignalAndWait() if there are 2 tasks signalled the process will run, otherwise it will wait for 2, in other words, the barrier will block all the threads until 2 tasks get signalled.
         */

        Barrier _barrier = new Barrier(2, x =>
        {
            Console.WriteLine($"{x.CurrentPhaseNumber} was complete ");
        });

        internal void ExecuteBarrierSample()
        {
            var task_water = Task.Factory.StartNew(water);
            var task_cup = Task.Factory.StartNew(cup);
        }

        public void water()
        {
            Console.WriteLine("Putting the kettle on (take a bit longer)");
            Thread.Sleep(2000);
            _barrier.SignalAndWait();
            Console.WriteLine("Pooring the water in the cup");
            _barrier.SignalAndWait();
            Console.WriteLine("Putting the kettle away");
        }

        public void cup()
        {
            Console.WriteLine("Finding the nicest cup of the tea (fast)");
            _barrier.SignalAndWait();
            Console.WriteLine("Adding tes");
            _barrier.SignalAndWait();
            Console.WriteLine("Adding sugar");
        }

        /*
         This approach synchronized the threads using a counter  to decrease by wait() ans increase by release(n) 
         */
        internal void ExecuteSemaphoreSlim()
        {
            var semaphore = new SemaphoreSlim(2, 10);

            for (int i = 0; i < 20; i++)
            {
                Task.Factory.StartNew(() => {
                    Console.WriteLine($"Entering task {Task.CurrentId}");
                    semaphore.Wait();
                    Console.WriteLine($"Processing task {Task.CurrentId}");
                });
            }

            while (semaphore.CurrentCount <= 2) {
                Console.WriteLine($"Semaphore count: {semaphore.CurrentCount}");
                Console.ReadKey();
                semaphore.Release(2);
            }
        }
    }
}
