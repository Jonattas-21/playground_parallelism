﻿
namespace playground_csharp
{
    internal class AsyncFactoryObjOne
    {
        /* 
         Bellow there is a pproach to initialize asyncronouly a object using the factory patters, so, the constructor it's private, and there is a methos to create the obj InitAsyncObj() which will do dome things and it's also private, and the function to explosed function CreateAsyncObject() that will create the class and init it.
         
         */

        private AsyncFactoryObjOne() { }

        private async Task<AsyncFactoryObjOne> InitAsyncObj()
        {
            //doing many stuffs here
            Console.WriteLine($"Creating object One with the taskID {Task.CurrentId}");
            await Task.Delay(3000);
            return this;
        }

        public static Task<AsyncFactoryObjOne> CreateAsyncObject()
        {
            var result = new AsyncFactoryObjOne();
            Console.WriteLine($"object one returned with the taskID {Task.CurrentId}");
            return result.InitAsyncObj();
        }
    }

    public interface IAsyncInit
    {
        Task InitTask { get; }
    }

    internal class AsyncFactoryObjTwo : IAsyncInit
    {
        public AsyncFactoryObjTwo()
        {
            InitTask = InitAsync();
            Console.WriteLine($"object two returned with the taskID {Task.CurrentId}");
        }

        public Task InitTask { get; }

        public async Task InitAsync()
        {
            Console.WriteLine($"Creating object two with the taskID {Task.CurrentId}");
            await Task.Delay(3000);
        }
    }
}
