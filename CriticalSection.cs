
namespace playground_csharp
{
    internal class CriticalSection
    {
        class Balance
        {
            public Balance()
            {
                BottomLine = 0;
            }

            private int _balance;

            public int BottomLine
            {
                get { return _balance; }
                private set { _balance = value; }
            }

            private object locker = new object();

            internal void Deposit(int amount)
            {
                // += is really two operations
                // op1 is temp <- get_Balance() + amount
                // op2 is set_Balance(temp)

                //option one
                lock (locker)
                {
                    BottomLine += amount;
                }

                //option two
                //Interlocked.Add(ref _balance, amount);
            }

            internal void Withdraw(int amount)
            {
                //option one
                lock (locker)
                {
                    BottomLine -= amount;
                }

                //option two
                //Interlocked.Add(ref _balance, -amount);
            }
        }

        /*
         This example shows many perpectives to lock transactons os to apply the concept of mutex for a example of deposit and withdraw.
        -- option one:   Using simply lock (object){} in a closed context.
        -- option two:   Using the static class Interlocked.add() this method it's not usual and has limitations.
        -- option three: Using the class SpinLock, and a ref boolean in this case lock(obj){} it's not necessary.

        The three options do the mutual exclusion approach.
         */
        internal static void SimpleBalanceCalculation(int cashIn, int cashOut)
        {
            Balance balance = new Balance();
            var tasks = new List<Task>();
            SpinLock spinLock = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    //option one
                    //for (int j = 0; j < 1000; ++j)
                    //    balance.Withdraw(cashOut);

                    //option 3
                    var lockTaken = false;
                    try
                    {
                        spinLock.Enter(ref lockTaken);
                        balance.Withdraw(cashOut);
                    }
                    finally
                    {
                        if(lockTaken)
                            spinLock.Exit();    
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    //option one
                    //for (int j = 0; j < 1000; ++j)
                    //    balance.Deposit(cashIn);

                    //option 3
                    var lockTaken = false;
                    try
                    {
                        spinLock.Enter(ref lockTaken);
                        balance.Deposit(cashOut);
                    }
                    finally
                    {
                        if (lockTaken)
                            spinLock.Exit();
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"The balance is: {balance.BottomLine}");
        }
    }
}
