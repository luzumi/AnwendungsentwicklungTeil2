using System;
using System.Threading.Tasks;

namespace ThreadingConsole
{
    class Program
    {
        static readonly Object sync = new Object();

        static void Main()
        {
            Console.WriteLine(
                $"Willkommen zum Threading-Spielplatz. Sie haben {Environment.ProcessorCount} Prozessoren. Loslogen mit Enter");
            Console.ReadLine();

            var t = new Task<int>[6];


            foreach (var _ in t)
            {
                CalculateCounter();
            }

            Console.WriteLine("Fertig");
            Console.ReadLine();
        }

        private static void CalculateCounter()
        {
            Task<int>[] t = new Task<int>[6];
            counter = 0;

            t[0] = new Task<int>(() => CountUpV2("T0", 120011));
            t[1] = new Task<int>(() => CountUpV2("T1", 13111));
            t[2] = new Task<int>(() => CountUpV2("T2", 79));
            t[3] = new Task<int>(() => CountUp("T3"));
            t[4] = new Task<int>(() => CountUp("T4"));
            t[5] = new Task<int>(() => CountUp("T5"));
            foreach (var item in t)
            {
                item.Start();
            }

            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < 6)
            {
                Task.Delay(250).Wait();
                //Console.WriteLine("Aktueller Zählerstand: {0}", counter);
            }

            Task.WaitAll(t);
            int summeThreads = 0;
            foreach (var item in t)
            {
                summeThreads += item.Result;
            }

            Console.WriteLine("##############################################################");
            Console.WriteLine("letzter Zählerstand: {0}, summe der Threads: {1}", counter, summeThreads);
        }

        static int counter;

        static int CountUp(string pName)
        {
            int ownCounter = 0;
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < 5)
            {
                lock (sync)
                {
                    ownCounter += 1;
                    counter += 1;
                }
            }

            Console.WriteLine(
                $"Thread: {pName}, intern: {ownCounter}, vergangene Zeit: {(DateTime.Now - startTime).TotalSeconds}");
            return ownCounter;
        }

        static int CountUpV2(string name, int refreshspeed)
        {
            int ownCounter = 0;
            DateTime startTime = DateTime.Now;
            int cachedCounter = 0;
            while ((DateTime.Now - startTime).TotalSeconds < 5)
            {
                ownCounter += 1;
                cachedCounter += 1;
                if (cachedCounter > refreshspeed)
                {
                    lock (sync)
                    {
                        counter += cachedCounter;
                        cachedCounter = 0;
                    }
                }
            }

            counter += cachedCounter;
            Console.WriteLine(
                $"Thread: {name}, intern: {ownCounter}, vergangene Zeit: {(DateTime.Now - startTime).TotalSeconds}");
            return ownCounter;
        }
    }
}
