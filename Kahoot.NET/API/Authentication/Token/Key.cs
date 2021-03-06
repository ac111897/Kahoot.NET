namespace Kahoot.NET.API.Authentication.Token;

/// <summary>
/// Creates the websocket key
/// </summary>
internal class Key
{
    /// <summary>
    /// Creates the WebSocket key by using x-kahoot-session-token and the challenge function
    /// </summary>
    /// <param name="sessionHeaderToken"></param>
    /// <param name="challengeFunction"></param>
    /// <returns>The WebSocket key used to connection</returns>
    internal static string Create(ReadOnlySpan<char> sessionHeaderToken, ReadOnlySpan<char> challengeFunction)
    {
        var header = Header.Create(sessionHeaderToken);
        var challenge = Challenge.CreateToken(challengeFunction);

        // new span to allocate for the token
        Span<char> merged = new char[header.Length];

        for (int i = 0; i < header.Length; i++) // loop every character in the header
        {
            // black magic stuff
            var character = (int)header[i];
            var mod = (int)challenge[i % challenge.Length];
            var decoded = character ^ mod;
            merged[i] = Convert.ToChar(decoded);
        }
        return new string(merged); // allocate into a string
    }
}
