using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using HotelBounty.Billing;
using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace HotelBounty.Bookings;

[Serializable]
public class Booking
{
    private static List<Booking> _bookingList = new List<Booking>();
    private static int nextId = 1;
    public int Id { get; set; }
    
    private Room _room;
    public Room Room
    {
        get => _room;
        set
        {
            if(value == null) throw new ArgumentNullException("The room cannot be null.");
            
            if (Status == BookingStatus.COMPLETED || Status == BookingStatus.CANCELED)
                throw new InvalidOperationException("Room cannot be changed for completed or canceled booking.");
    
            _room = value;
        }
    }

    private List<Bill> _bills = new List<Bill>();
    public List<Bill> Bills
    {
        get { return _bills; }
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Bills collection cannot be null.");

            if (value.Any(b => b == null))
                throw new ArgumentException("Bills collection cannot contain null items.", nameof(value));

            _bills = value;
        }
    }

    private DateTime _checkInDate;
    private DateTime _checkOutDate;

    public DateTime CheckInDate
    {
        get => _checkInDate;
        set
        {
            if (value.Date < DateTime.Today)
                throw new ArgumentException("Check-in date cannot be in the past.");

            if (_checkOutDate != default && value >= _checkOutDate)
                throw new ArgumentException("Check-in date must be before check-out date.");

            _checkInDate = value;
        }
    }

    public DateTime CheckOutDate
    {
        get => _checkOutDate;
        set
        {
            if (value <= _checkInDate)
                throw new ArgumentException("Check-out date must be after check-in date.");
            _checkOutDate = value;
        }
    }

    private BookingStatus _status;

    public BookingStatus Status
    {
        get => _status;
        set
        {
            if (_status == BookingStatus.COMPLETED && value != BookingStatus.COMPLETED)
                throw new InvalidOperationException("Cannot change status of a completed booking.");

            _status = value;
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

    public Booking()
    {
        Id = nextId++;
        AddBooking(this);
    }

    public Booking(DateTime checkInDate, DateTime checkOutDate, string guestCardNumber)
    {
        if (checkOutDate <= checkInDate)
            throw new ArgumentException("Check-out date must be after check-in date");

        Id = nextId++;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        GuestCardNumber = guestCardNumber;

        Status = BookingStatus.PREPARING;

        AddBooking(this);
    }

    private static void AddBooking(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        _bookingList.Add(booking);
    }

    public static ReadOnlyCollection<Booking> GetExtent()
    {
        return _bookingList.AsReadOnly();
    }

    public string CheckBookingState()
    {
        return Status.ToString();
    }

    public void MakeNewBooking(DateTime newCheckIn, DateTime newCheckOut)
    {
        if (newCheckOut <= newCheckIn)
            throw new ArgumentException("Check-out date must be after check-in date");

        _checkInDate = newCheckIn;
        _checkOutDate = newCheckOut;
        Status = BookingStatus.BOOKED;
    }

    public void CancelBooking()
    {
        if (Status == BookingStatus.COMPLETED)
            throw new InvalidOperationException("Cannot cancel a completed booking");
        Status = BookingStatus.CANCELED;
    }

    public void MarkAsPaid()
    {
        if (Status == BookingStatus.CANCELED)
            throw new InvalidOperationException("Cannot pay for a canceled booking");
        Status = BookingStatus.PAID;
    }

    public void MarkAsCompleted()
    {
        if (Status != BookingStatus.PAID)
            throw new InvalidOperationException("Booking must be paid before completion");
        Status = BookingStatus.COMPLETED;
    }

    public static void DeleteCanceledBookings()
    {
        _bookingList.RemoveAll(b => b.Status == BookingStatus.CANCELED);
    }

    internal static void ReplaceExtent(List<Booking> bookings)
    {
        if (bookings == null) throw new ArgumentNullException(nameof(bookings));
        _bookingList = bookings;
    }

    public static void ClearExtent()
    {
        _bookingList.Clear();
    }

    internal static void FixIdCounter()
    {
        if (_bookingList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _bookingList.Max(b => b.Id);
            nextId = maxId + 1;
        }
    }
}