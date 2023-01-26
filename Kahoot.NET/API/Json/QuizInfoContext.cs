﻿namespace Kahoot.NET.API.Json;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true)]
[JsonSerializable(typeof(QuizInfo))]
internal partial class QuizInfoContext : JsonSerializerContext
{
}
