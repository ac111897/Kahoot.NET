﻿using System.Net.WebSockets;
using Kahoot.NET.Client;
using Kahoot.NET.Game.Internal.Request;
using Microsoft.Extensions.Logging;

namespace Kahoot.NET.Game.Client;

public partial class QuizCreator : IQuizCreator
{
    private ClientWebSocket Socket { get; }
    private ILogger<IQuizCreator> Logger { get; }
    private readonly ConnectionObject _sessionObject;

    public QuizCreator(ILogger<IQuizCreator> logger)
    {
        Socket = new();
        Logger = logger;
        _sessionObject = new()
        {
            id = 1,
        };
    }
    

    public async Task<int> CreateSessionAsync(string quizUrl, GameConfiguration? configuration = null, CancellationToken cancellationToken = default)
    {
        configuration ??= new GameConfiguration();

        (int code, string webSocket) = await QuizInitializer.CreateSessionAsync(quizUrl, configuration);

        await CreateHandshakeAsync(code, webSocket);

        var thread = new Thread(async () => await ReceiveAsync());

        thread.Start();

        return code;
    }
}