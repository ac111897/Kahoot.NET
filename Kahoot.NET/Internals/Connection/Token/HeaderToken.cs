﻿namespace Kahoot.NET.Internals.Connection.Token;

/// <summary>
/// Decodes the base 64 string to get the 1st part of the token
/// </summary>
internal class HeaderToken
{
    /// <summary>
    /// Creates the header token
    /// </summary>
    /// <param name="header"></param>
    /// <returns>Header token</returns>
    public static ReadOnlySpan<char> CreateHeaderToken(ReadOnlySpan<char> header)
    {
        Span<byte> span = new byte[Encoding.ASCII.GetByteCount(header)];
        
        Encoding.ASCII.GetBytes(header, span);

        return Encoding.ASCII.GetString(span);
    }
}