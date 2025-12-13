using HotelBounty;
using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace TestsHotelBounty;

public class TestHotelRoomAssociation
{
    private Hotel _hotel1;
    private Hotel _hotel2;

    [SetUp]
    public void Setup()
    {
        _hotel1 = new Hotel("Hotel One", "New York", "123456789", 4);
        _hotel2 = new Hotel("Hotel Two", "Rio", "987654321", 5);
    }
    
    [Test]
    public void AddRoom_ShouldCreateBidirectionalReference()
    {
        var room = new Room(101, _hotel1, Occupancy.SINGLE, 100, true, true, true);
        
        _hotel1.AddRoom(room);
        
        Assert.AreEqual(_hotel1, room.Hotel);
        Assert.IsTrue(_hotel1.Rooms.ContainsKey(101));
        Assert.AreEqual(room, _hotel1.GetRoom(101));
    }
    
    [Test]
    public void RemoveRoom_ShouldDeleteBidirectionalReference()
    {
        var room = new Room(102, _hotel1, Occupancy.DOUBLE, 150, true, true, true);
        _hotel1.AddRoom(room);
        
        _hotel1.RemoveRoom(102);
        
        Assert.IsNull(room.Hotel);
        Assert.IsFalse(_hotel1.Rooms.ContainsKey(102));
    }
    
    [Test]
    public void MoveRoomBetweenHotels_ShouldUpdateReferences()
    {
        var room = new Room(103, _hotel1, Occupancy.DOUBLE, 120, true, true, true);
        
        room.SetHotel(_hotel2);
        
        Assert.AreEqual(_hotel2, room.Hotel);
        Assert.IsFalse(_hotel1.Rooms.ContainsKey(103));
        Assert.IsTrue(_hotel2.Rooms.ContainsKey(103));
    }

    [Test]
    public void AddRoom_ShouldThrow_WhenRoomIsNull()
    {
      
        var ex = Assert.Throws<ArgumentNullException>(() => _hotel1.AddRoom(null));
        Assert.AreEqual("room", ex.ParamName);
    }
    
    
    [Test]
    public void AddRoom_ShouldThrow_WhenRoomNumberAlreadyExists()
    {
        var room1 = new Room(104, _hotel1, Occupancy.SINGLE, 80, false, true, true);
        var room2 = new Room(104, _hotel2, Occupancy.DOUBLE, 120, true, true, true); // different hotel
        
        var ex = Assert.Throws<InvalidOperationException>(() => room2.SetHotel(_hotel1));
        Assert.AreEqual("Room 104 already exists in this hotel.", ex.Message);
    }


    [Test]
    public void RemoveRoom_ShouldThrow_WhenRoomDoesNotExist()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => _hotel1.RemoveRoom(999));
        Assert.AreEqual("Room 999 does not exist in this hotel.", ex.Message);
    }

    [Test]
    public void CreatingRoom_ShouldThrow_WhenHotelIsNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
            new Room(105, null, Occupancy.SINGLE, 100, true, true, true)
        );
        Assert.AreEqual("When creating the room the Hotel cannot be null", ex.ParamName);
    }
    
    [Test]
    public void ChangeRoomNumber_ShouldUpdateHotelBinding()
    {
        var room = new Room(201, _hotel1, Occupancy.SINGLE, 100, true, true, true);

        room.RoomNumber = 202;

        Assert.IsFalse(_hotel1.Rooms.ContainsKey(201));
        Assert.IsTrue(_hotel1.Rooms.ContainsKey(202));
        Assert.AreEqual(room, _hotel1.GetRoom(202));
    }

    
}