using System.Collections.ObjectModel;

namespace HotelBounty.ComplexAttributes;

[Serializable]
public class Address
{
    private static List<Address> _addressList = new List<Address>();
    private static int nextId = 1;
    public int Id { get; set; }
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
    private string _district;
    public string District
    {
        get => _district;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("District cannot be empty");
            _district = value;
        }
    }
    private string _street;
    public string Street
    {
        get => _city;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("Street cannot be empty");
            _street = value;
        }
    }
    private int _houseNumber;
    public int HouseNumber
    {
        get => _houseNumber;
        set
        {
            if(value < 0) throw new ArgumentOutOfRangeException("The number of the house cannot be less than zero.");
            _houseNumber = value;
        }
    }

    public Address(string city, string district, string street, int houseNumber)
    {
        Id = nextId++;
        _city = city;
        _district = district;
        _street = street;
        _houseNumber = houseNumber;

        AddAddress(this);
    }
    public Address(){}
    
    public static ReadOnlyCollection<Address> GetExtent()
    {
        return _addressList.AsReadOnly();
    }

    private static void AddAddress(Address e)
    {
        if(e == null) throw new ArgumentException("Address cannot be null");
        _addressList.Add(e);
    }

    internal static void ReplaceExtent(List<Address> h)
    {
        if (h == null) throw new ArgumentNullException(nameof(h));
        _addressList = h;
    }

    public static void ClearExtent()
    {
        _addressList.Clear();
    }
    
    internal static void FixIdCounter()
    {
        if (_addressList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _addressList.Max(g => g.Id);
            nextId = maxId + 1;
        }
    }
}