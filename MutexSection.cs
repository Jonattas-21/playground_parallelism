
using System.Threading;

namespace playground_csharp
{
    internal class MutexSection
    {
        class BankingAccount
        {
            public BankingAccount()
            {
                BottomLine = 0;
            }

            private int _balance;

            public int BottomLine
            {
                get { return _balance; }
                private set { _balance = value; }
            }

            internal void Deposit(int amount)
            {
                BottomLine += amount;
            }

            internal void Withdraw(int amount)
            {
                BottomLine -= amount;
            }

            internal void Transfer(int amount, BankingAccount to)
            {
                this._balance -= amount;
                to._balance += amount;
            }
        }
        /*
         In this example three differente things happens using mutex, the first ans second two mutex controls (lock and release) the mutex for the transactions deposit and withdraw, this situations emulate many tasks trying to do math with a number, but one per time to ensure the correct final result, and in the end another bunch of tasks transfer the money to one account to another, the goal here it's to lock two diferents mutex for two entities.
         */
        internal static void SimpleBalanceCalculation(int cashIn, int cashOut)
        {
            BankingAccount ba = new BankingAccount();
            BankingAccount ba2 = new BankingAccount();
            var tasks = new List<Task>();
            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; ++j)
                    {
                        var haveLocker = mutex.WaitOne();

                        try
                        {
                            ba.Deposit(cashOut);
                        }
                        finally
                        {
                            if (haveLocker)
                                mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; ++j)
                    {
                        var haveLocker = mutex2.WaitOne();

                        try
                        {
                            ba2.Deposit(cashIn);
                        }
                        finally
                        {
                            if (haveLocker)
                                mutex2.ReleaseMutex();
                        }
                    }

                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool haveLock = WaitHandle.WaitAll(new[] { mutex, mutex2 });
                        try
                        {
                            ba.Transfer(1, ba2);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex2.ReleaseMutex();
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"The balance is: {ba.BottomLine}");
            Console.WriteLine($"The balance2 is: {ba2.BottomLine}");
        }
    }
}
