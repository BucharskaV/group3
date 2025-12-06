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
        var employee = new Cleaner("Anna", "Smith", 200, Specialization.ROOMS)
        {
            HotelBlock = block
        };
        
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
        Assert.Throws<ArgumentException>(() =>
        {
            var emp1 = new Cleaner("", "Ivanov", 100, Specialization.ROOMS);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var emp2 = new Cleaner("Jakub", "", 100, Specialization.ROOMS);
        });
    }
    
    [Test]
    public void Employee_NegativeBonus_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var emp = new Cleaner("Jakub", "Ivanov", -100, Specialization.ROOMS);
        });
    }
    
    [Test]
    public void Employee_HotelBlockIsNull_ThrowsException()
    {
        var emp = new Cleaner("Jakub", "Ivanov", 100, Specialization.ROOMS);
        Assert.Throws<ArgumentNullException>(() =>
        {
            emp.HotelBlock = null;
        });
    }

    [Test]
    public void Receptionist_SetAndGetPropertiesCorrectly()
    {
        var rec = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
        rec.SetLanguages(new List<string>() { "English", "French" });
        
        Assert.That(rec.DatabaseKey, Is.EqualTo("MyKey12345"));
        CollectionAssert.Contains(rec.Languages, "English");
        CollectionAssert.Contains(rec.Languages, "French");
    }
    
    [Test]
    public void Receptionist_InvalidDatabaseKey_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var rec = new Receptionist("Jakub", "Ivanov", 100, "");
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var rec = new Receptionist("Jakub", "Ivanov", 100, "TheLengthIsGreaterThan20xxxxxx");
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var rec = new Receptionist("Jakub", "Ivanov", 100, "key1");
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var rec = new Receptionist("Jakub", "Ivanov", 100, "$NotOnlyAlphaNumeric$");
        });
    }

    [Test]
    public void Receptionist_SetNullOrEmptyLanguages_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var rec1 = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
            rec1.SetLanguages(null);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var rec2 = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
            rec2.SetLanguages(new List<string>(){"English", ""});
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var rec3 = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
            rec3.SetLanguages(new List<string>());
        });
    }

    [Test]
    public void Receptionist_AddLanguage_SetPropertyCorrectly()
    {
        var rec = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
        rec.SetLanguages(new List<string>() { "English" });
        rec.AddLanguage("Spanish");
        CollectionAssert.Contains(rec.Languages, "English");
        CollectionAssert.Contains(rec.Languages, "Spanish");
    }

    [Test]
    public void Receptionist_AddEmptyOrDublicateLanguage_ThrowsException()
    {
        var rec = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
        rec.SetLanguages(new List<string>() { "English" });
        Assert.Throws<ArgumentException>(() =>
        {
            rec.AddLanguage("");
        });
        Assert.Throws<ArgumentException>(() =>
        {
            rec.AddLanguage("English");
        });
    }
    
    [Test]
    public void Receptionist_RemoveLanguage_Correctly()
    {
        var rec = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
        rec.SetLanguages(new List<string>() { "English", "Spanish" });
        rec.RemoveLanguage("Spanish");
    }
    
    [Test]
    public void Receptionist_RemoveEmptyOrLastLanguage_ThrowsException()
    {
        var rec = new Receptionist("Jakub", "Ivanov", 100, "MyKey12345");
        rec.SetLanguages(new List<string>() { "English" });
        Assert.Throws<ArgumentException>(() =>
        {
            rec.RemoveLanguage("");
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            rec.RemoveLanguage("English");
        });
    }
    
    [Test]
    public void SecurityGuard_SetAndGetPropertiesCorrectly()
    {
        var security = new SecurityGuard("Jakub", "Ivanov", 100, "MyKey12345", "Full access");
        Assert.That(security.SecurityCode, Is.EqualTo("MyKey12345"));
        Assert.That(security.AccessToWeapons, Is.EqualTo("Full access"));
    }
    
    [Test]
    public void SecurityGuard_InvalidDatabaseKey_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var security = new SecurityGuard("Jakub", "Ivanov", 100, "", "Full access");
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var security = new SecurityGuard("Jakub", "Ivanov", 100, new string('A', 21), "Full access");
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var security = new SecurityGuard("Jakub", "Ivanov", 100, "key1", "Full access");
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var security = new SecurityGuard("Jakub", "Ivanov", 100, "$NotOnlyAlphaNumeric$", "Full access");
        });
    }

    [Test]
    public void SecurityGuard_InvalidAccessToWeaponsDescription_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var security = new SecurityGuard("Jakub", "Ivanov", 100, "MyKe12345", new string('A', 51));
        });
    }
}