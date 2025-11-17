namespace HotelBounty.Rooms;

[Serializable]
public class Standart : Room
{
    public Standart(int occupancy, double price, string? climatization, string? isCleaned, string? isAvailable)
        : base(occupancy, price, climatization, isCleaned, isAvailable)
    {
    }

    public Standart() { }
}