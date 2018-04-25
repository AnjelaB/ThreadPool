using System;


namespace ThreadPool_Implemention
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            ThreadPool.InitializeThreadPool(10);
            ThreadPool.Run();
            for(int i = 0; i < 10; i++)
            {
                var k = rnd.Next(10,20);
                ThreadPool.QueueUserWorkItem(() => Console.WriteLine(Fibonacci(k)));
            }
        }

        static int Fibonacci(int index)
        {
            if (index == 0)
            {
                return 0;
            }
            else if (index == 1)
            {
                return 1;
            }
            else
                return Fibonacci(index - 1) + Fibonacci(index - 2);
        }
    }
}
