﻿using System.Data;
using System.Text.RegularExpressions;

namespace Kahoot.NET.Parsers;

/// <summary>
/// Calculate the offset for the function to calculate the challenge string
/// </summary>
internal partial class OffsetArithmetic : IValueParser<long>
{
    // Uses regex source generator in .NET 7 instead
    private static Regex InternalRegex { get; } =
#if NET7_0_OR_GREATER
        GenerateRegex();

        [GeneratedRegex(@"\d+(\.\d+)?")]
        internal static partial Regex GenerateRegex();
#else
        new(@"\d+(\.\d+)?", RegexOptions.Compiled);
#endif
    private static DataTable Table { get; } = new();

    public long Parse(ReadOnlySpan<char> input)
    {
        var result = Table.Compute(SanitiseToDecimal(input).ToString(), null);

        return Convert.ToInt64((decimal)result);
    }

    internal static ReadOnlySpan<char> SanitiseToDecimal(ReadOnlySpan<char> value)
    {
        return InternalRegex.Replace(value.ToString(), m =>
        {
            var slice = m.ValueSpan;
            return slice.Contains('.') ? slice.ToString() : string.Format("{0}.0", slice.ToString());
        });
    }
}
