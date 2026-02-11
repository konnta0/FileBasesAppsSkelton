#!/usr/bin/env -S dotnet run -c Release
#:package BenchmarkDotNet@0.15.8
#:property PublishAot=false

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

var config = DefaultConfig.Instance
    .AddJob(Job.ShortRun.WithToolchain(InProcessEmitToolchain.Instance));

BenchmarkRunner.Run<SortBenchmark>(config);

[MemoryDiagnoser]
public class SortBenchmark
{
    private int[] _data = null!;

    [Params(100, 1000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _data = Enumerable.Range(0, N).Select(_ => random.Next()).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int[] ArraySort()
    {
        var copy = (int[])_data.Clone();
        Array.Sort(copy);
        return copy;
    }

    [Benchmark]
    public int[] ArraySortWithComparer()
    {
        var copy = (int[])_data.Clone();
        Array.Sort(copy, Comparer<int>.Default);
        return copy;
    }

    [Benchmark]
    public int[] LinqOrderByStatic()
    {
        return _data.OrderBy(static x => x).ToArray();
    }

    [Benchmark]
    public int[] LinqOrderBy()
    {
        return _data.OrderBy(x => x).ToArray();
    }

    [Benchmark]
    public int[] SpanSort()
    {
        var copy = (int[])_data.Clone();
        copy.AsSpan().Sort();
        return copy;
    }
}
