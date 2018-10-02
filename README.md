[![Master build status](https://ci.appveyor.com/api/projects/status/w7s71q0tpxovhxnh/branch/master?svg=true&passingText=master%20-%20OK&failingText=master%20-%20failing&pendingText=master%20-%20pending)](https://ci.appveyor.com/project/RaZeR-RawByte/diffstore/branch/master) 
[![Develop build status](https://ci.appveyor.com/api/projects/status/w7s71q0tpxovhxnh/branch/develop?svg=true&passingText=develop%20-%20OK&failingText=develop%20-%20failing&pendingText=develop%20-%20pending)](https://ci.appveyor.com/project/RaZeR-RawByte/diffstore/branch/develop)
[![GitHub license](https://img.shields.io/github/license/RaZeR-RBI/diffstore.svg)](https://github.com/RaZeR-RBI/diffstore/blob/master/LICENSE) 
[![NuGet Version](https://img.shields.io/nuget/v/Diffstore.svg)](https://www.nuget.org/packages/Diffstore) 
[![NuGet](https://img.shields.io/nuget/dt/Diffstore.svg)](https://www.nuget.org/packages/Diffstore)

---

### Simple and lightweight key-value storage with snapshot capability
*NOTE: The README and documentation is being worked on. Stay tuned.*

Diffstore is a simple but flexible embeddable key-value storage aimed at
statistical data analysis and caching.

There is two concepts central to this library - **entity** and **snapshot**.
* **Entity** is just a strongly typed key-value pair
* **Snapshot** is a copy of the entity at some time point (can be specified or set automatically)

By default, whenever you make changes to an entity, Diffstore saves a snapshot of
it's previous state. You can specify which fields should not be 'tracked' (not included in snapshots) 
or not saved both in entity and its snapshots (some runtime-related data, for example), 
which makes it useful for statistical applications.

### Project goals
* Simple to use
* Flexible and extensible
* Low resource usage (both memory and storage)
* No additional layers such as caching and connection management

**If you need more features, check out the [Diffstore DBMS](https://github.com/razer-rbi/diffstore-dbms) project.**

### Out-of-box configurations
Currently supported entity formats for the file-based storage are:
- XML
- JSON (powered by [Jil](https://github.com/kevin-montrose/Jil))
- Binary

Available snapshot managers:
- Single file per snapshot (uniform access time)
- Last-first binary files with configurable partitioning (low disk usage, faster access for newer data, read-oriented, GZIP-friendly)

It's possible to extend the engine with other options like relational DB backend (MySQL, Postgres, etc.) and combined storage options (to use the Diffstore as a intermediate caching layer, for example).

### How to use?
- [API Documentation](https://razer-rbi.github.io/diffstore/)

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
