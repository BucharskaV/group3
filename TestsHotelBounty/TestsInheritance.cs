using HotelBounty;
using HotelBounty.Employees;
using HotelBounty.Enums;

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
}