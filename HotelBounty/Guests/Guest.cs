using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;

[Serializable]
public class Guest
{
    private static List<Guest> _guestList = new List<Guest>();
    private static int nextId = 1;
    public int Id { get; set; }
    
    private List<Booking> _bookings = new List<Booking>();
    public List<Booking> Bookings
    {
        get { return _bookings; }
        set { _bookings = value; }
    }

    private string _name;
    private DateTime _dateOfBirth;
    private string _pesel;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Guest name cannot be empty");
            if (value.Length > 50)
                throw new ArgumentException("Guest name cannot be longer than 50 characters");
            _name = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value > DateTime.Today)
                throw new ArgumentException("Date of birth cannot be in the future");
            _dateOfBirth = value;
        }
    }

    public int Age
    {
        get
        {
            return CalculateAge(_dateOfBirth);
        }
    }

    private Address _address;
    public Address Address
    {
        get => _address;
        set
        {
            if(value == null) throw new ArgumentNullException("The adress cannot be null.");
            _address = value;
        }
    }

    public string Pesel
    {
        get => _pesel;
        set
        {
            if (!string.IsNullOrEmpty(value) && value.Length != 11)
                throw new ArgumentException("Pesel should contain 11 characters");
            _pesel = value;
        }
    }
    
    private string _guestCardNumber;
    public string GuestCardNumber
    {
        get => _guestCardNumber;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("Guest card number cannot be empty");
            if (value.Length != 10 && !value.All(char.IsDigit))
                throw new ArgumentException("Guest card number  must be exactly 10 digits");
            _guestCardNumber = value;
        }
    }

    public Guest() 
    {
        Id = nextId++;
        AddGuest(this);
    }

    public Guest(string name, DateTime dateOfBirth, Address address, string pesel, string guestCardNumber)
    {
        Id = nextId++;
        Name = name;
        DateOfBirth = dateOfBirth;
        Address = address;
        Pesel = pesel;
        GuestCardNumber = guestCardNumber;

        AddGuest(this);
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;
        return age;
    }

    private static void AddGuest(Guest guest)
    {
        if (guest == null) throw new ArgumentNullException(nameof(guest));
        _guestList.Add(guest);
    }


    public static ReadOnlyCollection<Guest> GetExtent()
    {
        return _guestList.AsReadOnly();
    }

    public void EditGuestInfo(string name, DateTime dateOfBirth, Address address, string? pesel)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        Address = address;
        Pesel = pesel;
    }

    internal static void ReplaceExtent(List<Guest> guests)
    {
        if (guests == null) throw new ArgumentNullException(nameof(guests));
        _guestList = guests;
    }

    public static void ClearExtent()
    {
        _guestList.Clear();
    }

    internal static void FixIdCounter()
    {
        if (_guestList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _guestList.Max(g => g.Id);
            nextId = maxId + 1;
        }
    }
}