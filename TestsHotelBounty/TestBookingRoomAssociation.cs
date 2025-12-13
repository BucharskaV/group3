using HotelBounty;
using HotelBounty.Bookings;
using HotelBounty.Enums;
using HotelBounty.Rooms;
using HotelBounty.ComplexAttributes;

namespace TestsHotelBounty;

public class TestBookingRoomAssociation
{
    private Hotel _hotel;
    private Guest _dummyGuest;
    
    [SetUp]
    public void Setup()
    {
        _hotel = new Hotel(); 
        _dummyGuest = new Guest("Dummy", DateTime.Now.AddYears(-20), new Address("C", "D", "S", 1), "12345678901", "1234567890");
        
    }
    
    
    [Test]
    public void AddBookingToRoom_ShouldCreateBidirectionalReference()
    {
        var room = new Room(101, _hotel, Occupancy.DOUBLE, 150, true, true, true);
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _dummyGuest);

        booking.SetRoom(room);

        Assert.Contains(booking, room.Bookings.ToList());
        Assert.AreEqual(room, booking.Room);
    }
    
    
     [Test]
     public void AddBooking_ShouldThrowArgumentNullException_WhenBookingIsNull()
     {
            
         var room = new Room(101, _hotel, Occupancy.SINGLE, 100, false, true, true);
         
         var ex = Assert.Throws<ArgumentNullException>(() => room.AddBooking(null));
         Assert.AreEqual("booking", ex.ParamName);
     }
     
     
     [Test]
     public void RemoveBooking_ShouldThrow_WhenBookingIsNotInRoom()
     {
         var room = new Room(104, _hotel, Occupancy.SINGLE, 90, true, true, true);
         var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _dummyGuest);

         var ex = Assert.Throws<InvalidOperationException>(() =>
             room.RemoveBooking(booking)
         );

         Assert.AreEqual("This booking is not assigned to the room.", ex.Message);
     }

    
    [Test]
    public void RemoveBookingFromRoom_ShouldRemoveBidirectionalReference()
    {
        var room = new Room(102, _hotel, Occupancy.SINGLE, 100, false, true, true);
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _dummyGuest);
        
        room.AddBooking(booking);
        
        room.RemoveBooking(booking);

        Assert.IsEmpty(room.Bookings);
        Assert.Null(booking.Room); 
    }
    
    [Test]
    public void RemoveBooking_ShouldThrowArgumentNullException_WhenBookingIsNull()
    {
        var room = new Room(102, _hotel, Occupancy.DOUBLE, 150, true, true, true);
        
        var ex = Assert.Throws<ArgumentNullException>(() => room.RemoveBooking(null));
        Assert.AreEqual("booking", ex.ParamName);
    }
    
    
    [Test]
    public void RemoveBooking_ShouldNotThrow_WhenBookingExistsOrNotInRoom()
    {
        var room = new Room(104, _hotel, Occupancy.SINGLE, 90, true, true, true);
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), _dummyGuest);

        room.AddBooking(booking);
        Assert.DoesNotThrow(() => room.RemoveBooking(booking));
        Assert.IsEmpty(room.Bookings);

        var newBooking = new Booking(DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), _dummyGuest);
        Assert.DoesNotThrow(() => room.RemoveBooking(newBooking));
    }
    
    

    [Test]
    public void Booking_ChangeRoom_ReverseConnectionWorks()
    {
        var room1 = new Standard(103, _hotel, Occupancy.SINGLE, 100, true, true, true);
        var room2 = new Standard(104, _hotel, Occupancy.DOUBLE, 150, true, true, true);
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _dummyGuest, room1);

        booking.SetRoom(room2);
        
        Assert.AreEqual(room2, booking.Room);
        
        Assert.IsFalse(room1.Bookings.Contains(booking));
        
        Assert.IsTrue(room2.Bookings.Contains(booking));
    }

    [Test]
    public void Booking_SetRoomToNull_ThrowsExceptionForCompletedOrCanceled()
    {
        var room = new Standard(105, _hotel, Occupancy.SINGLE, 100, true, true, true);
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _dummyGuest, room);
        
        booking.Status = BookingStatus.COMPLETED;

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking.SetRoom(null);
        });
        
        var booking2 = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _dummyGuest);
        booking2.SetRoom(room);
        booking2.CancelBooking();

        Assert.Throws<InvalidOperationException>(() =>
        {
            booking2.SetRoom(null);
        });
    }
    
    [Test]
    public void Room_AddBooking_ShouldThrow_WhenDuplicateBookingAdded()
    {
        var room = new Room(106, _hotel, Occupancy.SINGLE, 80, false, true, true);
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), _dummyGuest);

        room.AddBooking(booking);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            room.AddBooking(booking)
        );

        Assert.AreEqual("This booking is already assigned to the room.", ex.Message);
    }

    
    [Test]
    public void ChangeBookingRoom_ShouldUpdateReferencesProperly()
    {
        var room1 = new Room(103, _hotel, Occupancy.DOUBLE, 120, true, true, true);
        var room2 = new Room(104, _hotel, Occupancy.DOUBLE, 130, true, true, true);
        var booking = new Booking(DateTime.Today.AddDays(1), DateTime.Today.AddDays(4), _dummyGuest);
        booking.SetRoom(room1);

        booking.SetRoom(room2);

        Assert.IsFalse(room1.Bookings.Contains(booking));
        Assert.Contains(booking, room2.Bookings.ToList());
        Assert.AreEqual(room2, booking.Room);
    }
}