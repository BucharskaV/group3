using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;
using HotelBounty.Enums;
using HotelBounty.Persistence;
using HotelBounty.Rooms;

namespace TestsHotelBounty;

public class TestsExtent
{
    [SetUp]
    public void Setup()
    {
        Address.ClearExtent();
        Hotel.ClearExtent();
        HotelBlock.ClearExtent();
        Room.ClearExtent();
        Employee.ClearExtent();
        Guest.ClearExtent();
        Booking.ClearExtent();
        Bill.ClearExtent();
        PaymentOperation.ClearExtent();
    }
    
    [Test]
    public void Extent_StoredInstancesCorrectly()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var address1 = new Address("Gdansk", "Oliwa", "Plocka", 1);
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock("First hotel block", address)
        {
            Hotel = hotel
        };
        var r1 = new Deluxe(201, hotel, Occupancy.TRIPLE, 300.50, true, true, true, true, true);
        var r2 = new NoPets(202, hotel, Occupancy.DOUBLE, 130, true, true, true, true);
        var r3 = new PetFriendly(203, hotel, Occupancy.DOUBLE, 130, true, true, true, "Meat", 2);
        var r4 = new Standard(204, hotel, Occupancy.DOUBLE, 100.99, true, true, true);
        var e1 = new Cleaner("Jakub", "Ivanov", 100, null, Specialization.ROOMS){
            HotelBlock = block
        };
        var e2 = new Receptionist("Bob", "Ivanov", 100, null, "MyKe12334552"){
            HotelBlock = block
        };
        var e3 = new SecurityGuard("Masha", "Ivanova", 100, e2, "MyKe12334552", null){
            HotelBlock = block
        };
        var g = new Guest("Anna", new DateTime(1990, 04, 01), address1, "99072423358", "0000000001");
        var booking = new Booking(new DateTime(2025, 12, 22), new DateTime(2025, 12, 25), "0000000001")
        {
            Room = r1
        };
        var bill = new Bill(booking);
        var paymentOperation = new PaymentOperation(bill, booking, PaymentMethod.CARD, 1000);
        
        Assert.That(Address.GetExtent().Count, Is.EqualTo(2));
        Assert.That(Hotel.GetExtent().Count, Is.EqualTo(1));
        Assert.That(HotelBlock.GetExtent().Count, Is.EqualTo(1));
        Assert.That(Room.GetExtent().Count, Is.EqualTo(4));
        Assert.That(Employee.GetExtent().Count, Is.EqualTo(3));
        Assert.That(Guest.GetExtent().Count, Is.EqualTo(1));
        Assert.That(Booking.GetExtent().Count, Is.EqualTo(1));
        Assert.That(Bill.GetExtent().Count, Is.EqualTo(1));
        Assert.That(PaymentOperation.GetExtent().Count, Is.EqualTo(1));
    }

    [Test]
    public void Persistence_SaveAndRetrieveExtentsCorrectly()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var address1 = new Address("Gdansk", "Oliwa", "Plocka", 1);
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock("First hotel block", address)
        {
            Hotel = hotel
        };
        var r1 = new Deluxe(201, hotel, Occupancy.TRIPLE, 300.50, true, true, true, true, true);
        var r2 = new NoPets(202, hotel, Occupancy.DOUBLE, 130, true, true, true, true);
        var r3 = new PetFriendly(203, hotel, Occupancy.DOUBLE, 130, true, true, true, "Meat", 2);
        var r4 = new Standard(204, hotel, Occupancy.DOUBLE, 100.99, true, true, true);
        var e1 = new Cleaner("Jakub", "Ivanov", 100, null, Specialization.ROOMS){
            HotelBlock = block
        };
        var e2 = new Receptionist("Bob", "Ivanov", 100, null, "MyKe12334552"){
            HotelBlock = block
        };
        var e3 = new SecurityGuard("Masha", "Ivanova", 100, e2, "MyKe12334552", null){
            HotelBlock = block
        };
        var g = new Guest("Anna", new DateTime(1990, 04, 01), address1, "99072423358", "0000000001");
        var booking = new Booking(new DateTime(2025, 12, 22), new DateTime(2025, 12, 25), "0000000001")
        {
            Room = r1
        };
        var bill = new Bill(booking);
        var paymentOperation = new PaymentOperation(bill, booking, PaymentMethod.CARD, 1000);

        var countAddress = Address.GetExtent().Count;
        var countHotel = Hotel.GetExtent().Count;
        var countBlock = HotelBlock.GetExtent().Count;
        var countRoom = Room.GetExtent().Count;
        var countEmployee = Employee.GetExtent().Count;
        var countGuest = Guest.GetExtent().Count;
        var countBooking = Booking.GetExtent().Count;
        var countBill = Bill.GetExtent().Count;
        var countPayment = PaymentOperation.GetExtent().Count;

        ExtentPersistence.Save("hotels.xml");

        Address.ClearExtent();
        Hotel.ClearExtent();
        HotelBlock.ClearExtent();
        Room.ClearExtent();
        Employee.ClearExtent();
        Guest.ClearExtent();
        Booking.ClearExtent();
        Bill.ClearExtent();
        PaymentOperation.ClearExtent();

        Assert.That(Address.GetExtent().Count, Is.EqualTo(0));

        var loaded = ExtentPersistence.Load("hotels.xml");
        Assert.That(loaded, Is.True);

        Assert.That(Address.GetExtent().Count, Is.EqualTo(countAddress));
        Assert.That(Hotel.GetExtent().Count, Is.EqualTo(countHotel));
        Assert.That(HotelBlock.GetExtent().Count, Is.EqualTo(countBlock));
        Assert.That(Room.GetExtent().Count, Is.EqualTo(countRoom));
        Assert.That(Employee.GetExtent().Count, Is.EqualTo(countEmployee));
        Assert.That(Guest.GetExtent().Count, Is.EqualTo(countGuest));
        Assert.That(Booking.GetExtent().Count, Is.EqualTo(countBooking));
        Assert.That(Bill.GetExtent().Count, Is.EqualTo(countBill));
        Assert.That(PaymentOperation.GetExtent().Count, Is.EqualTo(countPayment));
    }

}