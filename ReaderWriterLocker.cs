using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace playground_csharp
{
    internal class ReaderWriterLocker
    {
        static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();
        //static ReaderWriterLockSlim padLockRec = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /*
         This method such as a mutex, controls access to resources, and these access can be coltrolled by exclusivelly write and read for multiples threads, and there is a option for recurrency. This examples illustrate many threads reading the same resource and one thread writing.
         */

        internal static void SimpleReadLock()
        {
            int x = 0;

            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    padLock.EnterReadLock();
                    Console.WriteLine($"Entering in the readlock, x = {x}");

                    Thread.Sleep(5000);
                    padLock.ExitReadLock();
                    Console.WriteLine($"Exited in the readlock, x = {x}");
                }));
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();
                padLock.EnterWriteLock();
                Console.WriteLine("Write lock acquired");
                int newValue = new Random().Next(10);
                x = newValue;
                Console.WriteLine($"New value setted to x {x}");
                padLock.ExitWriteLock();
                Console.WriteLine("The padlock has been released");
            }
        }

    }
}
