using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
public class Standard : Room
{
    public Standard(int roomNumber, Hotel hotel, Occupancy occupancy, double price, bool climatization, bool isCleaned, bool isAvailable)
        : base(roomNumber, hotel, occupancy, price, climatization, isCleaned, isAvailable)
    {
    }

    public Standard()
    {
        
    }
}