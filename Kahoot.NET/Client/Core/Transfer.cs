﻿#if DEBUG // see comment in DataProcessing.cs to see what this means
//#define VIEWTEXT
#endif

#if !VIEWTEXT
#pragma warning disable CA1822 // Mark members as static
#endif

using Kahoot.NET.API.Requests;

namespace Kahoot.NET.Client;

public partial class KahootClient
{
    internal async ValueTask SendAsync<TData>(TData data, JsonTypeInfo<TData>? typeInfo = null, CancellationToken cancellationToken = default)
        where TData : class
    {
        Debug.Assert(_ws != null);

        await _senderLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            await _ws.SendAsync(GetData(data, typeInfo), WebSocketMessageType.Text, WebSocketMessageFlags.EndOfMessage, cancellationToken);
        }
        finally
        {
            _senderLock.Release();
        }
    }


    internal byte[] GetData<T>(T data, JsonTypeInfo<T>? typeInfo = null)
    {
#if VIEWTEXT
        string json = typeInfo == null ? 
            JsonSerializer.Serialize(data, _serializerOptions) :
            JsonSerializer.Serialize(data, typeInfo);

        _logger?.LogDebug("[SEND]: {json}", json);
#endif

        if (typeInfo == null)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data, _serializerOptions);
        }

        return JsonSerializer.SerializeToUtf8Bytes(data, typeInfo);
    }

    internal async Task ReceiveAsync()
    {
        if (_ws.State != WebSocketState.Open)
        {
            // TODO: Add error handling
            return;
        }

        while (_ws.State == WebSocketState.Open)
        {
            byte[] bytes = ArrayPool<byte>.Shared.Rent(StateObject.BufferSize);

            Memory<byte> buffer = bytes;

            try
            {
                var result = await _ws.ReceiveAsync(buffer, default);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, default, default);

                    _username = default;

                    return;
                }

                await ProcessAsync(buffer[..result.Count]);
            }
            catch (Exception exception)
            {
                _logger?.LogError("{exceptionMessage}", exception.Message);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes);
            }
        }

        _username = default;
    }

    internal async Task ReplyAsync()
    {
        Bump();
        await SendPacketAsync();
    }

    // common operations
    internal ValueTask SendPacketAsync() => SendAsync(new Packet()
    {
        Id = Interlocked.Read(ref _stateObject.id).ToString(),
        Channel = Channels.Connect,
        Ext = _stateObject.ExtWithTimesync,
        ClientId = _stateObject.clientId,
        ConnectionType = ConnectionInfo.ConnectionType
    });
}
