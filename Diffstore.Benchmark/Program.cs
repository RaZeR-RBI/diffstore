using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Diffstore.Benchmark
{
    class Program
    {
        private static Stopwatch sw = new Stopwatch();
        private const int count = 1000;

        private struct BenchInfo
        {
            public float Min;
            public float Max;
            public float Average;
            public float Total;
            private const string Format = "|{0,10}|{1,10}|{2,10}|{3,10}|";
            private const string Delimiter = "----------";

            public override string ToString() =>
                string.Format(Format, "Min", "Max", "Average", "Per second") + "\n" +
                string.Format(Format, Delimiter, Delimiter, Delimiter, Delimiter) + "\n" +
                string.Format(Format, Min, Max, Average, 1000 / Average);
        }

        private class SampleData
        {
            public int SomeInteger;
            public string SomeString;

            public SampleData() { }
            public SampleData(int value)
            {
                SomeInteger = value;
                SomeString = "Lorem ipsum dolor sit amet";
            }
        }

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(
                Assembly.GetEntryAssembly().Location));
            Run(true); // perform a cold run to JIT-compile the benchmarking code
            Run(false); // perform a warm run and print out the results
        }

        static void Run(bool isCold)
        {
            var benches = new Dictionary<string, Action<IDiffstore<long, SampleData>, int>>() {
                { "Writing entities", (db, step) => db.Save(step, new SampleData(step)) },
                { "Checking existence", (db, step) => db.Exists(step) },
                { "Reading entities", (db, step) => db.Get(step) },
                { "Writing one snapshot", (db, step) => db.Save(step, new SampleData(step + 1)) },
                { "Writing one hundred snapshots", (db, step) => {
                    for (int i = 1; i <= 100; i++)
                        db.Save(step, new SampleData(step + i + 1));
                }},
                { "Reading oldest snapshot", (db, step) => db.GetFirst(step) },
                { "Reading newest snapshot", (db, step) => db.GetLast(step) },
                { "Reading all snapshots", (db, step) => db.GetSnapshots(step).Count() }
            };

            var implementations = new Dictionary<string, IDiffstore<long, SampleData>>() {
                { "Memory storage, LIFO optimized files", new DiffstoreBuilder<long, SampleData>()
                    .WithMemoryStorage()
                    .WithFileBasedEntities()
                    .WithLastFirstOptimizedSnapshots()
                    .Setup()
                },
                { "Disk storage, LIFO optimized files", new DiffstoreBuilder<long, SampleData>()
                    .WithDiskStorage()
                    .WithFileBasedEntities()
                    .WithLastFirstOptimizedSnapshots()
                    .Setup()
                }
            };

            foreach (var impl in implementations)
            {
                if (!isCold) Console.WriteLine($"# {impl.Key}");
                foreach (var pair in benches)
                {
                    var db = impl.Value;
                    var act = pair.Value;
                    var result = MeasureBatch((step) => act(db, step));
                    if (!isCold)
                    {
                        Console.WriteLine($"## {pair.Key}");
                        Console.WriteLine(result);
                        Console.WriteLine();
                    }
                }
            }

            // Cleanup
            Directory.Delete("storage", true);
        }

        static BenchInfo MeasureBatch(Action<int> step)
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            float total = 0;
            for (int i = 1; i <= count; i++)
            {
                var time = MeasureSingle(() => step(i));
                min = Math.Min(min, time);
                max = Math.Max(max, time);
                total += time;
            }
            return new BenchInfo()
            {
                Min = min,
                Max = max,
                Total = total,
                Average = total / count
            };
        }

        static float MeasureSingle(Action act)
        {
            sw.Restart();
            act();
            sw.Stop();
            return (float)sw.ElapsedTicks / TimeSpan.TicksPerMillisecond;
        }
    }
}
