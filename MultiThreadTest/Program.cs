using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace MultiThreadTest
{
    class Program
    {



        private static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);

            return st.GetFrame(0).GetFileLineNumber();
        }
#if false
        static int Main(string[] args)
        {
            Console.WriteLine("我是主线程，线程ID：{0}, Line:{1}", Thread.CurrentThread.ManagedThreadId, GetLineNum());
#if true
            TestAsync();
#endif
            Console.ReadLine();
            return 0;
        }

        static async Task TestAsync()
        {
            Console.WriteLine("调用GetReturnResult()之前，线程ID：{0}。当前时间：{1}, Line{2}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"), GetLineNum());
            var name = GetReturnResult();
            Console.WriteLine("调用GetReturnResult()之后，线程ID：{0}。当前时间：{1}, Line{2}", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"), GetLineNum());
            Console.WriteLine("得到GetReturnResult()方法的结果：{0}。当前时间：{1} Line{2}", await name, DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"), GetLineNum());
        }

        static async Task<string> GetReturnResult()
        {
            Console.WriteLine("执行Task.Run之前, 线程ID：{0} Line{1}", Thread.CurrentThread.ManagedThreadId, GetLineNum());
            return await Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("GetReturnResult()方法里面线程ID: {0} Line{1}", Thread.CurrentThread.ManagedThreadId, GetLineNum());
                return "我是返回值";
            });
        }
#endif


        private static int TestAsyncFunc(int param)
        {
            for (int i = 0; i < param; ++i)
            {
                
                Console.WriteLine("Child Thread ID {0} Running... param{1} Repeat {3} LINE {2}", Thread.CurrentThread.ManagedThreadId,param, GetLineNum(), i);
                Thread.Sleep(500);
                

            }
            return 1024;            
        }

        static Task<int> TestAsyncFuncAsync(int param)
        {
            //Func<int> tFunc = 
            return Task.Run<int>(()=> {
                return TestAsyncFunc(param);
            });
        }

        private async static void CallerWithAsync(int param)
        {
            Console.WriteLine("Before Call Async Func... Param {0} Line {1}", param, GetLineNum());
            //int res = await TestAsyncFuncAsync(param);
            var res = await TestAsyncFuncAsync(param);
            Console.WriteLine("After Call Async Func... Param {0} Line {1}", param, GetLineNum());
        }

        public static int Main()
        {
            Random rd = new Random();
            while (true)
            {
                int r = rd.Next(1, 10);
                
                Console.WriteLine("Here Is MainThread Running... LINE {0}", Thread.CurrentThread.ManagedThreadId, GetLineNum());
                if (r < 5)
                {
                    CallerWithAsync(r);
                } else
                {
                    // Do Nothing
                }
                Thread.Sleep(1000);
            }
            return 0;
        }

    }

}
