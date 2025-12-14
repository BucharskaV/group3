using System;
using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using HotelBounty.ComplexAttributes;
using NUnit.Framework;

namespace TestsHotelBounty;

public class TestsNewExceptions
{
    private Guest _guest;
    private Address _address;
    private Hotel _hotel;
    private Room _room;

    [SetUp]
    public void Setup()
    {
        PaymentOperation.ClearExtent();
        Bill.ClearExtent();
        Booking.ClearExtent();
        Guest.ClearExtent();
        Room.ClearExtent();
        Hotel.ClearExtent();

        _address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        _guest = new Guest("Test Guest", DateTime.Now.AddYears(-25), _address, "12345678901", "1234567890");
        _hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        _room = new Standard(101, _hotel, Occupancy.SINGLE, 100, false, true, true);
    }
    

    [Test]
    public void Booking_AddDuplicatePaymentOperation_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        var bill = new Bill(booking);
        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 100);
        
        Assert.Throws<InvalidOperationException>(() => booking.AddPaymentOperation(payment));
    }

    [Test]
    public void Booking_RemoveNonExistentPaymentOperation_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        var booking2 = new Booking(DateTime.Today.AddDays(5), DateTime.Today.AddDays(6), _guest, _room);
        var bill2 = new Bill(booking2);
        var paymentOther = new PaymentOperation(bill2, booking2, PaymentMethod.CASH, 100);
        
        Assert.Throws<InvalidOperationException>(() => booking.RemovePaymentOperation(paymentOther));
    }

    [Test]
    public void Booking_SetSameRoomAgain_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        
        Assert.Throws<InvalidOperationException>(() => booking.SetRoom(_room));
    }
    
    [Test]
    public void Booking_SetSameGuestAgain_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        
        Assert.Throws<ArgumentException>(() => booking.SetGuest(_guest)); 
    }
    

    [Test]
    public void Bill_AddDuplicatePaymentOperation_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        var bill = new Bill(booking);
        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 100);
        
        Assert.Throws<InvalidOperationException>(() => bill.AddPaymentOperation(payment));
    }

    [Test]
    public void Bill_RemoveNonExistentPaymentOperation_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        var bill = new Bill(booking);
        
        var booking2 = new Booking(DateTime.Today.AddDays(5), DateTime.Today.AddDays(6), _guest, _room);
        var bill2 = new Bill(booking2);
        var paymentOther = new PaymentOperation(bill2, booking2, PaymentMethod.CASH, 100);
        
        Assert.Throws<InvalidOperationException>(() => bill.RemovePaymentOperation(paymentOther));
    }
    

    [Test]
    public void PaymentOperation_SetSameBillAgain_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        var bill = new Bill(booking);
        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 100);

       
        Assert.Throws<InvalidOperationException>(() => payment.SetBill(bill));
    }

    [Test]
    public void PaymentOperation_SetSameBookingAgain_ThrowsException()
    {
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _guest, _room);
        var bill = new Bill(booking);
        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 100);

        
        Assert.Throws<InvalidOperationException>(() => payment.SetBooking(booking));
    }
}