using HotelBounty;
using HotelBounty.Employees;
using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace TestsHotelBounty;

public class TestsInheritance
{
    [SetUp]
    public void Setup()
    {
        Employee.ClearExtent();
    }
    
    [Test]
    public void Employee_IsSingleClass_ForAllRoles_WorksCorrectly()
    {
        var block = new HotelBlock();

        var cleaner = new Employee("Anna", "Smith", 100, block, EmployeeRole.Cleaner);
        var receptionist = new Employee("Bob", "Jones", 120, block, EmployeeRole.Receptionist);
        var guard = new Employee("Eve", "Brown", 150, block, EmployeeRole.SecurityGuard);

        Assert.That(cleaner, Is.TypeOf<Employee>());
        Assert.That(receptionist, Is.TypeOf<Employee>());
        Assert.That(guard, Is.TypeOf<Employee>());
    }
    
    [Test]
    public void Employee_MultipleRolesAtTheSameTime_WorksCorrectly()
    {
        var block = new HotelBlock();

        var emp = new Employee("Jakub", "Ivanov", 200, block, EmployeeRole.Cleaner | EmployeeRole.Receptionist);

        emp.Specialization = Specialization.ROOMS;
        emp.DatabaseKey = "DB12345";
        emp.SetLanguages(new[] { "English" });

        Assert.That(emp.Roles.HasFlag(EmployeeRole.Cleaner));
        Assert.That(emp.Roles.HasFlag(EmployeeRole.Receptionist));
    }
    
    [Test]
    public void Employee_CanChangeRolesDynamically_WorksCorrectly()
    {
        var block = new HotelBlock();

        var emp = new Employee("Jakub", "Ivanov", 200, block, EmployeeRole.Cleaner);

        emp.Specialization = Specialization.ROOMS;
        
        emp.AddRole(EmployeeRole.SecurityGuard);
        emp.SecurityCode = "SG12345";

        emp.RemoveRole(EmployeeRole.Cleaner);

        Assert.That(emp.Roles.HasFlag(EmployeeRole.SecurityGuard));
        Assert.That(emp.Roles.HasFlag(EmployeeRole.Cleaner), Is.False);
        Assert.That(emp.Specialization, Is.Null);
    }
    
    [Test]
    public void Employee_UsingCleanerPropertyWithoutRole_ThrowsException()
    {
        var block = new HotelBlock();

        var emp = new Employee("Jakub", "Ivanov", 200, block, EmployeeRole.Receptionist);

        Assert.Throws<InvalidOperationException>(() =>
        {
            emp.Specialization = Specialization.ROOMS;
        });
    }
    
    [Test]
    public void Employee_UsingReceptionistPropertyWithoutRole_ThrowsException()
    {
        var block = new HotelBlock();

        var emp = new Employee("Jakub", "Ivanov", 200, block, EmployeeRole.Cleaner);

        Assert.Throws<InvalidOperationException>(() =>
        {
            emp.DatabaseKey = "DB12345";
        });
    }
    
    [Test]
    public void Employee_UsingSecurityGuardPropertyWithoutRole_ThrowsException()
    {
        var block = new HotelBlock();

        var emp = new Employee("Jakub", "Ivanov", 200, block, EmployeeRole.Cleaner);

        Assert.Throws<InvalidOperationException>(() =>
        {
            emp.SecurityCode = "CODE12345";
        });
    }
    
    [Test]
    public void Employee_AddExistingRole_ThrowsException()
    {
        var block = new HotelBlock();

        var emp = new Employee("Jakub", "Ivanov", 200, block, EmployeeRole.Cleaner);

        Assert.Throws<InvalidOperationException>(() =>
        {
            emp.AddRole(EmployeeRole.Cleaner);
        });
    }
    
    [Test]
    public void Employee_RemoveNotAssignedRole_ThrowsException()
    {
        var block = new HotelBlock();

        var emp = new Employee("Jakub", "Ivanov", 200, block, EmployeeRole.Cleaner);

        Assert.Throws<InvalidOperationException>(() =>
        {
            emp.RemoveRole(EmployeeRole.Receptionist);
        });
    }


    
    
    
    [Test]
    public void Room_IsSingleClass_ForAllTypes_WorksCorrectly()
    {
        var hotel = new Hotel();

        var standardRoom = new Room(101, RoomType.Standard, hotel, Occupancy.SINGLE, 100, true, true, true);
        var deluxeRoom = new Room(102, RoomType.Deluxe, hotel, Occupancy.DOUBLE, 200, true, true, true, 
            terrace: true, extraBed: true);
        var petFriendlyRoom = new Room(103, RoomType.PetFriendly, hotel, Occupancy.DOUBLE, 150, true, true, true,
            petFeeders: "Standard feeders", maxPetsAllowed: 2);
        var noPetsRoom = new Room(104, RoomType.NoPets, hotel, Occupancy.SINGLE, 120, true, true, true,
            allergyFriendly: true);

        Assert.That(standardRoom, Is.TypeOf<Room>());
        Assert.That(deluxeRoom, Is.TypeOf<Room>());
        Assert.That(petFriendlyRoom, Is.TypeOf<Room>());
        Assert.That(noPetsRoom, Is.TypeOf<Room>());
    }
    
