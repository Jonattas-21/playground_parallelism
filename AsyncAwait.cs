namespace playground_csharp
{
    internal class AsyncAwait
    {
        public int calculateValue()
        {
            Thread.Sleep(5000);
            return 123;
        }

        public async Task<int> calculateValueAsync()
        {
            await Task.Delay(5000);
            return 123;
        }

        public async Task<string> ExecuteInterface()
        {
            /*Simple approach and it will block the current thread
                int n = calculateValue();
                return n.ToString();
            */

            /*With this approach new thread will perform the calculation and it will be a continuation with the task, therefore the thread will not be bloacked. 
            
                var caculation = calculateValueAsync();
                string result = "";
                caculation.ContinueWith(t =>
                {
                    result = t.Result.ToString();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            */

            var caculation = await calculateValueAsync();
            string result = caculation.ToString();


            return result;
        }
    }
}
