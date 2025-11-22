using System.Collections.ObjectModel;
using HotelBounty.Rooms;

namespace HotelBounty;

[Serializable]
public class Hotel
{
    private static List<Hotel> _hotelList = new List<Hotel>();
    private static int nextId = 1;
    public int Id { get; set; }
    private List<HotelBlock> _blocks = new List<HotelBlock>();
    public List<HotelBlock> HotelBlocks
    {
        get { return _blocks; }
        set { _blocks = value; }
    }
    
    private List<Room> _rooms = new List<Room>();
    public List<Room> Rooms
    {
        get { return _rooms; }
        set { _rooms = value; }
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
            if (value.Length != 9 && !value.All(char.IsDigit))
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

    public Hotel(string name, string city, string phoneNumber, int stars)
    {
        Id = nextId++;
        _name = name;
        _city = city;
        _phoneNumber = phoneNumber;
        _stars = stars;

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
}