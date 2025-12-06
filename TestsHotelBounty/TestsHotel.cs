using HotelBounty;
using HotelBounty.ComplexAttributes;

namespace TestsHotelBounty;

public class TestsHotel
{
    [Test]
    public void Hotel_SetAndGetPropertiesCorrectly()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        
        Assert.That(hotel.Name, Is.EqualTo("Hotel Bounty"));
        Assert.That(hotel.City, Is.EqualTo("Warsaw"));
        Assert.That(hotel.PhoneNumber, Is.EqualTo("799039000"));
        Assert.That(hotel.Stars, Is.EqualTo(5));
    }

    [Test]
    public void Hotel_EmptyOrNullNameCityPhoneNumber_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("", "Warsaw", "799039000", 5);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "", "799039000", 5);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "Warsaw", "", 5);
        });
    }
    
    [Test]
    public void Hotel_InvalidPhoneNumber_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "Warsaw", "aaaaaa", 5);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "Warsaw", "333", 5);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "Warsaw", "3a1es14d5633", 5);
        });
    }
    [Test]
    public void Hotel_InvalidStartsNumber_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "Warsaw", "799039000", 0);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "Warsaw", "799039000", -1);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            var hotel = new Hotel("Bounty", "Warsaw", "799039000", 10);
        });
    }
    
    [Test]
    public void HotelBlock_SetAndGetPropertiesCorrectly()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var block = new HotelBlock(hotel, "First hotel block", address);
        
        Assert.That(block.Name, Is.EqualTo("First hotel block"));
        Assert.That(block.Location, Is.EqualTo(address));
        Assert.That(block.Hotel, Is.EqualTo(hotel));
    }
    
    [Test]
    public void HotelBlock_EmptyOrNullName_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
            var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
            var block = new HotelBlock(hotel, "", address);
        });
    }

    [Test]
    public void HotelBlock_NullAddress_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
            var block = new HotelBlock(hotel, "First hotel block", null);
        });
    }
    
    [Test]
    public void HotelBlock_NullHotel_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
            var block = new HotelBlock(null, "First hotel block", address);
        });
    }
}