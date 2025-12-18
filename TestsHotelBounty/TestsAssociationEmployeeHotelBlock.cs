using HotelBounty;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;

namespace TestsHotelBounty;

public class TestsAssociationEmployeeHotelBlock
{
    [SetUp]
    public void SetUp()
    {
        Employee.ClearExtent();
        HotelBlock.ClearExtent();
    }

    [Test]
    public void AddEmployee_WorksCorrectly()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block, EmployeeRole.SecurityGuard, null);
        
        Assert.That(employee.HotelBlock, Is.EqualTo(block));
        Assert.IsTrue(block.Employees.Contains(employee));
    }
    
    [Test]
    public void RemoveEmployee_WorksCorrectly()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block, EmployeeRole.SecurityGuard, null);
        var employee1 = new Employee("Sasha", "Ivanov", 100, block, EmployeeRole.SecurityGuard, null);

        block.RemoveEmployee(employee);
        
        Assert.IsNull(employee.HotelBlock);
        Assert.IsFalse(block.Employees.Contains(employee));
    }

    [Test]
    public void ChangeHotelBlock_WorksCorrectly()
    {
        var block1 = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var block2 = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block1, EmployeeRole.SecurityGuard, null);
        var employee1 = new Employee("Sasha", "Ivanov", 100, block1, EmployeeRole.SecurityGuard, null);

        employee.ChangeHotelBlock(block2);
        
        Assert.That(employee.HotelBlock, Is.EqualTo(block2));
        Assert.IsTrue(block2.Employees.Contains(employee));
    }

    [Test]
    public void AddEmployee_NullEmployee_ThrowsArgument()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        Assert.Throws<ArgumentNullException>(() => block.AddEmployee(null));
    }
    
    [Test]
    public void AddEmployee_EmployeeAlreadyInBlock_ThrowsArgument()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block, EmployeeRole.SecurityGuard, null);
        Assert.Throws<InvalidOperationException>(() => block.AddEmployee(employee));
    }
    
    [Test]
    public void AddEmployee_EmployeeAlreadyInAnotherBlock_ThrowsArgument()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var block2 = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block, EmployeeRole.SecurityGuard, null);
        Assert.Throws<InvalidOperationException>(() => block2.AddEmployee(employee));
    }
    
    [Test]
    public void RemoveEmployee_NullEmployee_ThrowsException()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        
        Assert.Throws<ArgumentNullException>(() => block.RemoveEmployee(null));
    }
    
    [Test]
    public void RemoveEmployee_EmployeeFromAnotherBlock_ThrowsException()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var block2 = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block, EmployeeRole.SecurityGuard, null);
        Assert.Throws<InvalidOperationException>(() => block2.RemoveEmployee(employee));
    }
    
    [Test]
    public void RemoveEmployee_LastEmployee_ThrowsException()
    {
        var block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block, EmployeeRole.SecurityGuard, null);
        
        Assert.Throws<InvalidOperationException>(() => block.RemoveEmployee(employee));
    }
    
    [Test]
    public void ChangeHotelBlock_LastEmployee_ThrowsException()
    {
        var block1 = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var block2 = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        var employee = new Employee("Masha", "Ivanova", 100, block1, EmployeeRole.SecurityGuard, null);
        
        Assert.Throws<InvalidOperationException>(() => employee.ChangeHotelBlock(block2));
    }
}