using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace TestsHotelBounty;

public class TestsRooms
{
    [Test]
    public void CreateStandardRoom_ValidParameters_ShouldSetProperties()
    {
        var room = new Standard(
            occupancy: Occupancy.SINGLE,
            price: 100,
            climatization: true,
            isCleaned: true,
            isAvailable: true
        );

        Assert.AreEqual(Occupancy.SINGLE, room.Occupancy);
        Assert.AreEqual(100, room.Price);
        Assert.AreEqual(true, room.Climatization);
        Assert.AreEqual(true, room.IsCleaned);
        Assert.AreEqual(true, room.IsAvailable);
    }

    [Test]
    public void CreateRoom_InvalidPrice_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new Standard(Occupancy.SINGLE, -10, true, true, true));
    }

    [Test]
    public void ChangeCleaningStatus_ShouldUpdateValue()
    {
        var room = new Standard(Occupancy.SINGLE, 80, false, false, true);

        room.IsCleaned = true;

        Assert.AreEqual(true, room.IsCleaned);
    }

    [Test]
    public void ChangeIsAvailable_ShouldUpdateValue()
    {
        var room = new Standard(Occupancy.SINGLE, 80, false, true, false);

        room.IsAvailable = true;

        Assert.AreEqual(true, room.IsAvailable);
    }
    
    
     [Test]
    public void DeluxeRoom_SetAndGetPropertiesCorrectly()
    {
        var deluxe = new Deluxe(
            occupancy: Occupancy.SINGLE,
            price: 250,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            terrace: true,
            extraBad: false
        );

        Assert.That(deluxe.Terrace, Is.EqualTo(true));
        Assert.That(deluxe.ExtraBad, Is.EqualTo(false));
    }
    
    [Test]
    public void DeluxeRoom_SetMiniBarFillingCorrectly()
    {
        var deluxe = new Deluxe();

        deluxe.SetMiniBarFilling(new List<string> { "Water", "Soda" });

        CollectionAssert.AreEqual(
            new List<string> { "Water", "Soda" },
            deluxe.MiniBarFilling
        );
    }

    [Test]
    public void DeluxeRoom_SetMiniBar_EmptyList_ThrowsException()
    {
        var deluxe = new Deluxe();

        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.SetMiniBarFilling(new List<string>());
        });
    }

    [Test]
    public void DeluxeRoom_SetMiniBar_NullItem_ThrowsException()
    {
        var deluxe = new Deluxe();

        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.SetMiniBarFilling(new List<string> { "Water", null });
        });
    }

    [Test]
    public void DeluxeRoom_SetMiniBar_WhitespaceItem_ThrowsException()
    {
        var deluxe = new Deluxe();

        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.SetMiniBarFilling(new List<string> { "   " });
        });
    }
    
    [Test]
    public void PetFriendly_SetAndGetPropertiesCorrectly()
    {
        var room = new PetFriendly(
            occupancy: Occupancy.SINGLE,
            price: 120,
            climatization: true,
            isCleaned: true,
            isAvailable: true,
            petFeeders: "Automatic feeder",
            maxPetsAllowed: 2
        );

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
        Assert.Throws<ArgumentException>(() =>
        {
            var room = new PetFriendly(Occupancy.SINGLE, 100, true, true, true, "", 1);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new PetFriendly(Occupancy.SINGLE, 100, true, true, true, null, 1);
        });
    }

    [Test]
    public void PetFriendly_SettingMaxPetsAboveLimit_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var room = new PetFriendly(Occupancy.SINGLE, 100, true, true, true, "Feeder", 4);
        });
    }

    [Test]
    public void PetFriendly_MaxPetsAllowed_StaticValueChanges()
    {
        var room = new PetFriendly(Occupancy.SINGLE, 100, true, true, true, "Feeder", 2);
        room.MaxPetsAllowed = 3; 

        Assert.That(room.MaxPetsAllowed, Is.EqualTo(3));
    }
    
    [Test]
    public void NoPets_SetAndGetPropertiesCorrectly()
    {
        var room = new NoPets(
            occupancy: Occupancy.SINGLE,
            price: 150,
            climatization: true,
            isCleaned: true,
            isAvailsble: true,
            allergyFriendly: true
        );

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
        Assert.Throws<ArgumentException>(() =>
            new NoPets(Occupancy.SINGLE, -50, true, true, true, true)
        );
    }
    
}
