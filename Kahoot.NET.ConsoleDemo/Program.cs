﻿using Kahoot.NET;
using Kahoot.NET.Client;
using Kahoot.NET.Internals.Connection;
using Kahoot.NET.Internals.Parsers;
using Kahoot.NET.Internals.Connection.Token;
using Kahoot.NET.Shared;
using Kahoot.NET.FluentBuilder;
using System.Net.WebSockets;
using Kahoot.NET.Internals.Messages;
using System.Text;

namespace Kahoot.NET.ConsoleDemo;

public class Program
{
    public static async Task Main(string[] args)
    {
        await RunConnectionTestAsync(int.Parse(Console.ReadLine()!));
    }

    public static async Task RunConnectionTestAsync(int code)
    {
        var (res, header) = await ConnectionHelper.CreateSessionResponseAsync(code, new());

        Console.WriteLine($"Header before transformation:\n\n {header}");

        var head = HeaderToken.CreateHeaderToken(header).ToString();

        Console.WriteLine($"Header After:\n\n{head}");


        Console.WriteLine($"Challenge before transformation:\n\n {res!.Challenge}");

        Console.WriteLine($"\nOffset calculated:\n{new OffsetParser().Parse(res.Challenge.AsSpan().RemoveWhitespace())}");


        Console.WriteLine(ChallengeToken.CreateToken(res.Challenge).ToString());


        var result = await Token.CreateTokenAsync(code, new());
        Console.WriteLine(result.ToString());
    }
}
