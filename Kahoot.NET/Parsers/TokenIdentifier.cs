﻿namespace Kahoot.NET.Parsers;

/// <summary>
/// Gets the special token from the challenge string to later decode this
/// </summary>
internal class TokenIdentifer : IParser
{
    const char Identifier = '\'';

    public ReadOnlySpan<char> Parse(ReadOnlySpan<char> input)
    {
        int first = input.IndexOf(Identifier);
        int last = input.LastIndexOf(Identifier);

        return input.Slice(++first, last - first);
    }
}
