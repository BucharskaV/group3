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
    public void AddBooking_WorksCorrectly()
    {
        var guest = new Guest("Anna", DateTime.Now.AddYears(-25), _address, "12345678901", "1234567890");
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), guest, _room);
        
        Assert.Contains(booking, guest.Bookings.ToList());
        Assert.That(booking.Guest, Is.EqualTo(guest));
    }
    
    [Test]
    public void RemoveBooking_WorksCorrectly()
    {
        var guest = new Guest("Bob", DateTime.Today.AddYears(-28), new Address(), "11122233344", "1112223333");
        var b1 = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), guest);
        var b2 = new Booking(DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), guest);
        
        b1.UnsetGuest();
        Assert.IsFalse(guest.Bookings.Contains(b1));
        
        Assert.Throws<ArgumentNullException>(() => b1.UnsetGuest());

        Assert.Throws<InvalidOperationException>(() => guest.RemoveBooking(b2));
    }
    
    [Test]
    public void RemoveBooking_AtLeastOneBooking_ThrowsException()
    {
        var guest = new Guest("Charlie", DateTime.Today.AddYears(-40), new Address(), "55566677788", "5556667777");
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), guest);

        Assert.Throws<InvalidOperationException>(() => guest.RemoveBooking(booking));
    }
    
    [Test]
    public void SetGuest_NullGuest_ThrowsException()
    {
        var guest = new Guest("David", DateTime.Today.AddYears(-35), new Address(), "99988877766", "9998887776");
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), guest);

        Assert.Throws<ArgumentNullException>(() => booking.SetGuest(null));
    }
    
    [Test]
    public void AddBooking_SameBookingTwice_ShouldThrowException()
    {
        var guest = new Guest("Eve", DateTime.Today.AddYears(-27), new Address(), "22233344455", "2223334445");
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), guest);

        Assert.Throws<ArgumentException>(() => guest.AddBooking(booking));
    }
    
    [Test]
    public void SetGuest_Update_WorksCorrectly()
    {
        var guest1 = new Guest("Fiona", DateTime.Today.AddYears(-30), new Address(), "12312312312", "1231231231");
        var guest2 = new Guest("George", DateTime.Today.AddYears(-29), new Address(), "32132132132", "3213213213");
        var booking1 = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), guest1);
        var booking2 = new Booking(DateTime.Today.AddDays(3), DateTime.Today.AddDays(4), guest1);

        booking1.SetGuest(guest2);

        Assert.That(booking1.Guest, Is.EqualTo(guest2));
        Assert.IsTrue(guest2.Bookings.Contains(booking1));
        Assert.IsTrue(guest1.Bookings.Contains(booking2));
        Assert.IsFalse(guest1.Bookings.Contains(booking1));
    }

    [Test]
    public void SetGuest_WorksCorrectly()
    {
        var guest1 = new Guest("Anna", DateTime.Now.AddYears(-25), _address, "11111111111", "1111111111");
        var guest2 = new Guest("Bob", DateTime.Now.AddYears(-30), _address, "22222222222", "2222222222");
        var booking1 = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), guest1, _room);
        var booking2 = new Booking(DateTime.Today.AddDays(4), DateTime.Today.AddDays(5), guest1, _room);

        booking1.SetGuest(guest2);

        Assert.That(booking1.Guest, Is.EqualTo(guest2));
        Assert.IsTrue(guest2.Bookings.Contains(booking1));
        Assert.IsTrue(guest1.Bookings.Contains(booking2));
        Assert.IsFalse(guest1.Bookings.Contains(booking1));
    }

    [Test]
    public void CreateBooking_NullGuest_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), null, _room);
        });
    }
    
}