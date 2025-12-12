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
    
    private HashSet<Booking> _bookings = new HashSet<Booking>();
    public IReadOnlyCollection<Booking> Bookings => _bookings.ToList().AsReadOnly();
    public void AddBooking(Booking booking, bool internalCall = false)
    {
        if (booking == null) throw new ArgumentNullException("The booking can't be null");
        if (_bookings.Contains(booking)) throw new ArgumentException("The booking is already in the list");

        _bookings.Add(booking);
        if (!internalCall)
        {
            booking.SetGuest(this, true);
        }
    }

    public void RemoveBooking(Booking booking, bool internalCall = false)
    {
        if (booking == null) throw new ArgumentNullException("The booking can't be null");
        if (!_bookings.Contains(booking)) return;

        if (_bookings.Count <= 1)
            throw new InvalidOperationException("Guest must have at least one booking");

        _bookings.Remove(booking);
        if (!internalCall)
        {
            booking.SetGuest(null, true);
        }
    }
    private string _guestCardNumber;
    public string GuestCardNumber
    {
        get => _guestCardNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Guest card number cannot be empty.");

            if (!value.All(char.IsDigit) || value.Length != 10)
                throw new ArgumentException("Card number must be 10 digits.");
            _guestCardNumber = value;
        }
    }

    private string _name;
    private DateTime _dateOfBirth;
    private string _pesel;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Guest name cannot be empty");
            if (value.Length > 50) throw new ArgumentException("Guest name too long");
            _name = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value > DateTime.Today) throw new ArgumentException("Date cannot be in future");
            if (CalculateAge(value) < 18) throw new ArgumentException("Guest must be at least 18.");
            _dateOfBirth = value;
        }
    }

    public int Age => CalculateAge(_dateOfBirth);

    private Address _address;
    public Address Address
    {
        get => _address;
        set
        {
            if(value == null) throw new ArgumentNullException("Address cannot be null");
            _address = value;
        }
    }

    public string Pesel
    {
        get => _pesel;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("PESEL cannot be empty.");
            if (!string.IsNullOrEmpty(value) && value.Length != 11) throw new ArgumentException("Pesel should contain 11 characters");
            _pesel = value;
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
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    private static void AddGuest(Guest guest)
    {
        if (guest == null) throw new ArgumentNullException(nameof(guest));
        _guestList.Add(guest);
    }

    public static ReadOnlyCollection<Guest> GetExtent() => _guestList.AsReadOnly();

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

    public static void ClearExtent() => _guestList.Clear();

    internal static void FixIdCounter()
    {
        if (_guestList.Count == 0) nextId = 1;
        else nextId = _guestList.Max(g => g.Id) + 1;
    }
}