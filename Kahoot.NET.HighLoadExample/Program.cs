﻿using Kahoot.NET.Client;
using Kahoot.NET.Client.Events;
using Kahoot.NET.API.Shared;
using ParadoxTerminal;

namespace Kahoot.NET.HighLoadExample;

public class Program
{
    private const int BatchAmount = 30;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Enter game code: ");

        uint code = Terminal.ReadUInt32();

        Console.WriteLine("Enter number of clients");

        var count = Terminal.ReadInt32(1, 10_000); // decent limit

        var clients = new List<IKahootClient>(count);

        for (int i = 0; i < count; i++)
        {
            clients.Add(new KahootClient());
        }

        var tasks = new List<Task>();

        foreach (var client in clients)
        {
            client.Joined += Client_Joined;
            client.QuestionReceived += Client_QuestionReceived;
            tasks.Add(client.JoinAsync(code, Random.Shared.Next(0, 999_999_999).ToString()));

            if (tasks.Count == BatchAmount)
            {
                await Task.WhenAll(tasks);
                await Task.Delay(800);
                tasks.Clear();
            }
        }

        if (tasks.Count > 0)
        {
            await Task.WhenAll(tasks);
        }

        await Task.Delay(-1);
    }

    private static async Task Client_QuestionReceived(object? sender, QuestionReceivedArgs questionArgs)
    {
        if (questionArgs.ShouldIgnore)
        {
            return;
        }

        if (sender is not IKahootClient client)
        {
            return;
        }

        if (questionArgs.Question.QuestionType == QuestionType.Quiz)
        {
            await client.AnswerAsync(questionArgs.Question, answerIndex: Random.Shared.Next(questionArgs.Question.NumberOfChoices));
        }
    }

    private static Task Client_Joined(object? obj, JoinEventArgs args)
    {
        if (!args.IsSuccess)
        {
            Console.WriteLine(Enum.GetName(args.Result));
        }

        return Task.CompletedTask;
    }
}
