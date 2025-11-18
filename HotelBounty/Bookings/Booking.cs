using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using HotelBounty.Enums;
using HotelBounty.Guests;
using HotelBounty.Rooms;

namespace HotelBounty.Bookings;

[Serializable]
public class Booking
{
    private static List<Booking> _bookingList = new List<Booking>();
    private static int nextId = 1;

    public int Id { get; set; }

    private DateTime _checkInDate;
    private DateTime _checkOutDate;

    public DateTime CheckInDate
    {
        get => _checkInDate;
        set
        {
            _checkInDate = value;
        }
    }

    public DateTime CheckOutDate
    {
        get => _checkOutDate;
        set
        {
            
            _checkOutDate = value;
        }
    }

    public BookingStatus Status { get; set; }

    public Guest Guest { get; set; }
    public Room Room { get; set; }

    public string? GuestCardNumber { get; set; }

    public Booking()
    {
        Id = nextId++;
        AddBooking(this);
    }

    public Booking(Guest guest, Room room, DateTime checkInDate, DateTime checkOutDate, string? guestCardNumber = null)
    {
        if (guest == null) throw new ArgumentNullException(nameof(guest));
        if (room == null) throw new ArgumentNullException(nameof(room));
        if (checkOutDate <= checkInDate)
            throw new ArgumentException("Check-out date must be after check-in date");

        Id = nextId++;

        Guest = guest;
        Room = room;
        _checkInDate = checkInDate;
        _checkOutDate = checkOutDate;
        GuestCardNumber = guestCardNumber;

        Status = BookingStatus.PREPARING;

        AddBooking(this);
    }

    private static void AddBooking(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        _bookingList.Add(booking);
    }

    public static ReadOnlyCollection<Booking> GetBookingList()
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

    internal static void ClearExtent()
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