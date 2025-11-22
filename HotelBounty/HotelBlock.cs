using System.Collections.ObjectModel;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;

namespace HotelBounty;

[Serializable]
public class HotelBlock
{
    private static List<HotelBlock> _hotelBlocksList = new List<HotelBlock>();
    private static int nextId = 1;
    public int Id { get; set; }
    
    private List<Employee> _employees = new List<Employee>();

    public List<Employee> Employees
    {
        get { return _employees; }
        set { _employees = value; }
    }
    
    private Hotel _hotel;
    public Hotel Hotel
    {
        get => _hotel;
        set
        {
            if(value == null) throw new ArgumentNullException("The hotel cannot be null.");
            _hotel = value;
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
    
    private Address _location;

    public Address Location
    {
        get => _location;
        set
        {
            if(value == null) throw new ArgumentNullException("The location cannot be null.");
            _location = value;
        }
    }

    public HotelBlock(string name, Address location)
    {
        Id = nextId++;
        _name = name;
        _location = location;
        
        AddHotelBlock(this);
    }

    public HotelBlock()
    {
        Id = nextId++;
        
        AddHotelBlock(this);
    }
    
    public static ReadOnlyCollection<HotelBlock> GetExtent()
    {
        return _hotelBlocksList.AsReadOnly();
    }

    private static void AddHotelBlock(HotelBlock e)
    {
        if(e == null) throw new ArgumentException("Hotel block cannot be null");
        _hotelBlocksList.Add(e);
    }

    internal static void ReplaceExtent(List<HotelBlock> h)
    {
        if (h == null) throw new ArgumentNullException(nameof(h));
        _hotelBlocksList = h;
    }

    public static void ClearExtent()
    {
        _hotelBlocksList.Clear();
    }
    
    internal static void FixIdCounter()
    {
        if (_hotelBlocksList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _hotelBlocksList.Max(g => g.Id);
            nextId = maxId + 1;
        }
    }
}