using System;
using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using NUnit.Framework;

namespace TestsHotelBounty;

public class TestsBill
{
    [SetUp]
    public void Setup()
    {
        Bill.ClearExtent();
    }

    [Test]
    public void Bill_ConstructorWithBooking_SetsBookingAndCalculatesTotalPrice()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(4); // 3 nights
        var booking = new Booking(checkIn, checkOut, "1234567890");
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true,true);
        booking.Room = room;

        var bill = new Bill(booking);

        Assert.That(bill.Booking, Is.EqualTo(booking));

        var nights = (checkOut - checkIn).Days; // 3
        var basePrice = (decimal)room.Price * nights; // 100 * 3 = 300
        var expectedTotal = basePrice + basePrice * 0.08m; // 8% tax

        Assert.That(bill.TotalPrice, Is.EqualTo(expectedTotal));
    }

    [Test]
    public void Bill_DefaultConstructor_AddsToExtent()
    {
        var bill = new Bill();

        var extent = Bill.GetExtent();
        CollectionAssert.Contains(extent, bill);
    }

    [Test]
    public void Bill_TotalPriceNegative_ThrowsException()
    {
        var bill = new Bill();

        Assert.Throws<ArgumentException>(() =>
        {
            bill.TotalPrice = -1m;
        });
    }

    [Test]
    public void Bill_SetBookingToNull_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true,true);
        booking.Room = room;

        var bill = new Bill(booking);

        Assert.Throws<ArgumentNullException>(() =>
        {
            bill.Booking = null;
        });
    }

    [Test]
    public void Bill_ChangeBookingAfterSet_ThrowsException()
    {
        var checkIn1 = DateTime.Today.AddDays(1);
        var checkOut1 = DateTime.Today.AddDays(3);
        var booking1 = new Booking(checkIn1, checkOut1, "1234567890");
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true,true);

        booking1.Room = room;
        var bill = new Bill(booking1);

        var checkIn2 = DateTime.Today.AddDays(5);
        var checkOut2 = DateTime.Today.AddDays(7);
        var booking2 = new Booking(checkIn2, checkOut2, "0987654321");
        booking2.Room = new Standard(202, hotel, Occupancy.DOUBLE, 150, true, true, true);

        Assert.Throws<InvalidOperationException>(() =>
        {
            bill.Booking = booking2;
        });
    }

    [Test]
    public void Bill_GenerateBill_WithoutBooking_ThrowsException()
    {
        var bill = new Bill();

        Assert.Throws<InvalidOperationException>(() =>
        {
            bill.GenerateBill();
        });
    }

    [Test]
    public void Bill_GenerateBill_RecalculatesTotalPrice()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3); // 2 nights
        var booking = new Booking(checkIn, checkOut, "1234567890");
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true,true);
        booking.Room = room;

        var bill = new Bill(booking);

        var newCheckOut = checkOut.AddDays(1); // 3 nights
        booking.CheckOutDate = newCheckOut;

        bill.GenerateBill();

        var nights = (newCheckOut - checkIn).Days; // 3
        var basePrice = (decimal)room.Price * nights; // 200 * 3 = 600
        var expectedTotal = basePrice + basePrice * 0.08m;

        Assert.That(bill.TotalPrice, Is.EqualTo(expectedTotal));
    }
}