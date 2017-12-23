# Diffstore [![Build status](https://ci.appveyor.com/api/projects/status/w7s71q0tpxovhxnh?svg=true)](https://ci.appveyor.com/project/RaZeR-RawByte/diffstore)

### Simple and lightweight storage engine for statistical data
The README and documentation is being worked on. Stay tuned.

### Project goals
* Simple to use
* Flexible and extensible
* Low resource usage (both memory and storage)
* No additional layers such as caching and connection management

### Out-of-box configurations
Currently supported entity formats for the file-based storage are:
- XML
- JSON
- Binary

Available snapshot managers:
- Single file per snapshot (uniform access time)
- Last-first binary files with configurable partitioning (low disk usage, faster access for newer data, read-oriented)

It's possible to extend the engine with other options like relational DB backend (MySQL, Postgres, etc.) and combined storage options (to use the Diffstore as a intermediate caching layer, for example).

### How to use?
The documentation is being worked on (using my other project for that purpose - **dotbook**).

You can check out the [test source which covers the basics](Diffstore.Tests/DiffstoreTest.cs).

### Notable uses
This storage engine was developed and is now used for the [SteamTrends](https://steamtrends.info/) service, which collects statistical data about Steam games every day and tracks this data for more than 19000 entries on a low-spec Linux-based VPS.

Using this library? Contact me and you'll be added!

### Benchmarks
Benchmark was performed 100 times with a warmup run before the measures to ensure JIT compilation of the benchmarking code.

**Machine specifications**:
- [HGST HTS721010A9E630](https://www.hgst.com/products/hard-drives/travelstar-7k1000) (7200 RPM, 32MB buffer, 4 ms average latency)
- i7 2630QM
- Debian 8 Jessie

![Alt text](python/images/Reading%20entities.png)

![Alt text](python/images/Reading%20all%20snapshots.png)

![Alt text](python/images/Reading%20oldest%20snapshot.png)

![Alt text](python/images/Reading%20newest%20snapshot.png)

![Alt text](python/images/Checking%20existence.png)

![Alt text](python/images/Writing%20entities.png)

![Alt text](python/images/Writing%20one%20snapshot.png)

![Alt text](python/images/Writing%20one%20hundred%20snapshots.png)
