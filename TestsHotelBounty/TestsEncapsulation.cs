using HotelBounty;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;
using HotelBounty.Enums;

namespace TestsHotelBounty;

public class TestsEncapsulation
{
    [Test]
    public void Employee_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Employee.ClearExtent();
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock("First hotel block", address)
        {
            Hotel = hotel
        };
        var e1 = new Cleaner("Jakub", "Ivanov", 100, null, Specialization.ROOMS){
            HotelBlock = block
        };
        e1.Name = "A";
        var extentEmployee = Employee.GetExtent()[0];
        Assert.That(extentEmployee.Name, Is.EqualTo("A"));
        var extent = Employee.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<Employee>)extent).Add(new Cleaner())
        );
    }
    
    [Test]
    public void Hotel_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        Hotel.ClearExtent();
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        hotel.Name = "Marriott";
        var extentHotel = Hotel.GetExtent()[0];
        Assert.That(extentHotel.Name, Is.EqualTo("Marriott"));
        var extent = Hotel.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<Hotel>)extent).Add(new Hotel())
        );
    }
    
    [Test]
    public void HotelBlock_ModifyingProperty_UpdateObjectButNotBypassEncapsulation()
    {
        HotelBlock.ClearExtent();
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock("First hotel block", address)
        {
            Hotel = hotel
        };
        block.Name = "A";
        var extentBlock = HotelBlock.GetExtent()[0];
        Assert.That(extentBlock.Name, Is.EqualTo("A"));
        var extent = HotelBlock.GetExtent();
        Assert.Throws<NotSupportedException>(() =>
            ((System.Collections.Generic.ICollection<HotelBlock>)extent).Add(new HotelBlock())
        );
    }
}