using HotelBounty;
using HotelBounty.Employees;
using HotelBounty.Enums;
using HotelBounty.Persistence;

class Program
{
    static void Main(string[] args)
    {
        //for testing, in future the appropriate unit tests will be added
        Console.WriteLine("=== Testing Extent Persistence ===");

        Employee.ClearExtent();

        var block = new HotelBlock();
        var e1 = new Cleaner("Jakub", "Ivanov", 100, null, Specialization.ROOMS){
            HotelBlock = block
        };
        var e2 = new Receptionist("Bob", "Ivanov", 100, null, "MyKe12334552"){
            HotelBlock = block
        };
        var e3 = new SecurityGuard("Masha", "Ivanova", 100, e2, "MyKe12334552", null){
            HotelBlock = block
        };
        

        Console.WriteLine("Original extent:");
        foreach (var e in Employee.GetExtent())
            Console.WriteLine(e);

        ExtentPersistence.Save();
        Console.WriteLine("Extent saved.");

        Employee.ClearExtent();

        if (Employee.GetExtent().Any())
            throw new Exception("Extent should be empty after ClearExtent!");

        var loaded = ExtentPersistence.Load("hotels.xml");

        Console.WriteLine(loaded
            ? "Extent loaded successfully."
            : "Failed to load extent.");

        var originalEmployees = new Employee[] { e1, e2, e3 };
        var loadedEmployees = Employee.GetExtent().ToList();

        if (loadedEmployees.Count != originalEmployees.Length)
            throw new Exception("Extent count mismatch after load!");

        Console.WriteLine("Extent restored correctly");
    }
}