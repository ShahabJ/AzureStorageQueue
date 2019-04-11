using System;
using System.Linq;
using System.Threading.Tasks;

namespace AzureQueueSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ExeWithParalleLoop(100);
        }

        public static void ExeWithParalleLoop(int len)
        {
            var q = new Queue("queue2");
            //q.CreateQueue().Wait();
            //q.InsertMessage(RandomString(1000)).Wait();
            //q.PeekNextMessage().Wait();
            //q.ChangeContent().Wait();
            //q.DequeueMessage().Wait();

            var watchMain = System.Diagnostics.Stopwatch.StartNew();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Parallel.For(0, len, index =>
              {
                  watch.Reset();
                  q.PushMessage(RandomString(10000)).Wait();
                  watch.Stop();
                  //Console.WriteLine($"Index={index}");
                  //Console.WriteLine(watch.ElapsedMilliseconds);
              });

            watchMain.Stop();
            Console.WriteLine($"Total Time : {watchMain.Elapsed.Hours}:{watchMain.Elapsed.Minutes}:{watchMain.Elapsed.Seconds}");
        }

        public static void ExeWithoutParalleLoop(int len)
        {
            var q = new Queue("queue2");
            //q.CreateQueue().Wait();
            //q.InsertMessage(RandomString(1000)).Wait();
            //q.PeekNextMessage().Wait();
            //q.ChangeContent().Wait();
            //q.DequeueMessage().Wait();

            var watchMain = System.Diagnostics.Stopwatch.StartNew();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            for (int i = 0; i < len; i++)
            {
                watch.Reset();
                q.PushMessage(i.ToString()).Wait();
                watch.Stop();
                //Console.WriteLine($"Index={i}");
                //Console.WriteLine(watch.ElapsedMilliseconds);
            }

            watchMain.Stop();
            Console.WriteLine($"Total Time : {watchMain.Elapsed.Hours}:{watchMain.Elapsed.Minutes}:{watchMain.Elapsed.Seconds}");
        }


        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
