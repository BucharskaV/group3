using System;
using System.Collections.Generic;
using System.Linq; 
using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using HotelBounty.ComplexAttributes; 
using NUnit.Framework;

namespace TestsHotelBounty;

public class TestsBooking
{
    private Guest _guest;
    private Address _address;

    [SetUp]
    public void Setup()
    {
        Booking.ClearExtent();
        _address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        _guest = new Guest("Test Guest", DateTime.Now.AddYears(-25), _address, "12345678901", "1234567890");
    }

    [Test]
    public void Booking_SetAndGetPropertiesCorrectly()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        
        Hotel hotel = new Hotel();
        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true, true);
        
        var booking = new Booking(checkIn, checkOut, _guest, room);
        
        var bill1 = new Bill(booking); 

        Assert.That(booking.CheckInDate, Is.EqualTo(checkIn));
        Assert.That(booking.CheckOutDate, Is.EqualTo(checkOut));
        
        Assert.That(booking.Guest.GuestCardNumber, Is.EqualTo("1234567890"));
        Assert.That(booking.Room, Is.EqualTo(room));
        
        
        Assert.That(booking.Status, Is.EqualTo(BookingStatus.PREPARING));
    }

    [Test]
    public void Booking_ConstructorInvalidDates_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(3);
        var checkOut = DateTime.Today.AddDays(1);

        Assert.Throws<ArgumentException>(() =>
        {
            var booking = new Booking(checkIn, checkOut, _guest);
        });
    }

    [Test]
    public void Booking_CheckInDateInPast_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, _guest); 

        Assert.Throws<ArgumentException>(() =>
        {
            booking.CheckInDate = DateTime.Today.AddDays(-1);
        });
    }

    [Test]
    public void Booking_CheckInAfterOrEqualCheckOut_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, _guest); 

        Assert.Throws<ArgumentException>(() =>
        {
            booking.CheckInDate = booking.CheckOutDate;
        });

        Assert.Throws<ArgumentException>(() =>
        {
            booking.CheckInDate = booking.CheckOutDate.AddDays(1);
        });
    }

    [Test]
    public void Booking_CheckOutBeforeOrEqualCheckIn_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(2);
        var checkOut = DateTime.Today.AddDays(4);
        var booking = new Booking(checkIn, checkOut, _guest); 

        Assert.Throws<ArgumentException>(() =>
        {
            booking.CheckOutDate = booking.CheckInDate;
        });

        Assert.Throws<ArgumentException>(() =>
        {
            booking.CheckOutDate = booking.CheckInDate.AddDays(-1);
        });
    }

    [Test]
    public void Booking_SetRoomToNull_RemovesRoom()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(2);
        var hotel = new Hotel();
        var room = new Standard(101, hotel, Occupancy.SINGLE, 100, true, true, true);
        
        var booking = new Booking(checkIn, checkOut, _guest, room); 

        booking.SetRoom(null);
        Assert.IsNull(booking.Room);
    }


    [Test]
    public void Booking_ChangeRoomForCompletedOrCanceled_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(2);
        Hotel hotel = new Hotel();
        var room1 = new Standard(102, hotel, Occupancy.SINGLE, 100, true, true, true);
        var room2 = new Standard(103, hotel, Occupancy.DOUBLE, 150, true, true, true);
        
        var booking = new Booking(checkIn, checkOut, _guest, room1); 

        booking.Status = BookingStatus.COMPLETED;

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.SetRoom(room2); 
        });
        
        var booking2 = new Booking(checkIn, checkOut, _guest); 
        booking2.SetRoom(room1); 
        booking2.CancelBooking();

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking2.SetRoom(room2); 
        });
    }
    
    [Test]
    public void Booking_NullGuest_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(2);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var booking = new Booking(checkIn, checkOut, null);
        });
    }
    
    

    [Test]
    public void Booking_MakeNewBooking_ValidDates_UpdatesDatesAndStatus()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, _guest); 

        var newCheckIn = DateTime.Today.AddDays(5);
        var newCheckOut = DateTime.Today.AddDays(7);

        booking.MakeNewBooking(newCheckIn, newCheckOut);

        Assert.That(booking.CheckInDate, Is.EqualTo(newCheckIn));
        Assert.That(booking.CheckOutDate, Is.EqualTo(newCheckOut));
        Assert.That(booking.Status, Is.EqualTo(BookingStatus.BOOKED));
    }

    [Test]
    public void Booking_MakeNewBooking_InvalidDates_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, _guest);

        var newCheckIn = DateTime.Today.AddDays(7);
        var newCheckOut = DateTime.Today.AddDays(5);

        Assert.Throws<ArgumentException>(() =>
        {
            booking.MakeNewBooking(newCheckIn, newCheckOut);
        });
    }

    [Test]
    public void Booking_CancelBooking_ChangesStatusToCanceled()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _guest); 

        booking.CancelBooking();

        Assert.That(booking.Status, Is.EqualTo(BookingStatus.CANCELED));
    }

    [Test]
    public void Booking_CancelBooking_WhenCompleted_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _guest); 
        booking.MarkAsPaid();
        booking.MarkAsCompleted();

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.CancelBooking();
        });
    }

    [Test]
    public void Booking_MarkAsPaid_WhenCanceled_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _guest); 
        booking.CancelBooking();

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.MarkAsPaid();
        });
    }

    [Test]
    public void Booking_MarkAsCompleted_NotPaid_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _guest); 

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.MarkAsCompleted();
        });
    }

    [Test]
    public void Booking_MarkAsCompleted_FromPaid_SetsStatusCompleted()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _guest); 

        booking.MarkAsPaid();
        booking.MarkAsCompleted();

        Assert.That(booking.Status, Is.EqualTo(BookingStatus.COMPLETED));
    }

    [Test]
    public void Booking_StatusFromCompletedToOther_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _guest); 

        booking.MarkAsPaid();
        booking.MarkAsCompleted();

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.Status = BookingStatus.BOOKED;
        });
    }

    [Test]
    public void Booking_DeleteCanceledBookings_RemovesOnlyCanceled()
    {
        Booking.ClearExtent();

        var b1 = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _guest); 
        var b2 = new Booking(DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), _guest); 
        var b3 = new Booking(DateTime.Today.AddDays(5), DateTime.Today.AddDays(6), _guest); 

        b2.CancelBooking();

        Assert.That(Booking.GetExtent().Count, Is.EqualTo(3));

        Booking.DeleteCanceledBookings();

        var extent = Booking.GetExtent();
        Assert.That(extent.Count, Is.EqualTo(2));
        Assert.False(extent.Contains(b2));
        Assert.True(extent.Contains(b1));
        Assert.True(extent.Contains(b3));
    }
}