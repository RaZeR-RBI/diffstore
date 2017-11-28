using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Jil;

namespace Diffstore.Benchmark
{
    class Program
    {
        private static Stopwatch sw = new Stopwatch();
        private const int count = 100;

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

        private class Result
        {
            public readonly Dictionary<string, Dictionary<string, float[]>> Value =
                new Dictionary<string, Dictionary<string, float[]>>();

            public void Add(string bench, string impl, float[] results)
            {
                if (!Value.ContainsKey(bench))
                    Value.Add(bench, new Dictionary<string, float[]>());

                Value[bench].Add(impl, results);
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
                { "Binary LIFO", new DiffstoreBuilder<long, SampleData>()
                    .WithDiskStorage()
                    .WithFileBasedEntities()
                    .WithLastFirstOptimizedSnapshots()
                    .Setup()
                },
                { "XML Single File", new DiffstoreBuilder<long, SampleData>()
                    .WithDiskStorage()
                    .WithFileBasedEntities(FileFormat.XML)
                    .WithSingleFileSnapshots(FileFormat.XML)
                    .Setup()
                },
                { "JSON Single File", new DiffstoreBuilder<long, SampleData>()
                    .WithDiskStorage()
                    .WithFileBasedEntities(FileFormat.JSON)
                    .WithSingleFileSnapshots(FileFormat.JSON)
                    .Setup()
                }
            };

            var result = new Result();
            foreach (var impl in implementations)
            {
                Cleanup();
                foreach (var pair in benches)
                {
                    var db = impl.Value;
                    var act = pair.Value;
                    var times = MeasureBatch((step) => act(db, step));
                    result.Add(pair.Key, impl.Key, times);
                }
            }
            if (!isCold) Console.WriteLine(JSON.Serialize(result));
            Cleanup();
        }

        static void Cleanup()
        {
            if (Directory.Exists("storage")) Directory.Delete("storage", true);
        }

        static float[] MeasureBatch(Action<int> step)
        {
            var times = new float[count];
            for (int i = 1; i <= count; i++)
                times[i - 1] = MeasureSingle(() => step(i));

            return times;
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
