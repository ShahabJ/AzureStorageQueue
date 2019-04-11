using System;
using System.Linq;

namespace AzureQueueSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new Queue("queue2");
            //q.CreateQueue().Wait();
            //q.InsertMessage(RandomString(1000)).Wait();
            //q.PeekNextMessage().Wait();
            //q.ChangeContent().Wait();
            q.DequeueMessage().Wait();
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
