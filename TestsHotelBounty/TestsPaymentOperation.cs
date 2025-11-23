using System;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using NUnit.Framework;

namespace TestsHotelBounty;

public class TestsPaymentOperation
{
    [SetUp]
    public void Setup()
    {
        PaymentOperation.ClearExtent();
        Bill.ClearExtent();
        Booking.ClearExtent();
    }

    [Test]
    public void PaymentOperation_ConstructorWithParameters_SetsPropertiesCorrectly()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        var room = new Standard(Occupancy.SINGLE, 100, "AC", "Yes", "Yes");
        booking.Room = room;

        var bill = new Bill(booking);

        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 50m);

        Assert.That(payment.Bill, Is.EqualTo(bill));
        Assert.That(payment.Booking, Is.EqualTo(booking));
        Assert.That(payment.PaymentMethod, Is.EqualTo(PaymentMethod.CASH));
        Assert.That(payment.Amount, Is.EqualTo(50m));
        Assert.That(payment.PaymentDate, Is.Not.EqualTo(default(DateTime)));
    }

    [Test]
    public void PaymentOperation_DefaultConstructor_AddsToExtent()
    {
        var payment = new PaymentOperation();

        var extent = PaymentOperation.GetExtent();
        CollectionAssert.Contains(extent, payment);
    }

    [Test]
    public void PaymentOperation_InvalidPaymentMethod_ThrowsException()
    {
        var payment = new PaymentOperation();

        Assert.Throws<ArgumentException>(() =>
        {
            payment.PaymentMethod = (PaymentMethod)999;
        });
    }

    [Test]
    public void PaymentOperation_ChangePaymentMethodAfterAmountSet_ThrowsException()
    {
        var payment = new PaymentOperation();
        payment.PaymentMethod = PaymentMethod.CASH;
        payment.Amount = 100m;

        Assert.Throws<InvalidOperationException>(() =>
        {
            payment.PaymentMethod = PaymentMethod.CARD;
        });
    }

    [Test]
    public void PaymentOperation_NegativeAmount_ThrowsException()
    {
        var payment = new PaymentOperation();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            payment.Amount = -10m;
        });
    }

    [Test]
    public void PaymentOperation_SetBillToNull_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        booking.Room = new Standard(Occupancy.SINGLE, 100, "AC", "Yes", "Yes");
        var bill = new Bill(booking);

        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 50m);

        Assert.Throws<ArgumentNullException>(() =>
        {
            payment.Bill = null;
        });
    }

    [Test]
    public void PaymentOperation_ChangeBillAfterSet_ThrowsException()
    {
        var checkIn1 = DateTime.Today.AddDays(1);
        var checkOut1 = DateTime.Today.AddDays(3);
        var booking1 = new Booking(checkIn1, checkOut1, "1234567890");
        booking1.Room = new Standard(Occupancy.SINGLE, 100, "AC", "Yes", "Yes");
        var bill1 = new Bill(booking1);

        var payment = new PaymentOperation(bill1, booking1, PaymentMethod.CARD, 100m);

        var checkIn2 = DateTime.Today.AddDays(5);
        var checkOut2 = DateTime.Today.AddDays(7);
        var booking2 = new Booking(checkIn2, checkOut2, "0987654321");
        booking2.Room = new Standard(Occupancy.DOUBLE, 150, "AC", "Yes", "Yes");
        var bill2 = new Bill(booking2);

        Assert.Throws<InvalidOperationException>(() =>
        {
            payment.Bill = bill2;
        });
    }

    [Test]
    public void PaymentOperation_BillBookingMismatch_ThrowsException()
    {
        var checkIn1 = DateTime.Today.AddDays(1);
        var checkOut1 = DateTime.Today.AddDays(3);
        var booking1 = new Booking(checkIn1, checkOut1, "1234567890");
        booking1.Room = new Standard(Occupancy.SINGLE, 100, "AC", "Yes", "Yes");

        var checkIn2 = DateTime.Today.AddDays(5);
        var checkOut2 = DateTime.Today.AddDays(7);
        var booking2 = new Booking(checkIn2, checkOut2, "0987654321");
        booking2.Room = new Standard(Occupancy.DOUBLE, 150, "AC", "Yes", "Yes");

        var billForBooking2 = new Bill(booking2);

        Assert.Throws<ArgumentException>(() =>
        {
            var p = new PaymentOperation(billForBooking2, booking1, PaymentMethod.CARD, 50m);
        });
    }

    [Test]
    public void PaymentOperation_SetBookingToNull_ThrowsException()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        booking.Room = new Standard(Occupancy.SINGLE, 100, "AC", "Yes", "Yes");
        var bill = new Bill(booking);

        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 50m);

        Assert.Throws<ArgumentNullException>(() =>
        {
            payment.Booking = null;
        });
    }

    [Test]
    public void PaymentOperation_ChangeBookingAfterSet_ThrowsException()
    {
        var checkIn1 = DateTime.Today.AddDays(1);
        var checkOut1 = DateTime.Today.AddDays(3);
        var booking1 = new Booking(checkIn1, checkOut1, "1234567890");
        booking1.Room = new Standard(Occupancy.SINGLE, 100, "AC", "Yes", "Yes");

        var checkIn2 = DateTime.Today.AddDays(5);
        var checkOut2 = DateTime.Today.AddDays(7);
        var booking2 = new Booking(checkIn2, checkOut2, "0987654321");
        booking2.Room = new Standard(Occupancy.DOUBLE, 150, "AC", "Yes", "Yes");

        var bill1 = new Bill(booking1);

        var payment = new PaymentOperation(bill1, booking1, PaymentMethod.CARD, 100m);

        Assert.Throws<InvalidOperationException>(() =>
        {
            payment.Booking = booking2;
        });
    }

    [Test]
    public void PaymentOperation_Prepay_ValidAmount_SetsAmountAndDate()
    {
        var payment = new PaymentOperation();

        payment.Prepay(100m);

        Assert.That(payment.Amount, Is.EqualTo(100m));
        Assert.That(payment.PaymentDate, Is.Not.EqualTo(default(DateTime)));
    }

    [Test]
    public void PaymentOperation_Prepay_NonPositiveAmount_ThrowsException()
    {
        var payment = new PaymentOperation();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            payment.Prepay(0m);
        });

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            payment.Prepay(-10m);
        });
    }

    [Test]
    public void PaymentOperation_Pay_WithoutBill_ThrowsException()
    {
        var payment = new PaymentOperation();

        Assert.Throws<InvalidOperationException>(() =>
        {
            payment.Pay();
        });
    }

    [Test]
    public void PaymentOperation_Pay_WithBill_SetsAmountAndDate()
    {
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        var room = new Standard(Occupancy.SINGLE, 200, "AC", "Yes", "Yes");
        booking.Room = room;

        var bill = new Bill(booking);

        var payment = new PaymentOperation();
        payment.Booking = booking;
        payment.Bill = bill;
        payment.PaymentMethod = PaymentMethod.CARD;

        payment.Pay();

        Assert.That(payment.Amount, Is.EqualTo(bill.TotalPrice));
        Assert.That(payment.PaymentDate, Is.Not.EqualTo(default(DateTime)));
    }
}