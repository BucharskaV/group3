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
    public int Id { get; set; }
    
    private List<Booking> _bookings = new List<Booking>();

    public List<Booking> Bookings
    {
        get { return _bookings; }
        set { _bookings = value; }
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

    private string? _climatization;
    
    public string? Climatization
    {
        get => _climatization;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                    throw new ArgumentException("Description of climatization availability cannot be longer than 50 characters");
            }
            _climatization = value; 
        }
    }

    private string? _isCleaned;
    public string? IsCleaned
    {
        get => _isCleaned;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if(value.Length > 50)
                    throw new ArgumentException("Description of room state cannot be longer than 50 characters");
            }
            _isCleaned = value;
        }
    }

    private string? _isAvailable;
    public string? IsAvailable
    {
        get => _isAvailable;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if(value.Length > 50)
                    throw new ArgumentException("Description of room state cannot be longer than 50 characters");
                _isAvailable = value;
            }
        }
    }
    
    private Hotel _hotel;
    public Hotel Hotel
    {
        get => _hotel;
        set
        {
            if(value == null) throw new ArgumentNullException("The Hotel cannot be null");
            _hotel = value;
        }
    }

    public Room()
    {
        Id = nextId++;
        AddRoom(this);
    }

    public Room(Occupancy occupancy, double price, string climatization, string isCleaned, string isAvailable)
    { 
        Id = nextId++;
        Occupancy = occupancy;
        Price = price;
        Climatization = climatization;
        IsCleaned = isCleaned;
        IsAvailable = isAvailable;
        
        AddRoom(this);
    }
    
    public static List<Room> GetListOfAvailableRooms()
    {
        return _roomList
            .Where(r => r.IsAvailable != null && r.IsAvailable.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static List<Room> GetListOfRoomsToClean()
    {
        return _roomList
            .Where(r => r.IsCleaned != null && r.IsCleaned.Equals("No", StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public static IReadOnlyList<Room> GetExtent()
    {
        return _roomList.AsReadOnly();
    }

    public static void AddRoom(Room r)
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