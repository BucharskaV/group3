using HotelBounty;
using HotelBounty.Employees;
using HotelBounty.Enums;

namespace TestsHotelBounty;

public class TestsEmployee
{
    [Test]
    public void Employee_SetAndGetPropertiesCorrectly()
    {
        var block = new HotelBlock();
        var employee = new Employee(
            "Anna",
            "Smith",
            200,
            block,
            EmployeeRole.Cleaner
        );

        employee.Specialization = Specialization.ROOMS;
        Assert.That(employee.Name, Is.EqualTo("Anna"));
        Assert.That(employee.Surname, Is.EqualTo("Smith"));
        Assert.That(employee.Bonus, Is.EqualTo(200));
        Assert.That(employee.Salary, Is.EqualTo(Employee.MinSalary + employee.Bonus));
        Assert.That(employee.Specialization, Is.EqualTo(Specialization.ROOMS));
    }

    [Test]
    public void Employee_NegativeMinSalary_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Employee.MinSalary = -1000;
        });
    }
    
    [Test]
    public void Employee_EmptyNameOrSurname_ThrowsException()
    {
        var block = new HotelBlock();

        Assert.Throws<ArgumentException>(() =>
        {
            new Employee("", "Ivanov", 100, block, EmployeeRole.Cleaner);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            new Employee("Jakub", "", 100, block, EmployeeRole.Cleaner);
        });
    }

    [Test]
    public void Employee_NegativeBonus_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var block = new HotelBlock();
            var emp =  new Employee("Jakub", "Ivanov", -100, block, EmployeeRole.Cleaner);
        });
    }

    [Test]
    public void Receptionist_SetAndGetPropertiesCorrectly()
    {
        var block = new HotelBlock();
        var rec = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.Receptionist
        );

        rec.DatabaseKey = "MyKey12345";
        rec.SetLanguages(new List<string> { "English", "French" });
        Assert.That(rec.DatabaseKey, Is.EqualTo("MyKey12345"));
        CollectionAssert.Contains(rec.Languages, "English");
        CollectionAssert.Contains(rec.Languages, "French");
    }
    
    [Test]
    public void Receptionist_InvalidDatabaseKey_ThrowsException()
    {
        var block = new HotelBlock();
        var emp = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.Receptionist
        );

        Assert.Throws<ArgumentException>(() => emp.DatabaseKey = "");
        Assert.Throws<ArgumentException>(() => emp.DatabaseKey = new string('A', 21));
        Assert.Throws<ArgumentException>(() => emp.DatabaseKey = "key1");
        Assert.Throws<ArgumentException>(() => emp.DatabaseKey = "$Invalid$");
    }

    [Test]
    public void Receptionist_SetNullOrEmptyLanguages_ThrowsException()
    {
        var block = new HotelBlock();
        var emp = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.Receptionist
        );

        Assert.Throws<ArgumentNullException>(() => emp.SetLanguages(null));
        Assert.Throws<ArgumentException>(() => emp.SetLanguages(new List<string>()));
        Assert.Throws<ArgumentException>(() => emp.SetLanguages(new List<string> { "" }));
    }

    [Test]
    public void Receptionist_AddAndRemoveLanguage_WorksCorrectly()
    {
        var block = new HotelBlock();
        var emp = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.Receptionist
        );

        emp.SetLanguages(new List<string> { "English" });
        emp.AddLanguage("Spanish");

        CollectionAssert.Contains(emp.Languages, "Spanish");

        emp.RemoveLanguage("Spanish");

        CollectionAssert.DoesNotContain(emp.Languages, "Spanish");
    }

    [Test]
    public void Receptionist_RemoveLastLanguage_Throws()
    {
        var block = new HotelBlock();
        var emp = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.Receptionist
        );

        emp.SetLanguages(new List<string> { "English" });

        Assert.Throws<InvalidOperationException>(() =>
        {
            emp.RemoveLanguage("English");
        });
    }
    
    [Test]
    public void SecurityGuard_SetAndGetPropertiesCorrectly()
    {
        var block = new HotelBlock();
        var emp = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.SecurityGuard
        );

        emp.SecurityCode = "SG12345";
        emp.AccessToWeapons = "Full access";

        Assert.That(emp.SecurityCode, Is.EqualTo("SG12345"));
        Assert.That(emp.AccessToWeapons, Is.EqualTo("Full access"));
    }
    
    [Test]
    public void SecurityGuard_InvalidSecurityCode_ThrowsException()
    {
        var block = new HotelBlock();
        var emp = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.SecurityGuard
        );

        Assert.Throws<ArgumentException>(() => emp.SecurityCode = "");
        Assert.Throws<ArgumentException>(() => emp.SecurityCode = new string('A', 21));
        Assert.Throws<ArgumentException>(() => emp.SecurityCode = "key1");
        Assert.Throws<ArgumentException>(() => emp.SecurityCode = "$Invalid$");
    }


    [Test]
    public void SecurityGuard_InvalidAccessToWeaponsDescription_ThrowsException()
    {
        var block = new HotelBlock();
        var emp = new Employee(
            "Jakub",
            "Ivanov",
            100,
            block,
            EmployeeRole.SecurityGuard
        );

        Assert.Throws<ArgumentException>(() =>
        {
            emp.AccessToWeapons = new string('A', 51);
        });
    }
}