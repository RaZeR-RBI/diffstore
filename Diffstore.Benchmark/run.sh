rm -f ../python/bench.txt
dotnet build
dotnet run --no-build >> ../python/bench.txt
cd ../python
python plot.py bench.txt
