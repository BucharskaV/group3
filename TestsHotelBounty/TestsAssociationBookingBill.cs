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
        Assert.That(bill.Id, Is.GreaterThan(0));
        Assert.That(bill.TotalPrice, Is.GreaterThan(0));
    }

    [Test]
    public void CreateBill_NullBooking_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new Bill(null));
    }

   
}