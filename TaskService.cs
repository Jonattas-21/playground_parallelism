namespace playground_csharp
{
    internal class TaskService
    {
        /*
         This is an exemple of start and cancelation of a task, this can occurs by many reasons the point is, the previous method that creates asn start the task, has the control to cancell it by any reason, this can be usefull because the task once useful, can turn out disposable.
        */
        internal static (Task, CancellationTokenSource) SimpleTaskWithCancelation()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = new Task(async () =>
            {
                Int128 a = 0, b = 1, next;

                for (int infinity = 0; infinity < 2000; infinity++)
                {
                    //better way to cancel
                    //token.ThrowIfCancellationRequested();

                    //verbose way to cancel
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    // Method to calculate the sum of two integers
                    next = a + b;
                    a = b;
                    b = next;

                    await Task.Delay(TimeSpan.FromSeconds(0.5));
                    //Thread.Sleep(1000 * 30);

                    Console.WriteLine($"{next} \t");
                }
            }, token);

            return (task1, cts);
        }

        /*
         Similarly to a simple cancelation token, this stratecy can have more than one cancelation token, and it can be divided in many possible cancelations reasons, once one of this reason/token is triggered, all the process it's canceled, is the example bellow there are three reasons, planned,  preventive and emergency, and this can be usefull to control one task by many perspective by the code.
         */
        internal static (Task, CancellationTokenSource planned, CancellationTokenSource prventive, CancellationTokenSource emergency) SimpleTaskWithCancelationSource()
        {
            var planned = new CancellationTokenSource();
            var preventive = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var paranoid = CancellationTokenSource.CreateLinkedTokenSource(planned.Token, preventive.Token, emergency.Token);
            Console.WriteLine("type any key to cancel");

            var task = new Task(async () =>
            {
                string[] animationFrames = new[]
                        {
                            "[~o      ]",
                            "[ ~o     ]",
                            "[  ~o    ]",
                            "[   ~o   ]",
                            "[    ~o  ]",
                            "[     ~o ]",
                            "[      ~o]",
                            "[      o~]",
                            "[     o~ ]",
                            "[    o~  ]",
                            "[   o~   ]",
                            "[  o~    ]",
                            "[ o~     ]",
                            "[o~      ]",
                    };
                int counter = 0;
                while (true)
                {
                    if (paranoid.IsCancellationRequested)
                    {
                        break;
                    }

                    Console.Write(animationFrames[counter % animationFrames.Length]);
                    await Task.Delay(200); // Delay for 200 milliseconds
                    try
                    {
                        Console.SetCursorPosition(Console.CursorLeft - animationFrames[counter % animationFrames.Length].Length, Console.CursorTop);
                        counter++;
                    }
                    catch { }
                }
            });

            return (task, planned, preventive, emergency);
        }
    }
}
