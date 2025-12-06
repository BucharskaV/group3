using System;
using System.Collections.Generic;
using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using NUnit.Framework;

namespace TestsHotelBounty;

public class TestsBooking
{
    [SetUp]
    public void Setup()
    {
        Booking.ClearExtent();
    }

    [Test]
    public void Booking_SetAndGetPropertiesCorrectly()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);

        var booking = new Booking(checkIn, checkOut, "1234567890");

        Hotel hotel = new Hotel();
        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true,true);
        booking.Room = room;

        var bill1 = new Bill();
        var bill2 = new Bill();
        booking.Bills = new List<Bill> { bill1, bill2 };

        Assert.That(booking.CheckInDate, Is.EqualTo(checkIn));
        Assert.That(booking.CheckOutDate, Is.EqualTo(checkOut));
        Assert.That(booking.GuestCardNumber, Is.EqualTo("1234567890"));
        Assert.That(booking.Room, Is.EqualTo(room));
        CollectionAssert.AreEquivalent(new[] { bill1, bill2 }, booking.Bills);
        Assert.That(booking.Status, Is.EqualTo(BookingStatus.PREPARING));
    }

    [Test]
    public void Booking_ConstructorInvalidDates_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(3);
        var checkOut = DateTime.Today.AddDays(1);

        Assert.Throws<ArgumentException>(() =>
        {
            var booking = new Booking(checkIn, checkOut, "1234567890");
        });
    }

    [Test]
    public void Booking_CheckInDateInPast_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");

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
        var booking = new Booking(checkIn, checkOut, "1234567890");

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
        var booking = new Booking(checkIn, checkOut, "1234567890");

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
    public void Booking_RoomNull_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(2);
        var booking = new Booking(checkIn, checkOut, "1234567890");

        Assert.Throws<ArgumentNullException>(() =>
        {
            booking.Room = null;
        });
    }

    [Test]
    public void Booking_ChangeRoomForCompletedOrCanceled_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(2);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        Hotel hotel = new Hotel();
        var room1 = new Standard(102, hotel, Occupancy.SINGLE, 100, true, true, true);
        var room2 = new Standard(103, hotel, Occupancy.DOUBLE, 150, true, true, true);

        booking.Room = room1;
        booking.Status = BookingStatus.COMPLETED;

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.Room = room2;
        });

        // второй кейс: для CANCELED
        var booking2 = new Booking(checkIn, checkOut, "0987654321");
        booking2.Room = room1;
        booking2.CancelBooking();

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking2.Room = room2;
        });
    }

    [Test]
    public void Booking_EmptyGuestCardNumber_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(2);

        Assert.Throws<ArgumentException>(() =>
        {
            var booking = new Booking(checkIn, checkOut, "");
        });
    }

    [Test]
    public void Booking_InvalidGuestCardNumber_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(2);
        var booking = new Booking(checkIn, checkOut, "1234567890");

        Assert.Throws<ArgumentException>(() =>
        {
            booking.GuestCardNumber = "12345"; // too short
        });

        Assert.Throws<ArgumentException>(() =>
        {
            booking.GuestCardNumber = "abcdefghij"; // not digits
        });
    }

    [Test]
    public void Booking_SetBillsNullCollection_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");

        Assert.Throws<ArgumentNullException>(() =>
        {
            booking.Bills = null;
        });
    }

    [Test]
    public void Booking_SetBillsWithNullItem_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");

        Assert.Throws<ArgumentException>(() =>
        {
            booking.Bills = new List<Bill> { new Bill(), null };
        });
    }

    [Test]
    public void Booking_MakeNewBooking_ValidDates_UpdatesDatesAndStatus()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");

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
        var booking = new Booking(checkIn, checkOut, "1234567890");

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
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");

        booking.CancelBooking();

        Assert.That(booking.Status, Is.EqualTo(BookingStatus.CANCELED));
    }

    [Test]
    public void Booking_CancelBooking_WhenCompleted_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");
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
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");
        booking.CancelBooking();

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.MarkAsPaid();
        });
    }

    [Test]
    public void Booking_MarkAsCompleted_NotPaid_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.MarkAsCompleted();
        });
    }

    [Test]
    public void Booking_MarkAsCompleted_FromPaid_SetsStatusCompleted()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");

        booking.MarkAsPaid();
        booking.MarkAsCompleted();

        Assert.That(booking.Status, Is.EqualTo(BookingStatus.COMPLETED));
    }

    [Test]
    public void Booking_StatusFromCompletedToOther_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1234567890");

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

        var b1 = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), "1111111111");
        var b2 = new Booking(DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), "2222222222");
        var b3 = new Booking(DateTime.Today.AddDays(5), DateTime.Today.AddDays(6), "3333333333");

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