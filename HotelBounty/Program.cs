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
        
        var r1 = new Deluxe(101, hotel, Occupancy.TRIPLE, 300.50, true, true, true, true, true);
        var r2 = new NoPets(102, hotel, Occupancy.DOUBLE, 130, true, true, true, true);
        var r3 = new PetFriendly(103, hotel, Occupancy.DOUBLE, 130, false, false, false, "Meat", 2);
        var r4 = new Standard(104, hotel, Occupancy.DOUBLE, 100.99, false, false, false);
        
        var e1 = new Cleaner("Jakub", "Ivanov", 100, block1, null, Specialization.ROOMS);
        var e2 = new Receptionist("Bob", "Ivanov", 100, block2, null, "MyKe12334552");
        var e3 = new SecurityGuard("Masha", "Ivanova", 100, block2, e2,"MyKe12334552", null);
        
        /*e3.SetSupervisor(e2);

        block1.AddEmployee(e1);
        block1.AddEmployee(e2);
        block1.AddEmployee(e3);*/
        
        var g = new Guest("Anna", new DateTime(1990, 04, 01), address1, "99072423358", "0000000001");


        var booking = new Booking(new DateTime(2025, 12, 22), new DateTime(2025, 12, 25), g, r1);
        // {
        //     Room = r1
        // };

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