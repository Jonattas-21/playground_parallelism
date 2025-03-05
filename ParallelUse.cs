
using System.Collections.Concurrent;

namespace playground_csharp
{
    internal class ParallelUse
    {
        internal void ParallelVanilla()
        {
            var a1 = new Action(() => Console.WriteLine($"First: {Task.CurrentId}"));
            var a2 = new Action(() => Console.WriteLine($"Second: {Task.CurrentId}"));
            var a3 = new Action(() => Console.WriteLine($"Third: {Task.CurrentId}"));

            //this is create 3 differents tasks
            Parallel.Invoke(a1, a2, a3); //this line means wait all

            //Ten tasks are starting at this point and a wait all.
            Parallel.For(1, 11, (i, state) =>
            {
                try
                {
                    Console.WriteLine($"{i}");
                }
                catch
                {
                    state.Break(); //imediatly stop all the executions
                }
            });
        }

        // Not the  most performance way to calculate.
        internal void SquareEachValueDefault()
        {
            const int count = 100000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];
            var watch = System.Diagnostics.Stopwatch.StartNew();

            //For each value in the list, a new delegate will be created to math the operation, therefore the performance will be low.
            Parallel.ForEach(values, x =>
            {
                results[x] = (int)Math.Pow(x, 2);
            });

            watch.Stop();
            Console.WriteLine($"The function SquareEachValueDefault took: {watch.ElapsedMilliseconds} milliseconds to be executed");
        }

        // The most performence way to do the same thing.
        internal void SquareEachValueOptmized()
        {
            const int count = 100000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // In this line, I'm creating some chunks of 10.000 for the 100.000 that it will be needed to process.
            var part = Partitioner.Create(0, count, 10000);

            // Now the parallel will execute by chunk of 10.000
            Parallel.ForEach(part, range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    results[i] = (int)Math.Pow(i, 2);
                }
            });

            watch.Stop();
            Console.WriteLine($"The function SquareEachValueOptmized took: {watch.ElapsedMilliseconds} milliseconds to be executed");
        }
    }
}
