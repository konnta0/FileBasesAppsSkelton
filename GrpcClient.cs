#!/usr/bin/env -S dotnet run
#:package MagicOnion.Client@7.0.6
#:package Grpc.Net.Client@2.71.0
#:property PublishAot=false

using Grpc.Net.Client;
using MagicOnion;
using MagicOnion.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = MagicOnionClient.Create<IGreeterService>(channel);

var result = await client.HelloAsync("World");
Console.WriteLine(result);

public interface IGreeterService : IService<IGreeterService>
{
    UnaryResult<string> HelloAsync(string name);
}
