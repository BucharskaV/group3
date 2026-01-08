using HotelBounty;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;
using HotelBounty.Enums;
using HotelBounty.Persistence;
using HotelBounty.Rooms;

class Program
{
    static void Main(string[] args)
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
        var r4 = new Room(104, RoomType.Standard ,hotel, Occupancy.DOUBLE, 100.99, false, false, false);
        
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
        
        var booking = new Booking(new DateTime(2026, 12, 22), new DateTime(2026, 12, 25), g, r1);
      
        var bill = new Bill(booking);

        var paymentOperation = new PaymentOperation(bill, booking, PaymentMethod.CARD, 1000);
        
        Console.WriteLine("\n--- Original extent ---");
        PrintAllExtents();

        ExtentPersistence.Save();
        Console.WriteLine("\nExtent saved.\n");

        Address.ClearExtent();
        Hotel.ClearExtent();
        HotelBlock.ClearExtent();
        Room.ClearExtent();
        Employee.ClearExtent();
        Guest.ClearExtent();
        Booking.ClearExtent();
        Bill.ClearExtent();
        PaymentOperation.ClearExtent();

        var loaded = ExtentPersistence.Load("hotels.xml");
        Console.WriteLine(loaded ? "Extent loaded successfully.\n" : "Failed to load extent.\n");

        PrintAllExtents();

        if (Employee.GetExtent().Count() != 3)
            throw new Exception("Employee extent count mismatch after load!");
        
        if (Room.GetExtent().Count() != 4)
            throw new Exception("Room extent count mismatch after load!");

        if (Hotel.GetExtent().Count() != 1)
            throw new Exception("Hotel extent count mismatch after load!");

        if (Address.GetExtent().Count() != 2)
            throw new Exception("Address extent count mismatch after load!");

        if (HotelBlock.GetExtent().Count() != 2)
            throw new Exception("HotelBlock extent count mismatch after load!");
        
        if (Guest.GetExtent().Count() != 1)
            throw new Exception("Guest extent count mismatch after load!");
        
        if (Booking.GetExtent().Count() != 1)
            throw new Exception("Booking extent count mismatch after load!");
        
        if (Bill.GetExtent().Count() != 1)
            throw new Exception("Bill extent count mismatch after load!");
        
        if (PaymentOperation.GetExtent().Count() != 1)
            throw new Exception("PaymentOperation extent count mismatch after load!");
    }
    
    static void PrintAllExtents()
    {
        Console.WriteLine("\nAddresses:");
        foreach (var a in Address.GetExtent()) Console.WriteLine(a);

        Console.WriteLine("\nHotels:");
        foreach (var h in Hotel.GetExtent()) Console.WriteLine(h);

        Console.WriteLine("\nHotelBlocks:");
        foreach (var hb in HotelBlock.GetExtent()) Console.WriteLine(hb);
        
        Console.WriteLine("\nRooms:");
        foreach (var r in Room.GetExtent()) Console.WriteLine(r);

        Console.WriteLine("\nEmployees:");
        foreach (var e in Employee.GetExtent()) Console.WriteLine(e);
        
        Console.WriteLine("\nGuests:");
        foreach (var g in Guest.GetExtent()) Console.WriteLine(g);
        
        Console.WriteLine("\nBookings:");
        foreach (var b in Booking.GetExtent()) Console.WriteLine(b);
        
        Console.WriteLine("\nBills:");
        foreach (var b in Bill.GetExtent()) Console.WriteLine(b);
        
        Console.WriteLine("\nPayments:");
        foreach (var p in PaymentOperation.GetExtent()) Console.WriteLine(p);
    }
}