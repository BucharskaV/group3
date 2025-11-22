using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
public class NoPets : Room
{

    private string? _allergyFriendly;

    public string? AllergyFriendly
    {
        get => _allergyFriendly;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if(value.Length > 50) throw new ArgumentException("Description of allergy friendly feature cannot be longer than 50 characters");
            }
            _allergyFriendly = value;
        }
    }

    public NoPets(Occupancy occupancy, double price, string? climatization, string? isCleaned, string? isAvailsble, string? allergyFriendly) 
        : base(occupancy, price, climatization, isCleaned, isAvailsble)
    {
        AllergyFriendly = allergyFriendly;
    }

    public NoPets() { }
}