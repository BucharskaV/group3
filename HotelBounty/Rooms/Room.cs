using System.Xml.Serialization;
using HotelBounty.Bookings;
using HotelBounty.Employees;
using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
[XmlInclude(typeof(PetFriendly))]
[XmlInclude(typeof(NoPets))]
[XmlInclude(typeof(Deluxe))]
[XmlInclude(typeof(Standard))]
public class Room
{
    private static List<Room> _roomList = new List<Room>();
    private static int nextId = 1;
    
    private int _roomNumber;
    public int RoomNumber
    {
        get => _roomNumber;
        set
        {
            if (value <= 0) throw new ArgumentException("Room number must be positive.");
            _roomNumber = value;
        }
    }
    public int Id { get; set; }
    
    private HashSet<Booking> _bookings = new HashSet<Booking>();
    
    public IReadOnlyCollection<Booking> Bookings => _bookings.ToList().AsReadOnly();

    internal void AddBooking(Booking booking, bool internalCall = false)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (_bookings.Contains(booking))
            return;

        _bookings.Add(booking);

        if (!internalCall)
            booking.SetRoom(this, true);
    }

    internal void RemoveBooking(Booking booking, bool internalCall = false)
    {
        if (booking == null)
            throw new ArgumentNullException(nameof(booking));

        if (!_bookings.Contains(booking))
            return;

        _bookings.Remove(booking);

        if (!internalCall)
            booking.SetRoom(null, true);
    }

    internal void SetBookingRoom(Booking booking, bool internalCall = false)
    {
        if (booking == null)
            return;

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
    
    internal void SetHotel(Hotel newHotel, bool internalCall = false)
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
    
    public Room() { }

    public Room(int roomNumber, Hotel hotel, Occupancy occupancy, double price, bool climatization, bool isCleaned, bool isAvailable)
    { 
        if(hotel == null) throw new ArgumentNullException("When creating the room the Hotel cannot be null");
        
        Id = nextId++;
        RoomNumber = roomNumber;
        Occupancy = occupancy;
        Price = price;
        Climatization = climatization;
        IsCleaned = isCleaned;
        IsAvailable = isAvailable;
        
        SetHotel(hotel);
        Add(this);
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