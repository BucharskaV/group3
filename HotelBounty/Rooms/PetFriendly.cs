using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
public class PetFriendly : Room
{
    private string _petFeeders;

    public string PetFeeders
    {
        get => _petFeeders;
        set
        {
            if(string.IsNullOrEmpty(value)) 
                throw new ArgumentException("The pet feeders cannot be null or empty.");
            _petFeeders = value;
        }
    }

    private static int _maxPetsAllowed = 3;

    public int MaxPetsAllowed
    {
        get => _maxPetsAllowed;
        set
        {
            if(value > 3) throw new ArgumentException("The amount of pets cannot be more then 3");
            _maxPetsAllowed = value;
        }
    }

    public PetFriendly(Occupancy occupancy, double price, bool climatization, bool isCleaned, bool isAvailable, string petFeeders, int maxPetsAllowed)
        : base(occupancy, price, climatization, isCleaned, isAvailable)
    {
        PetFeeders = petFeeders;
        MaxPetsAllowed = maxPetsAllowed;
    }

    public PetFriendly() { }
    
}