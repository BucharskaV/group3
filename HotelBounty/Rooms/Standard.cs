namespace HotelBounty.Rooms;

[Serializable]
public class Standard : Room
{
    public Standard(int occupancy, double price, string? climatization, string? isCleaned, string? isAvailable)
        : base(occupancy, price, climatization, isCleaned, isAvailable)
    {
    }

    public Standard() { }
}