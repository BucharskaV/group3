using System;
using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using HotelBounty.ComplexAttributes; 
using NUnit.Framework;

namespace TestsHotelBounty;

public class TestsBill
{
    private Guest _guest;
    private Address _address;

    [SetUp]
    public void Setup()
    {
        Bill.ClearExtent();
        Booking.ClearExtent();
        Guest.ClearExtent();
        Room.ClearExtent();
        Hotel.ClearExtent();
        Address.ClearExtent();
        
        _address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        _guest = new Guest("Test Guest", DateTime.Now.AddYears(-25), _address, "12345678901", "1234567890");
    }

    [Test]
    public void Bill_ConstructorWithBooking_SetsBookingAndCalculatesTotalPrice()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(4); // 3 nights
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true, true);
        
        var booking = new Booking(checkIn, checkOut, _guest, room);

        var bill = new Bill(booking);

        // Assert.That(bill.Booking, Is.EqualTo(booking)); 

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
    public void Bill_GenerateBill_RecalculatesTotalPrice()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3); // 2 nights
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        var room = new Standard(201, hotel, Occupancy.SINGLE, 100, false, true, true);
        var booking = new Booking(checkIn, checkOut, _guest, room);
        
        var newCheckOut = checkOut.AddDays(1); // 3 nights
        booking.CheckOutDate = newCheckOut;
        
        var bill = new Bill(booking); 

        var nights = (newCheckOut - checkIn).Days; // 3
        var basePrice = (decimal)room.Price * nights; // 300
        var expectedTotal = basePrice + basePrice * 0.08m;

        Assert.That(bill.TotalPrice, Is.EqualTo(expectedTotal));
    }
}