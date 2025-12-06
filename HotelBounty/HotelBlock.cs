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
    
    private readonly HashSet<Employee> _employees = new HashSet<Employee>();
    public IReadOnlyCollection<Employee> Employees => _employees;

    public void AddEmployee(Employee e)
    {
        if(e == null) throw new ArgumentNullException("The employee cannot be null.");
        if(_employees.Contains(e)) throw new InvalidOperationException("The employee already works in this block.");
        
        if(e.HotelBlock != null && e.HotelBlock != this)
            throw new InvalidOperationException($"The employee already works in another block {e.HotelBlock.Name}");
        
        _employees.Add(e);
        e.AssignHotelBlock(this);
    }

    public void RemoveEmployee(Employee e)
    {
        if(e == null) throw new ArgumentNullException("The employee cannot be null.");
        if(!_employees.Contains(e)) throw new InvalidOperationException("The employee does not works in this block.");
        
        if(_employees.Count == 1)
            throw new InvalidOperationException("The employee cannot be removed from the block, the hotel block must have at least one employee.");
        
        _employees.Remove(e);
        e.UnassignHotelBlock();
    }
    
    private Hotel _hotel;
    public Hotel Hotel => _hotel;

    internal void AssignHotel(Hotel hotel)
    {
        if(hotel == null) throw new ArgumentNullException("The hotel cannot be null.");
        
        if(_hotel == hotel) throw new ArgumentException("The hotel is already assigned to this block.");
        
        if(_hotel != null && _hotel != hotel) throw new InvalidOperationException("Block cannot change its hotel in composition.");
        
        _hotel = hotel;
        hotel.AddHotelBlock(this);
    }

    public void Delete()
    {
        foreach (var e in _employees.ToList())
        {
            e.UnassignHotelBlock();
        }

        _hotelBlocksList.Remove(this);
        _hotel?.HotelBlocks?.ToList().Remove(this);
        _hotel = null!;
    }

    public HotelBlock(Hotel hotel, string name, Address location, IEnumerable<Employee>? employees = null)
    {
        Id = nextId++;
        Name = name;
        Location = location;
        
        AssignHotel(hotel);
        
        _employees = new HashSet<Employee>();
        if (employees != null)
        {
            foreach (var e in employees)
            {
                AddEmployee(e);
            }
        }
        
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