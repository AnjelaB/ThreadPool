using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool_Implemention
{
    /// <summary>
    /// ThreadPool class for collection of threads.
    /// </summary>
    public static class ThreadPool
    {
        /// <summary>
        /// Max count of Threads in threadPool collection.
        /// </summary>
        private static int maxCountsOfThread;

        /// <summary>
        /// List of TaskItems.
        /// </summary>
        private static List<TaskItem> threadPool;

        /// <summary>
        /// Queue of tasks that will be given to solve with threadPool.
        /// </summary>
        private static Queue<WaitCallBack> queueWaitCallBack;

        /// <summary>
        /// Delegate for tasks.
        /// </summary>
        public delegate void WaitCallBack();

        /// <summary>
        /// Struct that contains thread and WaitCallBack type method that will be called in that thread.
        /// </summary>
        struct TaskItem
        {
            public WaitCallBack taskHandle;
            public Thread thread;
        }

        /// <summary>
        /// Method to initialize any fields of ThreadPool.
        /// </summary>
        /// <param name="max">Max count of thread that default is 10.</param>
        public static void InitializeThreadPool(int max=10)
        {
            maxCountsOfThread = max;
            threadPool = new List<TaskItem>();
            queueWaitCallBack = new Queue<WaitCallBack>();
        }

        /// <summary>
        /// Method that add given task to queue.
        /// </summary>
        /// <param name="callBack"></param>
        public static void QueueUserWorkItem(WaitCallBack callBack)
        {
            lock (queueWaitCallBack)
            {
                queueWaitCallBack.Enqueue(callBack);
                Monitor.Pulse(queueWaitCallBack);
            }
        }

        /// <summary>
        /// Method to Start threadPool process.
        /// </summary>
        public static void Run()
        {

            while (threadPool.Count <= maxCountsOfThread)
            {
                TaskItem task = new TaskItem();
                task.taskHandle = () => { };
                task.thread = new Thread(() =>
                  {
                      while (true)
                      {
                          task.taskHandle();
                          lock (queueWaitCallBack)
                          {
                              if (queueWaitCallBack.Count == 0)
                              {
                                  Monitor.Wait(queueWaitCallBack);
                              }


                              while (queueWaitCallBack.Count > 0 && queueWaitCallBack.Peek() == null)
                                  queueWaitCallBack.Dequeue();
                              if (queueWaitCallBack.Count > 0)
                                  task.taskHandle = queueWaitCallBack.Dequeue();
                          }
                      }
                  });
                task.thread.Start();
                threadPool.Add(task);
            }
            
        }
    }
}
