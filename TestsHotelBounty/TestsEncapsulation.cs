using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;
using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace TestsHotelBounty;

public class TestsEncapsulation
{
    [Test]
    public void Employee_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Employee.ClearExtent();
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock("First hotel block", address)
        {
            Hotel = hotel
        };
        var e1 = new Cleaner("Jakub", "Ivanov", 100, block, null, Specialization.ROOMS);
        e1.Name = "A";
        var extentEmployee = Employee.GetExtent()[0];
        Assert.That(extentEmployee.Name, Is.EqualTo("A"));
        var extent = Employee.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<Employee>)extent).Add(new Cleaner())
        );
    }
    
    [Test]
    public void Hotel_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Hotel.ClearExtent();
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        hotel.Name = "Marriott";
        var extentHotel = Hotel.GetExtent()[0];
        Assert.That(extentHotel.Name, Is.EqualTo("Marriott"));
        var extent = Hotel.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<Hotel>)extent).Add(new Hotel())
        );
    }
    
    [Test]
    public void HotelBlock_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        HotelBlock.ClearExtent();
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock("First hotel block", address)
        {
            Hotel = hotel
        };
        block.Name = "A";
        var extentBlock = HotelBlock.GetExtent()[0];
        Assert.That(extentBlock.Name, Is.EqualTo("A"));
        var extent = HotelBlock.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<HotelBlock>)extent).Add(new HotelBlock())
        );
    }
    [Test]
    public void Guest_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Guest.ClearExtent();
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dob = DateTime.Today.AddYears(-25);

        var guest = new Guest("Anna", dob, address, "12345678901", "1234567890");

        guest.Name = "Maria";

        var extentGuest = Guest.GetExtent()[0];
        Assert.That(extentGuest.Name, Is.EqualTo("Maria"));

        var extent = Guest.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<Guest>)extent).Add(new Guest())
        );
    }
    

    [Test]
    public void Booking_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Booking.ClearExtent();
        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);

        var booking = new Booking(checkIn, checkOut, "1234567890");
        booking.GuestCardNumber = "0987654321";

        var extentBooking = Booking.GetExtent()[0];
        Assert.That(extentBooking.GuestCardNumber, Is.EqualTo("0987654321"));

        var extent = Booking.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<Booking>)extent).Add(new Booking())
        );
    }
    

    [Test]
    public void Bill_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Bill.ClearExtent();
        Booking.ClearExtent();

        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        booking.Room = new Standard(202, hotel, Occupancy.SINGLE, 100, true, true, true);

        var bill = new Bill(booking);

        bill.TotalPrice = 999m;

        var extentBill = Bill.GetExtent()[0];
        Assert.That(extentBill.TotalPrice, Is.EqualTo(999m));

        var extent = Bill.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<Bill>)extent).Add(new Bill())
        );
    }
    

    [Test]
    public void PaymentOperation_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        PaymentOperation.ClearExtent();
        Bill.ClearExtent();
        Booking.ClearExtent();

        var checkIn = DateTime.Today.AddDays(1);
        var checkOut = DateTime.Today.AddDays(3);
        var booking = new Booking(checkIn, checkOut, "1234567890");
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        booking.Room = new Standard(202, hotel, Occupancy.SINGLE, 100, true, true, true);
        var bill = new Bill(booking);

        var payment = new PaymentOperation(bill, booking, PaymentMethod.CASH, 50m);

        payment.Amount = 120m;

        var extentPayment = PaymentOperation.GetExtent()[0];
        Assert.That(extentPayment.Amount, Is.EqualTo(120m));

        var extent = PaymentOperation.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<PaymentOperation>)extent).Add(new PaymentOperation())
        );
    }
    
    [Test]
    public void Room_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Room.ClearExtent();
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var r1 = new Standard(201, hotel, Occupancy.SINGLE, 100, true, true, true);
        
        r1.Price = 150;

        var extentRoom = Room.GetExtent()[0];
        Assert.That(extentRoom.Price, Is.EqualTo(150));
        
        var extent = Room.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
        {
            ((ICollection<Room>)extent).Add(new Standard(202, hotel, Occupancy.DOUBLE, 200, true, true, true));
        });
    }

}