using HotelBounty;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;
using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace TestsHotelBounty;

public class TestsAssociationBookingGuest
{
    private Address _address;
    private Room _room;

    [SetUp]
    public void SetUp()
    {
        Booking.ClearExtent();
        Guest.ClearExtent();
        Room.ClearExtent();
        Hotel.ClearExtent();
        
        _address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        _room = new Standard(101, new Hotel("Hotel Bounty", "Warsaw", "799039000", 5), Occupancy.SINGLE, 100, true, true, true);
    }

    [Test]
    public void CreateBooking_WorksCorrectly()
    {
        var guest = new Guest("Anna", DateTime.Now.AddYears(-25), _address, "12345678901", "1234567890");
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), guest, _room);
        
        Assert.That(booking.Guest, Is.EqualTo(guest));
        Assert.IsTrue(guest.Bookings.Contains(booking));
    }

    [Test]
    public void SetGuest_WorksCorrectly()
    {
        var guest1 = new Guest("Anna", DateTime.Now.AddYears(-25), _address, "11111111111", "1111111111");
        var guest2 = new Guest("Bob", DateTime.Now.AddYears(-30), _address, "22222222222", "2222222222");
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), guest1, _room);

        booking.SetGuest(guest2);

        Assert.That(booking.Guest, Is.EqualTo(guest2));
        Assert.IsTrue(guest2.Bookings.Contains(booking));
        Assert.IsFalse(guest1.Bookings.Contains(booking));
    }

    [Test]
    public void CreateBooking_NullGuest_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), null, _room);
        });
    }
    
    [Test]
    public void SetGuest_NullGuest_ThrowsException()
    {
        var guest = new Guest("Anna", DateTime.Now.AddYears(-25), _address, "12345678901", "1234567890");
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), guest, _room);

        Assert.Throws<ArgumentNullException>(() =>
        {
            booking.SetGuest(null);
        });
    }
}