    [Test]
    public void Room_DeluxeType_HasSpecificProperties_WorksCorrectly()
    {
        var hotel = new Hotel();

        var deluxeRoom = new Room(201, RoomType.Deluxe, hotel, Occupancy.DOUBLE, 250, true, true, true,
            terrace: true, extraBed: true);

        Assert.That(deluxeRoom.Type, Is.EqualTo(RoomType.Deluxe));
        Assert.That(deluxeRoom.Terrace, Is.True);
        Assert.That(deluxeRoom.ExtraBed, Is.True);
    }
    
    [Test]
    public void Room_PetFriendlyType_HasSpecificProperties_WorksCorrectly()
    {
        var hotel = new Hotel();

        var petFriendlyRoom = new Room(301, RoomType.PetFriendly, hotel, Occupancy.DOUBLE, 180, true, true, true,
            petFeeders: "Premium feeders", maxPetsAllowed: 3);

        Assert.That(petFriendlyRoom.Type, Is.EqualTo(RoomType.PetFriendly));
        Assert.That(petFriendlyRoom.PetFeeders, Is.EqualTo("Premium feeders"));
        Assert.That(petFriendlyRoom.MaxPetsAllowed, Is.EqualTo(3));
    }
    
    [Test]
    public void Room_NoPetsType_HasSpecificProperties_WorksCorrectly()
    {
        var hotel = new Hotel();

        var noPetsRoom = new Room(401, RoomType.NoPets, hotel, Occupancy.SINGLE, 130, true, true, true,
            allergyFriendly: true);

        Assert.That(noPetsRoom.Type, Is.EqualTo(RoomType.NoPets));
        Assert.That(noPetsRoom.AllergyFriendly, Is.True);
    }
    
    [Test]
    public void Room_PetFriendlyWithoutPetFeeders_ThrowsException()
    {
        var hotel = new Hotel();

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new Room(501, RoomType.PetFriendly, hotel, Occupancy.DOUBLE, 150, true, true, true,
                petFeeders: null, maxPetsAllowed: 2);
        });
    }
    
    [Test]
    public void Room_PetFriendlyWithTooManyPets_ThrowsException()
    {
        var hotel = new Hotel();

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new Room(601, RoomType.PetFriendly, hotel, Occupancy.DOUBLE, 150, true, true, true,
                petFeeders: "Standard feeders", maxPetsAllowed: 5);
        });
    }
    
    [Test]
    public void Room_CanSetMiniBarFilling_WorksCorrectly()
    {
        var hotel = new Hotel();

        var deluxeRoom = new Room(701, RoomType.Deluxe, hotel, Occupancy.DOUBLE, 250, true, true, true,
            terrace: true, extraBed: true);

        deluxeRoom.SetMiniBarFilling(new[] { "Water", "Juice", "Snacks" });

        Assert.That(deluxeRoom.MiniBarFilling.Count, Is.EqualTo(3));
        Assert.That(deluxeRoom.MiniBarFilling, Contains.Item("Water"));
    }
    
    [Test]
    public void Room_SetMiniBarFillingWithEmptyList_ThrowsException()
    {
        var hotel = new Hotel();

        var room = new Room(801, RoomType.Deluxe, hotel, Occupancy.DOUBLE, 250, true, true, true);

        Assert.Throws<ArgumentException>(() =>
        {
            room.SetMiniBarFilling(new string[] { });
        });
    }
    
    [Test]
    public void Room_WithNegativePrice_ThrowsException()
    {
        var hotel = new Hotel();

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new Room(901, RoomType.Standard, hotel, Occupancy.SINGLE, -100, true, true, true);
        });
    }
    
    [Test]
    public void Room_SetNegativePrice_ThrowsException()
    {
        var hotel = new Hotel();
        var room = new Room(902, RoomType.Standard, hotel, Occupancy.SINGLE, 100, true, true, true);

        Assert.Throws<ArgumentException>(() =>
        {
            room.Price = -50;
        });
    }
    
    [Test]
    public void Room_WithInvalidRoomNumber_ThrowsException()
    {
        var hotel = new Hotel();

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new Room(0, RoomType.Standard, hotel, Occupancy.SINGLE, 100, true, true, true);
        });
    }
    
    [Test]
    public void Room_SetInvalidRoomNumber_ThrowsException()
    {
        var hotel = new Hotel();
        var room = new Room(903, RoomType.Standard, hotel, Occupancy.SINGLE, 100, true, true, true);

        Assert.Throws<ArgumentException>(() =>
        {
            room.RoomNumber = -5;
        });
    }
    
    [Test]
    public void Room_WithNullHotel_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var room = new Room(904, RoomType.Standard, null, Occupancy.SINGLE, 100, true, true, true);
        });
    }
    
    [Test]
    public void Room_SetMiniBarFillingWithNullValues_ThrowsException()
    {
        var hotel = new Hotel();
        var room = new Room(905, RoomType.Deluxe, hotel, Occupancy.DOUBLE, 250, true, true, true);

        Assert.Throws<ArgumentException>(() =>
        {
            room.SetMiniBarFilling(new[] { "Water", "", "Juice" });
        });
    }
    
    [Test]
    public void Room_SetMiniBarFillingWithWhitespace_ThrowsException()
    {
        var hotel = new Hotel();
        var room = new Room(906, RoomType.Deluxe, hotel, Occupancy.DOUBLE, 250, true, true, true);

        Assert.Throws<ArgumentException>(() =>
        {
            room.SetMiniBarFilling(new[] { "Water", "   ", "Juice" });
        });
    }
}