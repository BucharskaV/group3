using HotelBounty;
using HotelBounty.ComplexAttributes;

namespace TestsHotelBounty;

public class TestsAssociationHoteBlockHotel
{
    [SetUp]
    public void SetUp()
    {
        Hotel.ClearExtent();
        HotelBlock.ClearExtent();
    }

    [Test]
    public void AssignHotel_WorksCorrectly()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        Assert.IsTrue(hotel.HotelBlocks.Contains(block));
    }
    
    [Test]
    public void RemoveHotelBlock_WorksCorrectly()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        hotel.RemoveHotelBlock(block);
        Assert.IsFalse(hotel.HotelBlocks.Contains(block));
    }
    
    [Test]
    public void AssignHotel_NullHotel_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var block = new HotelBlock(null, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));
        });
    }
    
    [Test]
    public void AssignHotel_HotelAlreadyAssigned_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        Assert.Throws<ArgumentException>(() =>
        {
            block.AssignHotel(hotel);
        });
    }
    
    [Test]
    public void AssignHotel_ChangeHotel_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var hotel1 = new Hotel("Hotel ", "Warsaw", "799039888", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        Assert.Throws<InvalidOperationException>(() =>
        {
            block.AssignHotel(hotel1);
        });
    }
    
    [Test]
    public void AddHotelBlock_NullBlock_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        Assert.Throws<ArgumentNullException>(() =>
        {
            hotel.AddHotelBlock(null);
        });
    }
    
    [Test]
    public void AddHotelBlock_AlreadyContainsBlock_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        Assert.Throws<ArgumentException>(() =>
        {
            hotel.AddHotelBlock(block);
        });
    }
    
    [Test]
    public void AddHotelBlock_BlockInAnotherHotel_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var hotel1 = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        Assert.Throws<InvalidOperationException>(() =>
        {
            hotel1.AddHotelBlock(block);
        });
    }

    [Test]
    public void RemoveHotelBlock_NullBlock_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        Assert.Throws<ArgumentNullException>(() =>
        {
            hotel.RemoveHotelBlock(null);
        });
    }
    
    [Test]
    public void RemoveHotelBlock_BlockInAnotherHotel_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var hotel1 = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "Block A",  new Address("Warsaw", "Wola", "Kaspszaka", 55));

        Assert.Throws<InvalidOperationException>(() =>
        {
            hotel1.RemoveHotelBlock(block);
        });
    }
}