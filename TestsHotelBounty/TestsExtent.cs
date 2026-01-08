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
        var block1 = new HotelBlock(hotel, "Block A", address);
        var block2 = new HotelBlock(hotel, "Block B", address);
        
        var r1 = new Room(101, RoomType.Deluxe,hotel, Occupancy.TRIPLE, 300.50, true, true, true, true, true);
        var r2 = new Room(401, RoomType.NoPets, hotel, Occupancy.DOUBLE, 120, false, false, true,
            allergyFriendly: true);
        var r3 = new Room(301, RoomType.PetFriendly, hotel, Occupancy.TRIPLE, 200, true, true, true,
            petFeeders: "Meat", maxPetsAllowed: 2);
        var r4 = new Room(104, RoomType.Standard,hotel, Occupancy.DOUBLE, 100.99, false, false, false);
        
        var e1 = new Employee("Jakub", "Ivanov", 100, block1, EmployeeRole.Cleaner, null);
        var e2 = new Employee("Bob", "Ivanov", 100, block2, EmployeeRole.Cleaner | EmployeeRole.SecurityGuard, null);
        e2.Specialization = Specialization.ROOMS;
        e2.SecurityCode = "SG12345";

        e2.AddRole(EmployeeRole.Receptionist);
        e2.DatabaseKey = "DBKEY123";
        e2.AddLanguage("English");

        e2.RemoveRole(EmployeeRole.Cleaner); 
        var e3 = new Employee("Masha", "Ivanova", 100, block2, EmployeeRole.SecurityGuard, e2);

        var g = new Guest("Anna", new DateTime(1990, 04, 01), address1, "99072423358", "0000000001");
        var booking = new Booking(new DateTime(2025, 12, 22), new DateTime(2025, 12, 25), g, r1);
        var bill = new Bill(booking);
        var paymentOperation = new PaymentOperation(bill, booking, PaymentMethod.CARD, 1000);
        
        Assert.That(Address.GetExtent().Count, Is.EqualTo(2));
        Assert.That(Hotel.GetExtent().Count, Is.EqualTo(1));
        Assert.That(HotelBlock.GetExtent().Count, Is.EqualTo(2));
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
        var block1 = new HotelBlock(hotel, "Block A", address);
        var block2 = new HotelBlock(hotel, "Block B", address);
        
        var r1 = new Room(101, RoomType.Deluxe,hotel, Occupancy.TRIPLE, 300.50, true, true, true, true, true);
        var r2 = new Room(401, RoomType.NoPets, hotel, Occupancy.DOUBLE, 120, false, false, true,
            allergyFriendly: true);
        var r3 = new Room(301, RoomType.PetFriendly, hotel, Occupancy.TRIPLE, 200, true, true, true,
            petFeeders: "Meat", maxPetsAllowed: 2);
        var r4 = new Room(104, RoomType.Standard,hotel, Occupancy.DOUBLE, 100.99, false, false, false);
        
        var e1 = new Employee("Jakub", "Ivanov", 100, block1, EmployeeRole.Cleaner, null);
        var e2 = new Employee("Bob", "Ivanov", 100, block2, EmployeeRole.Cleaner | EmployeeRole.SecurityGuard, null);
        e2.Specialization = Specialization.ROOMS;
        e2.SecurityCode = "SG12345";

        e2.AddRole(EmployeeRole.Receptionist);
        e2.DatabaseKey = "DBKEY123";
        e2.AddLanguage("English");

        e2.RemoveRole(EmployeeRole.Cleaner); 
        var e3 = new Employee("Masha", "Ivanova", 100, block2, EmployeeRole.SecurityGuard, e2);
        
        var g = new Guest("Anna", new DateTime(1990, 04, 01), address1, "99072423358", "0000000001");
        var booking = new Booking(new DateTime(2025, 12, 22), new DateTime(2025, 12, 25), g, r1);
       
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