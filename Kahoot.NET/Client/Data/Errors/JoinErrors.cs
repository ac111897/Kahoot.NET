﻿namespace Kahoot.NET.Client.Data.Errors;

/// <summary>
/// Errors that occur when trying to join the game
/// </summary>
public enum JoinErrors
{
    /// <summary>
    /// When you try to join the game but it requires 2fa
    /// </summary>
    GameRequires2fa,
    /// <summary>
    /// When you try to join the game, but someone already has the same name
    /// </summary>
    DuplicateUserName,
    /// <summary>
    /// Caused when the client has become unknown to the server, try rejoining
    /// </summary>
    SessionUnknown,
    /// <summary>
    /// Occurs when the game locks more players from joining
    /// </summary>
    Locked
}
