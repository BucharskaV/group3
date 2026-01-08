using HotelBounty;
using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace TestsHotelBounty;

public class TestsRooms
{
    [Test]
    public void CreateStandardRoom_ValidParameters_ShouldSetProperties()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var room = new Room(
            roomNumber: 201,
            type: RoomType.Standard,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 100,
            climatization: true,
            isCleaned: true,
            isAvailable: true
        );

        
        Assert.That(room.RoomNumber, Is.EqualTo(201));
        Assert.That(room.Hotel, Is.EqualTo(hotel));
        Assert.That(room.Occupancy, Is.EqualTo(Occupancy.SINGLE));
        Assert.That(room.Price, Is.EqualTo(100));
        Assert.That(room.Climatization, Is.True);
        Assert.That(room.IsCleaned, Is.True);
        Assert.That(room.IsAvailable, Is.True);
    }

    [Test]
    public void CreateRoom_InvalidPrice_ShouldThrow()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        Assert.Throws<ArgumentException>(() =>
            new Room(201, RoomType.Standard,hotel, Occupancy.SINGLE, -10, true, true, true));
    }

    [Test]
    public void ChangeCleaningStatus_ShouldUpdateValue()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var room = new Room( 201, RoomType.Standard,hotel, Occupancy.SINGLE, 80, false, false, true);

        room.IsCleaned = true;

        Assert.AreEqual(true, room.IsCleaned);
    }

    [Test]
    public void ChangeIsAvailable_ShouldUpdateValue()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var room = new Room( 201, RoomType.Standard,hotel, Occupancy.SINGLE, 80, false, false, true);

        room.IsAvailable = true;

        Assert.AreEqual(true, room.IsAvailable);
    }
    
    
     [Test]
    public void DeluxeRoom_SetAndGetPropertiesCorrectly()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var deluxe = new Room(
            roomNumber: 201,
            type: RoomType.Deluxe,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 250,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            terrace: true,
            extraBed: false
        );

        Assert.That(deluxe.Terrace, Is.EqualTo(true));
        Assert.That(deluxe.ExtraBed, Is.EqualTo(false));
    }
    
    [Test]
    public void DeluxeRoom_SetMiniBarFillingCorrectly()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var deluxe = new Room(
            roomNumber: 201,
            type: RoomType.Deluxe,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 250,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            terrace: true,
            extraBed: false
        );

    
        deluxe.SetMiniBarFilling(new List<string> { "Water", "Soda" });
    
        CollectionAssert.AreEqual(
            new List<string> { "Water", "Soda" },
            deluxe.MiniBarFilling
        );
    }

    [Test]
    public void DeluxeRoom_SetMiniBar_EmptyList_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var deluxe = new Room(
            roomNumber: 201,
            type: RoomType.Deluxe,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 250,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            terrace: true,
            extraBed: false
        );


        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.SetMiniBarFilling(new List<string>());
        });
    }

    [Test]
    public void DeluxeRoom_SetMiniBar_NullItem_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var deluxe = new Room(
            roomNumber: 201,
            type: RoomType.Deluxe,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 250,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            terrace: true,
            extraBed: false
        );


        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.SetMiniBarFilling(new List<string> { "Water", null });
        });
    }

    [Test]
    public void DeluxeRoom_SetMiniBar_WhitespaceItem_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var deluxe = new Room(
            roomNumber: 201,
            type: RoomType.Deluxe,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 250,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            terrace: true,
            extraBed: false
        );


        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.SetMiniBarFilling(new List<string> { "   " });
        });
    }
    
    [Test]
    public void PetFriendly_SetAndGetPropertiesCorrectly()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var room = new Room(
            roomNumber: 501,
            type: RoomType.PetFriendly,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 120,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            petFeeders: "Automatic feeder",
            maxPetsAllowed: 2
        );

        Assert.That(room.RoomNumber, Is.EqualTo(501));
        Assert.That(room.Hotel, Is.EqualTo(hotel));
        Assert.That(room.Occupancy, Is.EqualTo(Occupancy.SINGLE));
        Assert.That(room.Price, Is.EqualTo(120));
        Assert.That(room.Climatization, Is.EqualTo(true));
        Assert.That(room.IsCleaned, Is.EqualTo(true));
        Assert.That(room.IsAvailable, Is.EqualTo(true));
        Assert.That(room.PetFeeders, Is.EqualTo("Automatic feeder"));
        Assert.That(room.MaxPetsAllowed, Is.EqualTo(2));
    }

    [Test]
    public void PetFriendly_EmptyPetFeeders_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        Assert.Throws<ArgumentException>(() =>
        {
            var room = new Room(201, RoomType.PetFriendly ,hotel, Occupancy.SINGLE, 100, true, true, true, petFeeders: "", maxPetsAllowed:1);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new Room(202, RoomType.PetFriendly,hotel, Occupancy.SINGLE, 100, true, true, true, petFeeders: null, maxPetsAllowed: 1);
        });
    }

    [Test]
    public void PetFriendly_SettingMaxPetsAboveLimit_ThrowsException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new Room(201, RoomType.PetFriendly,hotel, Occupancy.SINGLE, 100, true, true, true, petFeeders: "Feeder", maxPetsAllowed: 4);
        });
    }

    [Test]
    public void PetFriendly_MaxPetsAllowed_InstanceValueChanges()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);
        var room = new Room(201, RoomType.PetFriendly, hotel, Occupancy.SINGLE, 100, true, true, true, 
            petFeeders: "Feeder", maxPetsAllowed: 2);
    
        room.MaxPetsAllowed = 3;
        Assert.That(room.MaxPetsAllowed, Is.EqualTo(3));
    }
    
    [Test]
    public void NoPets_SetAndGetPropertiesCorrectly()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        var room = new Room(
            roomNumber: 401,
            type: RoomType.NoPets,
            hotel: hotel,
            occupancy: Occupancy.SINGLE,
            price: 150,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            allergyFriendly: true
        );

        Assert.That(room.RoomNumber, Is.EqualTo(401));
        Assert.That(room.Hotel, Is.EqualTo(hotel));
        Assert.That(room.Occupancy, Is.EqualTo(Occupancy.SINGLE));
        Assert.That(room.Price, Is.EqualTo(150));
        Assert.That(room.Climatization, Is.EqualTo(true));
        Assert.That(room.IsCleaned, Is.EqualTo(true));
        Assert.That(room.IsAvailable, Is.EqualTo(true));
        Assert.That(room.AllergyFriendly, Is.EqualTo(true));
    }
    
    [Test]
    public void NoPetsRoom_SetNegativePrice_ShouldThrowException()
    {
        var hotel = new Hotel("Hotel Bounty", "Warsaw", "799039000", 5);

        Assert.Throws<ArgumentException>(() =>
           new Room(401, RoomType.NoPets, hotel, Occupancy.DOUBLE, -120, false, false, true,
                allergyFriendly: true)
        );
    }
    
}
