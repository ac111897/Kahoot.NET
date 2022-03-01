﻿namespace Kahoot.NET.Internal;

internal class InternalConsts
{
    internal const string MinVersion = "1.0";
    internal const string Version = "1.0";
    internal static readonly string[] SupportedConnectionTypes = { "websocket", "long-polling", "callback-polling" };
    internal const string ConnectionType = "websocket";
}