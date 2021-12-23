using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Geko.HttpClientService.Benchmark.Implementations;

namespace Geko.HttpClientService.Benchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        var sw = new Stopwatch();
        var grid = new Grid(90);
        var numberOfCalls = 10;
        var httpClient = new HttpClient();
        var tasks = new List<Task>();

        grid.PrintRow("Avg time for " + numberOfCalls + " requests");
        grid.PrintLine();
        grid.PrintRow("", numberOfCalls + " consecutive", numberOfCalls + " concurrent");
        grid.PrintLine();

        //native, consecutive
        sw.Start();
        for (var i = 0; i < numberOfCalls; i++)
        {
            NativeImplementation.Sample(httpClient).GetAwaiter().GetResult();
        }
        sw.Stop();
        var serial = sw.ElapsedMilliseconds;
        sw.Reset();

        //native, concurrent
        tasks.Clear();
        for (var i = 0; i < numberOfCalls; i++)
        {
            tasks.Add(NativeImplementation.Sample(httpClient));
        }
        sw.Start();
        Task.WhenAll(tasks).GetAwaiter().GetResult();
        sw.Stop();
        var concurrent = sw.ElapsedMilliseconds;
        sw.Reset();

        grid.PrintRow("Native", (serial / (double)numberOfCalls).ToString("0.000ms"), (concurrent / (double)numberOfCalls).ToString("0.000ms"));


        //httpClientService, consecutive
        sw.Start();
        for (var i = 0; i < numberOfCalls; i++)
        {
            HttpClientServiceImplementation.Sample().GetAwaiter().GetResult();
        }
        sw.Stop();
        serial = sw.ElapsedMilliseconds;
        sw.Reset();

        //httpClientService, consecutive
        tasks.Clear();
        for (var i = 0; i < numberOfCalls; i++)
        {
            tasks.Add(HttpClientServiceImplementation.Sample());
        }
        sw.Start();
        Task.WhenAll(tasks).GetAwaiter().GetResult();
        sw.Stop();
        concurrent = sw.ElapsedMilliseconds;
        sw.Reset();

        grid.PrintRow("HttpClientService", (serial / (double)numberOfCalls).ToString("0.000ms"), (concurrent / (double)numberOfCalls).ToString("0.000ms"));
        grid.PrintLine();

        Console.ReadKey();
    }
}
