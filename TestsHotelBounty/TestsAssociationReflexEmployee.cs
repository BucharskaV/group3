using HotelBounty;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;

namespace TestsHotelBounty;

public class TestsAssociationReflexEmployee
{
    private HotelBlock _block;

    [SetUp]
    public void Setup()
    {
        Employee.ClearExtent();
        HotelBlock.ClearExtent();
        Hotel.ClearExtent();
        
        _block = new HotelBlock(new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
    }

    [Test]
    public void SetSupervisor_WorksCorrectly()
    {
        var supervisor = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);
        
        employee.SetSupervisor(supervisor);
        
        Assert.That(employee.Supervisor, Is.EqualTo(supervisor));
        Assert.IsTrue(supervisor.Supervisees.Contains(employee));
    }
    
    [Test]
    public void RemoveSupervisor_WorksCorrectly()
    {
        var supervisor = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);
        
        employee.SetSupervisor(supervisor);
        employee.RemoveSupervisor();
        
        Assert.IsNull(employee.Supervisor);
        Assert.IsFalse(supervisor.Supervisees.Contains(employee));
    }
    
    [Test]
    public void UpdateSupervisor_WorksCorrectly()
    {
        var supervisor1 = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        var supervisor2 = new Receptionist("Milly", "Ivanova", 100, _block, null, "MyKe12334552");
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);
        
        employee.SetSupervisor(supervisor1);
        employee.SetSupervisor(supervisor2);
        
        Assert.That(employee.Supervisor, Is.EqualTo(supervisor2));
        Assert.IsTrue(supervisor2.Supervisees.Contains(employee));
    }
    
    [Test]
    public void SetSupervisor_NullSupervisor_ThrowsException()
    {
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);

        Assert.Throws<ArgumentNullException>(() =>
        {
            employee.SetSupervisor(null);
        });
    }
    
    [Test]
    public void SetSupervisor_SupervisorItself_ThrowsException()
    {
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);

        Assert.Throws<InvalidOperationException>(() =>
        {
            employee.SetSupervisor(employee);
        });
    }
    
    [Test]
    public void SetSupervisor_SupervisorTwice_ThrowsException()
    {
        var supervisor1 = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);

        employee.SetSupervisor(supervisor1);
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            employee.SetSupervisor(supervisor1);
        });
    }
    
    [Test]
    public void SetSupervisor_SuperviseeTwice_ThrowsException()
    {
        var supervisor = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);
        
        employee.SetSupervisor(supervisor);
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            supervisor.AddSupervisee(employee);
        });
    }

    [Test]
    public void RemoveSupervisor_NullSupervisor_ThrowsException()
    {
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            employee.RemoveSupervisor();
        });
    }

    [Test]
    public void AddSupervisee_NullSupervisee_ThrowsException()
    {
        var supervisor = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        
        Assert.Throws<ArgumentNullException>(() =>
        {
            supervisor.AddSupervisee(null);
        });
    }
    
    [Test]
    public void AddSupervisee_SuperviseeItself_ThrowsException()
    {
        var employee = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            employee.AddSupervisee(employee);
        });
    }
    
    [Test]
    public void AddSupervisee_TwiceSupervisee_ThrowsException()
    {
        var supervisor1 = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            supervisor1.AddSupervisee(employee);
        });
    }

    [Test]
    public void RemoveSupervisee_NullSupervisee_ThrowsException()
    {
        var supervisor1 = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        Assert.Throws<ArgumentNullException>(() =>
        {
            supervisor1.RemoveSupervisee(null);
        });
    }
    
    [Test]
    public void RemoveSupervisee_NotSupervisedSupervisee_ThrowsException()
    {
        var supervisor1 = new Receptionist("Bob", "Ivanov", 100, _block, null, "MyKe12334552");
        var employee = new SecurityGuard("Masha", "Ivanova", 100, _block, null,"MyKe12334552", null);

        Assert.Throws<InvalidOperationException>(() =>
        {
            supervisor1.RemoveSupervisee(employee);
        });
    }
    
}
