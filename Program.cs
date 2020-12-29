using System;
using System.Threading;
using System.Diagnostics;

namespace lab1
{
    public class Program
    {
        static double[] arr;
        static double[] brr;
        static int N;
        static int M;
        static int K = 1000;

        //args 0 - elements, args 1 - threads
        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            
            N=Int32.Parse(args[0]);
            M=Int32.Parse(args[1]);

            Console.WriteLine("Num of Processors: {2}\nNum of created Threads: {1}\nNum of Elements: {0}\n",
                                N, M, System.Environment.ProcessorCount);
            
            arr = new double[N];
            brr = new double[N];

            Random rand = new Random();
            for (int i = 0; i < N; i++) {
                arr[i] = rand.NextDouble(); 
            }


            sw.Start();
            powSeq();
            sw.Stop();
            Console.WriteLine("Sequential total time: {0}", 
                                sw.Elapsed.TotalMilliseconds);
            sw.Reset();



            Thread[] thrds = new Thread[M];

            int block = N / M;

            int s = 0, e = block;
            
            sw.Start();
            for (int i = 0; i < M; i++) {
                if (i == M - 1) {
                    thrds[i] = new Thread(powBlock);
                    thrds[i].Start(new int[] {s, N - 1});
                }
                else {
                    thrds[i] = new Thread(powBlock);
                    thrds[i].Start(new int[] {s, e - 1});
                }
                s += block;
                e += block;
            }            
            foreach (var thrd in thrds) {
                thrd.Join();
            }
            sw.Stop();
            Console.WriteLine("Block total time: {0}", 
                                sw.Elapsed.TotalMilliseconds);
            sw.Reset();

            
            sw.Start();
            for(int i = 0; i < M; i++) {
                thrds[i] = new Thread(powCycl);
                thrds[i].Start();
            }
            foreach (var thrd in thrds) {
                thrd.Join();
            }
            sw.Stop();
            Console.WriteLine("Cyclic total time: {0}", 
                                sw.Elapsed.TotalMilliseconds);
            sw.Reset();
        }

/* Усложнение обработки каждого элемента
        static void powSeq() {
            for(int i = 0; i < N; i++) {
                for(int j = 0; j<K; j++) {
                     brr[i] += Math.Pow(arr[i], 1.789);
                 }
            }
        }

        static void powBlock(Object indxs) {
            int[] inds = indxs as int[];
            int start = inds[0];
            int end = inds[1];

            for (int i = start; i <= end; i++) {
                for(int j = 0; j<K; j++) {
                     brr[i] += Math.Pow(arr[i], 1.789);
                 }
            }
        }

        static void powCycl() {
            int thrdNum = Thread.CurrentThread.ManagedThreadId;
            for (int i = thrdNum; i < N; i += M) {
                for(int j = 0; j<K; j++) {
                     brr[i] += Math.Pow(arr[i], 1.789);
                 }
            }
        }
*/

/* Неравномерная обработка  
        static void powSeq() {
            for(int i = 0; i < N; i++) {
                for(int j = 0; j<i; j++) {
                    brr[i] += Math.Pow(arr[i], 1.789);
                }
            }
        }

        static void powBlock(Object indxs) {
            int[] inds = indxs as int[];
            int start = inds[0];
            int end = inds[1];

            for (int i = start; i <= end; i++) {
                for(int j = 0; j<i; j++) {
                    brr[i] += Math.Pow(arr[i], 1.789);
                }
            }
        }

        static void powCycl() {
            int thrdNum = Thread.CurrentThread.ManagedThreadId;
            for (int i = thrdNum; i < N; i += M) {
                for(int j = 0; j<i; j++) {
                    brr[i] += Math.Pow(arr[i], 1.789);
                }
            }
        }
*/

        static void powSeq() {
            for(int i = 0; i < N; i++) {
                brr[i] = Math.Pow(arr[i], 1.789);
            }
        }

        static void powBlock(Object indxs) {
            int[] inds = indxs as int[];
            int start = inds[0];
            int end = inds[1];

            for (int i = start; i <= end; i++) {
                brr[i] = Math.Pow(arr[i], 1.789);
            }
        }

        static void powCycl() {
            int thrdNum = Thread.CurrentThread.ManagedThreadId;
            for (int i = thrdNum; i < N; i += M) {
                brr[i] = Math.Pow(arr[i], 1.789);
            }
        }
    }
}
