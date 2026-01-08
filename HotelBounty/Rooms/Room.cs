using System.Xml.Serialization;
using HotelBounty.Bookings;
using HotelBounty.Employees;
using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
public  class Room
{
    
    private static List<Room> _roomList = new List<Room>();
    private static int nextId = 1;
    
    private int _roomNumber;
    
    public int RoomNumber
    {
        get => _roomNumber;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Room number must be positive.");

            if (_roomNumber == value)
                return;

            if (_hotel != null)
            {
                _hotel.OnRoomNumberChanging(this, value);
            }

            _roomNumber = value;
        }
    }

    public int Id { get; set; }
    
    private HashSet<Booking> _bookings = new HashSet<Booking>();
    
    public IReadOnlyCollection<Booking> Bookings => _bookings.ToList().AsReadOnly();

    public void AddBooking(Booking booking, bool internalCall = false)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (_bookings.Contains(booking))
            throw new InvalidOperationException("This booking is already assigned to the room.");

        _bookings.Add(booking);

        if (!internalCall)
            booking.SetRoom(this, true);
    }

    public void RemoveBooking(Booking booking, bool internalCall = false)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (!_bookings.Contains(booking))
            throw new InvalidOperationException("This booking is not assigned to the room.");

        _bookings.Remove(booking);

        if (!internalCall)
            booking.SetRoom(null, true);
    }


    internal void SetBookingRoom(Booking booking, bool internalCall = false)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        AddBooking(booking, internalCall);
    }


    private Occupancy _occupancy;

    public Occupancy Occupancy
    {
        get => _occupancy;
        set
        {
            _occupancy = value;
        }
    }
    
    private double _price;

    public double Price
    {
        get => _price;
        set
        {
            if(value < 0)
                throw new ArgumentException("Price value cannot be less than 0");
            _price = value;
        }
    }

    private bool _climatization;
    
    public bool Climatization
    {
        get => _climatization;
        set
        {
            _climatization = value; 
        }
    }

    private bool _isCleaned;
    public bool IsCleaned
    {
        get => _isCleaned;
        set
        {
            _isCleaned = value;
        }
    }

    private bool _isAvailable;
    public bool IsAvailable
    {
        get => _isAvailable;
        set
        { 
            _isAvailable = value;
        }
    }
    
    private Hotel _hotel;

    public Hotel Hotel => _hotel;

    internal void AssignHotel(Hotel hotel)
    {
        _hotel = hotel; 
    }
    
    public void SetHotel(Hotel newHotel, bool internalCall = false)
    {
        if (_hotel == newHotel)
            return;
        
        if (_hotel != null)
        {
            var oldHotel = _hotel;
            _hotel = null;                
            oldHotel.RemoveRoom(RoomNumber, internalCall: true);
        }
        
        if (newHotel != null)
        {
            _hotel = newHotel;           
            newHotel.AddRoom(this, internalCall: true);
        }
    }
    
   // // Deluxe fields
   public bool Terrace { get; set; } = false;
   
   public bool ExtraBed { get; set; } = false;
   
    public List<string> MiniBarFilling { get; private set; } = new List<string>();
    
    public void SetMiniBarFilling(IEnumerable<string> filling)
    {
        if (filling == null)
            throw new ArgumentNullException(nameof(filling));
   
        var list = new List<string>();
        foreach (var f in filling)
        {
            if (string.IsNullOrWhiteSpace(f))
                throw new ArgumentException("The mini bar filling cannot be null, empty, or whitespace.");
            list.Add(f);
        }
   
        if (list.Count == 0)
            throw new ArgumentException("At least one mini bar filling must be added.");
        
        MiniBarFilling = list;
        
    }
    
    // PetFriendly-specific
    public string PetFeeders { get; set; } = null!;
    public int MaxPetsAllowed { get; set; } = 3;
    

    // NoPets-specific
    public bool AllergyFriendly { get; set; } = false;
    
    public RoomType Type { get; set; }

    public Room(int roomNumber, RoomType type, Hotel hotel, Occupancy occupancy,
        double price, bool climatization, bool isCleaned, bool isAvailable,
        bool terrace = false, bool extraBed = false,
        string? petFeeders = null, int maxPetsAllowed = 3,
        bool allergyFriendly = false)
    {
        if (hotel == null)
            throw new ArgumentNullException(nameof(hotel), "When creating the room, the Hotel cannot be null");

        Id = nextId++;
        RoomNumber = roomNumber;
        Type = type;
        Occupancy = occupancy;
        Price = price;
        Climatization = climatization;
        IsCleaned = isCleaned;
        IsAvailable = isAvailable;

        if (type == RoomType.Deluxe)
        {
            Terrace = terrace;
            ExtraBed = extraBed;
        }
        

        if (type == RoomType.PetFriendly)
        {
            if (string.IsNullOrWhiteSpace(petFeeders))
                throw new ArgumentException("Pet feeders must be provided for PetFriendly rooms.");
    
            if (maxPetsAllowed > 3)
                throw new ArgumentException("The amount of pets cannot be more than 3");

            PetFeeders = petFeeders;
            MaxPetsAllowed = maxPetsAllowed;
        }
        
        
        if (type == RoomType.NoPets)
        {
            AllergyFriendly = allergyFriendly;
        }


        SetHotel(hotel);
        Add(this);
    }

    public Room()
    {
    }


    public static List<Room> GetListOfAvailableRooms()
    {
        return _roomList
            .Where(r => r.IsAvailable)
            .ToList();
    }
    public static List<Room> GetListOfRoomsToClean()
    {
        return _roomList
            .Where(r => r.IsCleaned)
            .ToList();
    }

    public static IReadOnlyList<Room> GetExtent()
    {
        return _roomList.AsReadOnly();
    }

    public static void Add(Room r)
    {
        if (r == null) throw new ArgumentNullException(nameof(r));
        _roomList.Add(r);
    }

    internal static void ReplaceExtent(List<Room> rooms)
    {
        if (rooms == null) throw new ArgumentNullException(nameof(rooms));
        _roomList = rooms;
    }

    public static void ClearExtent()
    {
        _roomList.Clear();
    }
    
    internal static void FixIdCounter()
    {
        if (_roomList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _roomList.Max(g => g.Id);
            nextId = maxId + 1;
        }
    }
}