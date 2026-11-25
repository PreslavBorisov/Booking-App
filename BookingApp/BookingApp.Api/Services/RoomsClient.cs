using System.Net;

namespace BookingApp.Api.Services;

public sealed class RoomsClient(HttpClient client)
{
    public async Task<BookableRoom?> GetRoomAsync(int roomId, CancellationToken ct)
    {
        using var response = await RequestRoomAsync(roomId, ct);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookableRoom>(cancellationToken: ct)
            ?? throw new HttpRequestException("Rooms API returned an empty response.");
    }

    private async Task<HttpResponseMessage> RequestRoomAsync(int roomId, CancellationToken ct)
    {
        try
        {
            return await client.GetAsync($"api/room/{roomId}", ct);
        }
        catch (OperationCanceledException ex) when (!ct.IsCancellationRequested)
        {
            throw new TimeoutException("The Rooms API timed out.", ex);
        }
    }
}

public sealed record BookableRoom(string Name, bool IsActive, string? ImageUrl);
