﻿[assembly: InternalsVisibleTo("Kahoot.NET.ConsoleDemo")]

namespace Kahoot.NET.Client;

#pragma warning disable CS1998

/// <summary>
/// First implementation of <see cref="IKahootClient"/>
/// </summary>

public partial class KahootClient : IKahootClient
{
    #region Events
    /// <inheritdoc></inheritdoc>
    public event AsyncEventHandler? OnJoined;
    /// <inheritdoc></inheritdoc>
    public event AsyncEventHandler<QuestionReceivedEventArgs>? OnQuestionReceived;
    /// <inheritdoc></inheritdoc>
    public event AsyncEventHandler? OnQuizStart;
    /// <inheritdoc></inheritdoc>
    public event AsyncEventHandler? OnQuizFinish;
    /// <inheritdoc></inheritdoc>
    public event AsyncEventHandler? OnQuizDisconnect;

    #endregion

    #region Properties
    /// <summary>
    /// <see cref="HttpClient"/> used for some methods to connect
    /// </summary>
    private HttpClient Client { get; }
    /// <summary>
    /// The Kahoot game id to connect to
    /// </summary>
    internal int? GameId { get; set; }

    /// <inheritdoc></inheritdoc>
    public IQuiz? Quiz { get; private set; }

    /// <inheritdoc></inheritdoc>
    public INemesis? Nemesis { get; private set; }
    /// <inheritdoc></inheritdoc>
    public int? SessionId { get; private set; }
    /// <inheritdoc></inheritdoc>
    public string? UserName { get; private set; }
    /// <inheritdoc></inheritdoc>
    public int? TotalScore { get; private set; }
    /// <inheritdoc></inheritdoc>
    public int? ClientId { get; private set; }
    /// <inheritdoc></inheritdoc>
    public GameMode? Mode { get; private set; }

    #endregion

    #region Methods
    /// <inheritdoc></inheritdoc>
    public async Task LeaveAsync(CancellationToken cancellationToken = default)
    {
        if (WebSocket is null)
        {
            return;
        }

        await WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);

        WebSocket.Dispose();
        WebSocket = null;
    }
    /// <inheritdoc></inheritdoc>
    public async Task ReconnectAsync(CancellationToken cancellationToken)
    {

    }
    /// <inheritdoc></inheritdoc>
    public async Task SendFeedbackAsync(Feedback feedBack, CancellationToken cancellationToken = default)
    {
        ThrowIfNotConnected();
    }
    /// <inheritdoc></inheritdoc>
    public async Task AnswerAsync(OneOf<int, string, int[]> id, CancellationToken cancellationToken = default)
    {
        ThrowIfNotConnected();
    }
    /// <inheritdoc></inheritdoc>
    public async Task JoinAsync(int gameCode, string? name = null, CancellationToken cancellationToken = default)
    {

        UserName = name;

        GameId = gameCode;

        Logger?.LogInformation("Received game code, attempting to create handshake");

        await CreateHandshakeAsync(cancellationToken);

        var thread = new Thread(async () => await ExecuteAndWaitForDataAsync());

        thread.Start();

        //AsyncHelper.RunSync(ExecuteAndWaitForDataAsync);
    }

    #endregion
}
