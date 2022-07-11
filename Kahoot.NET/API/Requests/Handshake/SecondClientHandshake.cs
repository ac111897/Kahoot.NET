﻿using Kahoot.NET.API.Shared.Adv;
using Kahoot.NET.API.Shared.Extra;

namespace Kahoot.NET.API.Requests.Handshake;

internal class SecondClientHandshake : ClientMessage
{
    public SecondClientHandshake()
    {
        Channel = Channels.Connection;
    }
    /// <summary>
    /// Advice to send to the server
    /// </summary>
    [JsonPropertyName("advice")]
    public TimeoutAdvice Advice { get; set; } = new()
    {
        Timeout = 0
    };

    /// <summary>
    /// Ext with timesync data
    /// </summary>
    [JsonPropertyName("ext")]
    public ExtWithTimesync<long>? Ext { get; set; } 
}
