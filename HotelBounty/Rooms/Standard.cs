using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
public class Standard : Room
{
    public Standard(Occupancy occupancy, double price, bool climatization, bool isCleaned, bool isAvailable)
        : base(occupancy, price, climatization, isCleaned, isAvailable)
    {
    }

    public Standard()
    {
        
    }
}