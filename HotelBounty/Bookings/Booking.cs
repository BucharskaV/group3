using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using HotelBounty.Billing;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using ArgumentException = System.ArgumentException;

namespace HotelBounty.Bookings;

[Serializable]
public class Booking
{
    private static List<Booking> _bookingList = new List<Booking>();
    private static int nextId = 1;
    public int Id { get; set; }
    
    private HashSet<PaymentOperation> _paymentOperations = new HashSet<PaymentOperation>();
    public IReadOnlyCollection<PaymentOperation> PaymentOperations => _paymentOperations.ToList().AsReadOnly();

    public void AddPaymentOperation(PaymentOperation operation, bool internalCall = false)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (_paymentOperations.Contains(operation)) return;

        _paymentOperations.Add(operation);
        
        if (!internalCall)
        {
            operation.SetBooking(this, true);
        }
    }
    
    public void RemovePaymentOperation(PaymentOperation operation, bool internalCall = false)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        
        if (_paymentOperations.Contains(operation))
        {
            _paymentOperations.Remove(operation);
            
            if (!internalCall)
            {
                operation.UnsetBooking(true);
            }
        }
    }
    
    private Room _room;
    public Room Room => _room;

    public void SetRoom(Room room, bool internalCall = false)
    {
        if (Status == BookingStatus.COMPLETED || Status == BookingStatus.CANCELED)
            throw new InvalidOperationException("Cannot change room for completed or canceled booking");

        if (_room == room) return;
        
        if (_room != null)
        {
            Room old = _room;
            _room = null;
            if (!internalCall) old.RemoveBooking(this, true);
        }
        
        if (room != null)
        {
            _room = room;
            if (!internalCall) room.AddBooking(this, true);
        }
    }

    private Guest _guest;
    public Guest Guest => _guest;
    public void SetGuest(Guest guest, bool internalCall = false)
    {
        if (guest == null) throw new ArgumentNullException("The guest can't be null");
        if (_guest == guest) throw new ArgumentException("The guest can't be the same");

        if (_guest != null)
        {
            var oldGuest = _guest;
            _guest = null;
            if (!internalCall) oldGuest.RemoveBooking(this, true);
        }

        _guest = guest;
        if (!internalCall)
        {
            guest.AddBooking(this, true);
        }
    }

    public void UnsetGuest(bool internalCall = false)
    {
        if (_guest != null)
        {
            var oldGuest = _guest;
            _guest = null;

            if (!internalCall)
                oldGuest.RemoveBooking(this, true);
        }
        else 
            throw new ArgumentNullException("The guest is not set.");
    }
    
    private HashSet<Bill> _bills = new HashSet<Bill>();
    
    [XmlIgnore]
    public IReadOnlyCollection<Bill> Bills => _bills.ToList().AsReadOnly();

    public void AddBill(Bill bill, bool internalCall = false)
    {
        if (bill == null) throw new ArgumentNullException(nameof(bill));
        if (_bills.Contains(bill)) return;

        _bills.Add(bill);
        
        if (!internalCall)
        {
            bill.SetBooking(this, true);
        }
    }
    
    public void RemoveBill(Bill bill, bool internalCall = false)
    {
        if (bill == null) throw new ArgumentNullException(nameof(bill));
        if (_bills.Contains(bill))
        {
            _bills.Remove(bill);
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

    public Booking()
    {
        Id = nextId++;
        AddBookingToList(this);
    }

    public Booking(DateTime checkInDate, DateTime checkOutDate, Guest guest, Room room = null)
    {
        if (checkOutDate <= checkInDate)
            throw new ArgumentException("Check-out date must be after check-in date");
        
        if (guest == null) throw new ArgumentNullException(nameof(guest), "Booking requires a guest");

        Id = nextId++;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        Status = BookingStatus.PREPARING;

        SetGuest(guest);
        
        if (room != null)
            SetRoom(room);

        AddBookingToList(this);
    }

    private static void AddBookingToList(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        _bookingList.Add(booking);
    }

    public static ReadOnlyCollection<Booking> GetExtent()
    {
        return _bookingList.AsReadOnly();
    }

    public string CheckBookingState() => Status.ToString();

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

    public static void ClearExtent() => _bookingList.Clear();

    internal static void FixIdCounter()
    {
        if (_bookingList.Count == 0) nextId = 1;
        else nextId = _bookingList.Max(b => b.Id) + 1;
    }
}