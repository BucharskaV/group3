using System.Collections.ObjectModel;
using System.Xml.Serialization;
using HotelBounty.Rooms;

namespace HotelBounty;

[Serializable]
public class Hotel
{
    private static List<Hotel> _hotelList = new List<Hotel>();
    private static int nextId = 1;
    public int Id { get; set; }
    private Dictionary<int, Room> _rooms = new Dictionary<int, Room>();
    public IReadOnlyDictionary<int, Room> Rooms => _rooms;
    
    public List<Room> RoomsSerializable
    {
        get => _rooms.Values.ToList();
        set
        {
            _rooms = value?.ToDictionary(r => r.RoomNumber) ?? new Dictionary<int, Room>();
            foreach (var room in _rooms.Values)
            {
                room.SetHotel(this);
            }
        }
    }
    
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }
    
    private string _city;
    public string City
    {
        get => _city;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("City cannot be empty");
            _city = value;
        }
    }

    private string _phoneNumber;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("Phone number cannot be empty");
            if (value.Length != 9 || !value.All(char.IsDigit))
                throw new ArgumentException("Phone number must be exactly 9 digits");
            _phoneNumber = value;
        }
    }

    private int _stars;
    public int Stars
    {
        get => _stars;
        set
        {
            if(value < 1 || value > 5)
                throw new ArgumentException("Hotel can have from 1 to 5 stars");
            _stars = value;
        }
    }
    
    private readonly HashSet<HotelBlock> _blocks = new HashSet<HotelBlock>();
    public IReadOnlyCollection<HotelBlock> HotelBlocks => _blocks.ToList().AsReadOnly();

    public void AddHotelBlock(HotelBlock block)
    {
        if(block == null) throw new ArgumentNullException("The hotel block cannot be null");
        if (_blocks.Contains(block)) throw new ArgumentException("The hotel already contains the block");
        
        if(block.Hotel != this)
            throw new InvalidOperationException("Block belongs to another hotel");
        
        _blocks.Add(block);
    }
    
    public void RemoveHotelBlock(HotelBlock block)
    {
        if(block == null) throw new ArgumentNullException("The hotel block cannot be null");
        if (!_blocks.Contains(block)) throw new InvalidOperationException("Block does not belong to this hotel");
        
        _blocks.Remove(block);
        block.Delete();
    }

    public void DeleteWholeHotel()
    {
        foreach (var block in _blocks.ToList())
            block.Delete();
        
        _blocks.Clear();
        _hotelList.Remove(this);
    }
    
    public Hotel(string name, string city, string phoneNumber, int stars)
    {
        Id = nextId++;
        Name = name;
        City = city;
        PhoneNumber = phoneNumber;
        Stars = stars;

        AddHotel(this);
    }

    public Hotel()
    {
        Id = nextId++;
        AddHotel(this);
    }
    
    public static ReadOnlyCollection<Hotel> GetExtent()
    {
        return _hotelList.AsReadOnly();
    }

    private static void AddHotel(Hotel e)
    {
        if(e == null) throw new ArgumentException("Hotel cannot be null");
        _hotelList.Add(e);
    }

    internal static void ReplaceExtent(List<Hotel> h)
    {
        if (h == null) throw new ArgumentNullException(nameof(h));
        _hotelList = h;
    }

    public static void ClearExtent()
    {
        _hotelList.Clear();
    }
    
    internal static void FixIdCounter()
    {
        if (_hotelList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _hotelList.Max(g => g.Id);
            nextId = maxId + 1;
        }
    }
    
    
    public void AddRoom(Room room, bool internalCall = false)
    {
        if (room == null)
            throw new ArgumentNullException(nameof(room));

        if (_rooms.ContainsKey(room.RoomNumber) && _rooms[room.RoomNumber] != room)
            throw new InvalidOperationException($"Room {room.RoomNumber} already exists in this hotel.");

        _rooms[room.RoomNumber] = room;

        if (!internalCall)
            room.SetHotel(this, true); 
    }
    

    public Room GetRoom(int roomNumber)
    {
        if (!_rooms.ContainsKey(roomNumber))
            throw new Exception($"Room number {roomNumber} does not exist in hotel {Name}.");
    
        return _rooms[roomNumber];
    }
    
    public void RemoveRoom(int roomNumber, bool internalCall = false)
    {
        if (!_rooms.ContainsKey(roomNumber))
            throw new InvalidOperationException($"Room {roomNumber} does not exist in this hotel.");

        Room room = _rooms[roomNumber];
        _rooms.Remove(roomNumber);

        if (!internalCall)
            room.SetHotel(null, true);
    }

}