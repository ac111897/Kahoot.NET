﻿namespace Kahoot.NET.API.Responses;

internal class SessionResponse
{
    /// <summary>
    /// The game mode of the quiz
    /// </summary>
    [JsonPropertyName("gameMode")]
    public string? GameMode { get; set; }

    /// <summary>
    /// If collaborations are enabled in the quiz
    /// </summary>
    [JsonPropertyName("collaborations")]
    public bool Collabs { get; set; }

    /// <summary>
    /// The live game id of this quiz
    /// </summary>
    [JsonPropertyName("liveGameId")]
    public string? LiveGameId { get; set; }

    /// <summary>
    /// The challenge string used to decode the token to connect to the websocket
    /// </summary>
    [JsonPropertyName("challenge")]
    public string? Challenge { get; set; }

    /// <summary>
    /// If two factor authentication is enabled on the quiz
    /// </summary>
    [JsonPropertyName("twoFactorAuth")]
    public bool TwoFactorAuthentication { get; set; }

    /// <summary>
    /// If the namerator is enabled on the quiz
    /// </summary>
    [JsonPropertyName("namerator")]
    public bool Namerator { get; set; }

    /// <summary>
    /// If smart practice is enabled
    /// </summary>
    [JsonPropertyName("smartPractice")]
    public bool SmartPractice { get; set; }

    /// <summary>
    /// If participant id is enabled
    /// </summary>
    [JsonPropertyName("participantId")]
    public bool ParticipantId { get; set; }

    [JsonIgnore]
    public bool Success { get; set; }

    [JsonIgnore]
    public string? WebSocketKey { get; set; }
}
