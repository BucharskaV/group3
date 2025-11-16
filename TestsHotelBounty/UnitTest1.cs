using HotelBounty;
using HotelBounty.Employees;

namespace TestsHotelBounty;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        var instance = new Employee();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}