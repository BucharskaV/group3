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
            climatization: "AC",
            isCleaned: "Yes",
            isAvailable: "Yes"
        );

        Assert.AreEqual(Occupancy.SINGLE, room.Occupancy);
        Assert.AreEqual(100, room.Price);
        Assert.AreEqual("AC", room.Climatization);
        Assert.AreEqual("Yes", room.IsCleaned);
        Assert.AreEqual("Yes", room.IsAvailable);
    }

    [Test]
    public void CreateRoom_InvalidPrice_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() =>
            new Standard(Occupancy.SINGLE, -10, "AC", "Yes", "Yes"));
    }

    [Test]
    public void ChangeCleaningStatus_ShouldUpdateValue()
    {
        var room = new Standard(Occupancy.SINGLE, 80, "Fan", "No", "Yes");

        room.IsCleaned = "Yes";

        Assert.AreEqual("Yes", room.IsCleaned);
    }

    [Test]
    public void ChangeIsAvailable_ShouldUpdateValue()
    {
        var room = new Standard(Occupancy.SINGLE, 80, "Fan", "Yes", "No");

        room.IsAvailable = "Yes";

        Assert.AreEqual("Yes", room.IsAvailable);
    }
    
    
     [Test]
    public void DeluxeRoom_SetAndGetPropertiesCorrectly()
    {
        var deluxe = new Deluxe(
            occupancy: Occupancy.SINGLE,
            price: 250,
            climatization: "AC",
            isCleaned: "Yes",
            isAvailable: "Yes",
            terrace: "Medium terrace",
            extraBad: "Extra bed"
        );

        Assert.That(deluxe.Terrace, Is.EqualTo("Medium terrace"));
        Assert.That(deluxe.ExtraBad, Is.EqualTo("Extra bed"));
    }

    [Test]
    public void DeluxeRoom_TerraceTooLong_ThrowsException()
    {
        var deluxe = new Deluxe();

        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.Terrace = new string('A', 51);
        });
    }

    [Test]
    public void DeluxeRoom_ExtraBedTooLong_ThrowsException()
    {
        var deluxe = new Deluxe();

        Assert.Throws<ArgumentException>(() =>
        {
            deluxe.ExtraBad = new string('B', 55);
        });
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
            climatization: "AC",
            isCleaned: "Yes",
            isAvailable: "Yes",
            petFeeders: "Automatic feeder",
            maxPetsAllowed: 2
        );

        Assert.That(room.Occupancy, Is.EqualTo(Occupancy.SINGLE));
        Assert.That(room.Price, Is.EqualTo(120));
        Assert.That(room.Climatization, Is.EqualTo("AC"));
        Assert.That(room.IsCleaned, Is.EqualTo("Yes"));
        Assert.That(room.IsAvailable, Is.EqualTo("Yes"));
        Assert.That(room.PetFeeders, Is.EqualTo("Automatic feeder"));
        Assert.That(room.MaxPetsAllowed, Is.EqualTo(2));
    }

    [Test]
    public void PetFriendly_EmptyPetFeeders_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var room = new PetFriendly(Occupancy.SINGLE, 100, "AC", "Yes", "Yes", "", 1);
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new PetFriendly(Occupancy.SINGLE, 100, "AC", "Yes", "Yes", null, 1);
        });
    }

    [Test]
    public void PetFriendly_SettingMaxPetsAboveLimit_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var room = new PetFriendly(Occupancy.SINGLE, 100, "AC", "Yes", "Yes", "Feeder", 4);
        });
    }

    [Test]
    public void PetFriendly_MaxPetsAllowed_StaticValueChanges()
    {
        var room = new PetFriendly(Occupancy.SINGLE, 100, "AC", "Yes", "Yes", "Feeder", 2);
        room.MaxPetsAllowed = 3; 

        Assert.That(room.MaxPetsAllowed, Is.EqualTo(3));
    }
    
    [Test]
    public void NoPets_SetAndGetPropertiesCorrectly()
    {
        var room = new NoPets(
            occupancy: Occupancy.SINGLE,
            price: 150,
            climatization: "AC",
            isCleaned: "Yes",
            isAvailsble: "Yes",
            allergyFriendly: "Hypoallergenic bedding"
        );

        Assert.That(room.Occupancy, Is.EqualTo(Occupancy.SINGLE));
        Assert.That(room.Price, Is.EqualTo(150));
        Assert.That(room.Climatization, Is.EqualTo("AC"));
        Assert.That(room.IsCleaned, Is.EqualTo("Yes"));
        Assert.That(room.IsAvailable, Is.EqualTo("Yes"));
        Assert.That(room.AllergyFriendly, Is.EqualTo("Hypoallergenic bedding"));
    }

    [Test]
    public void NoPets_AllergyFriendly_AllowsNullOrEmpty()
    {
        var room1 = new NoPets(Occupancy.SINGLE, 80, "Fan", "Yes", "Yes", null);
        var room2 = new NoPets(Occupancy.SINGLE, 80, "Fan", "Yes", "Yes", "");

        Assert.That(room1.AllergyFriendly, Is.Null);
        Assert.That(room2.AllergyFriendly, Is.EqualTo(""));
    }

    [Test]
    public void NoPets_AllergyFriendly_TooLong_ThrowsException()
    {
        string longText = new string('A', 51);

        Assert.Throws<ArgumentException>(() =>
        {
            var room = new NoPets(Occupancy.SINGLE, 100, "AC", "Yes", "Yes", longText);
        });
    }

    [Test]
    public void NoPets_AllergyFriendly_Exactly50Characters_IsAllowed()
    {
        string exactly50 = new string('X', 50);

        var room = new NoPets(Occupancy.SINGLE, 120, "Heat", "Yes", "Yes", exactly50);

        Assert.That(room.AllergyFriendly, Is.EqualTo(exactly50));
    }
}
