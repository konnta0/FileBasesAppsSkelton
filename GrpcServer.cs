#!/usr/bin/env -S dotnet run
#:sdk Microsoft.NET.Sdk.Web
#:package MagicOnion.Server@7.0.6
#:property PublishAot=false

using MagicOnion;
using MagicOnion.Server;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, static o => o.Protocols = HttpProtocols.Http2);
});
builder.Services.AddMagicOnion();

var app = builder.Build();
app.MapMagicOnionService();

app.Run();

public interface IGreeterService : IService<IGreeterService>
{
    UnaryResult<string> HelloAsync(string name);
}

public class GreeterService : ServiceBase<IGreeterService>, IGreeterService
{
    public UnaryResult<string> HelloAsync(string name)
    {
        return UnaryResult.FromResult($"Hello, {name}!");
    }
}
