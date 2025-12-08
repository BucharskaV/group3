using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using NUnit.Framework;
using System;

namespace TestsHotelBounty;

public class TestsAssociationBookingBill
{
    private Booking _booking;

    [SetUp]
    public void SetUp()
    {
        Bill.ClearExtent();
        Booking.ClearExtent();
        Guest.ClearExtent();
        Room.ClearExtent();
        Hotel.ClearExtent();

        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var room = new Standard(101, new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), Occupancy.SINGLE, 100, true, true, true);
        var guest = new Guest("Anna", DateTime.Now.AddYears(-25), address, "12345678901", "1234567890");
        
        _booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), guest, room);
    }

    [Test]
    public void CreateBill_WorksCorrectly()
    {
        var bill = new Bill(_booking);
        Assert.That(bill.Booking, Is.EqualTo(_booking));
        Assert.IsTrue(_booking.Bills.Contains(bill));
    }

    [Test]
    public void SetBooking_WorksCorrectly()
    {
        var bill = new Bill(_booking);
        
        var room2 = new Standard(102, new Hotel("H2", "Warsaw", "123456789", 5), Occupancy.SINGLE, 100, true, true, true);
        var booking2 = new Booking(DateTime.Today.AddDays(5), DateTime.Today.AddDays(7), _booking.Guest, room2);

        bill.SetBooking(booking2);

        Assert.That(bill.Booking, Is.EqualTo(booking2));
        Assert.IsTrue(booking2.Bills.Contains(bill));
    }

    [Test]
    public void CreateBill_NullBooking_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new Bill(null));
    }

    [Test]
    public void SetBooking_NullBooking_ThrowsException()
    {
        var bill = new Bill(_booking);
        Assert.Throws<ArgumentNullException>(() => bill.SetBooking(null));
    }
    
    [Test]
    public void AddBill_NullBill_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => _booking.AddBill(null));
    }
}