using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
public class NoPets : Room
{

    private bool _allergyFriendly;

    public bool AllergyFriendly
    {
        get => _allergyFriendly;
        set
        {
            _allergyFriendly = value;
        }
    }

    public NoPets(Occupancy occupancy, double price, bool climatization, bool isCleaned, bool isAvailsble, bool allergyFriendly) 
        : base(occupancy, price, climatization, isCleaned, isAvailsble)
    {
        AllergyFriendly = allergyFriendly;
    }

    public NoPets() { }
}