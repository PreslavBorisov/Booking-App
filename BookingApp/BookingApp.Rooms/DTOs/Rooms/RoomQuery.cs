namespace BookingApp.Api.DTOs.Rooms;

public class RoomQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int? Capacity { get; set; }
    public decimal? MaxPrice { get; set; }

    public string? Search { get; set; }
    public string? Amenity {get; set;}

    public string? SortBy { get; set; }
    public string? SortOrder { get; set; } = "asc";

    public DateOnly? CheckIn { get; set; }
    public DateOnly? CheckOut { get; set; }
}