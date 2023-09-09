using BenchmarkDotNet.Running;
using MyBoards.Benchmark;

Console.WriteLine("Start!");

BenchmarkRunner.Run<TrackingBenchmark>();

Console.WriteLine("End!");
