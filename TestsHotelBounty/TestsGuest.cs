using System;
using HotelBounty.ComplexAttributes;
using NUnit.Framework;

namespace TestsHotelBounty;

public class TestsGuest
{
    [Test]
    public void Guest_SetAndGetPropertiesCorrectly()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dateOfBirth = new DateTime(1990, 1, 1);
        var guest = new Guest("Anna", dateOfBirth, address, "12345678901", "1234567890");

        Assert.That(guest.Name, Is.EqualTo("Anna"));
        Assert.That(guest.DateOfBirth, Is.EqualTo(dateOfBirth));
        Assert.That(guest.Address, Is.EqualTo(address));
        Assert.That(guest.Pesel, Is.EqualTo("12345678901"));
        Assert.That(guest.GuestCardNumber, Is.EqualTo("1234567890"));
    }

    [Test]
    public void Guest_EmptyOrWhitespaceName_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dateOfBirth = DateTime.Today.AddYears(-20);

        Assert.Throws<ArgumentException>(() =>
        {
            var g1 = new Guest("", dateOfBirth, address, "12345678901", "1234567890");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var g2 = new Guest("   ", dateOfBirth, address, "12345678901", "1234567890");
        });
    }

    [Test]
    public void Guest_NameTooLong_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dateOfBirth = DateTime.Today.AddYears(-25);
        string longName = new string('A', 51);

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest(longName, dateOfBirth, address, "12345678901", "1234567890");
        });
    }

    [Test]
    public void Guest_FutureDateOfBirth_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var futureDate = DateTime.Today.AddDays(1);

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest("Anna", futureDate, address, "12345678901", "1234567890");
        });
    }

    [Test]
    public void Guest_UnderageDateOfBirth_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var underageDob = DateTime.Today.AddYears(-17);

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest("Anna", underageDob, address, "12345678901", "1234567890");
        });
    }

    [Test]
    public void Guest_NullAddress_ThrowsException()
    {
        var dateOfBirth = DateTime.Today.AddYears(-25);

        Assert.Throws<ArgumentNullException>(() =>
        {
            var guest = new Guest("Anna", dateOfBirth, null, "12345678901", "1234567890");
        });
    }

    [Test]
    public void Guest_EmptyOrNullPesel_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dateOfBirth = DateTime.Today.AddYears(-25);

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest("Anna", dateOfBirth, address, "", "1234567890");
        });

        var validGuest = new Guest("Anna", dateOfBirth, address, "12345678901", "1234567890");
        Assert.Throws<ArgumentException>(() =>
        {
            validGuest.Pesel = null;
        });
    }

    [Test]
    public void Guest_InvalidPeselLength_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dateOfBirth = DateTime.Today.AddYears(-25);

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest("Anna", dateOfBirth, address, "123", "1234567890");
        });

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest("Anna", dateOfBirth, address, "1234567890", "1234567890"); // 10 chars
        });
    }

    [Test]
    public void Guest_EmptyOrNullGuestCardNumber_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dob = DateTime.Today.AddYears(-25);

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest("Anna", dob, address, "12345678901", "");
        });

        var g2 = new Guest("Anna", dob, address, "12345678901", "1234567890");
        Assert.Throws<ArgumentException>(() =>
        {
            g2.GuestCardNumber = null;
        });
    }

    [Test]
    public void Guest_InvalidGuestCardNumber_ThrowsException()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dob = DateTime.Today.AddYears(-25);

        Assert.Throws<ArgumentException>(() =>
        {
            var guest = new Guest("Anna", dob, address, "12345678901", "12345"); // too short
        });

        var g2 = new Guest("Anna", dob, address, "12345678901", "1234567890");
        Assert.Throws<ArgumentException>(() =>
        {
            g2.GuestCardNumber = "abcdefghij"; // not digits
        });
    }

    [Test]
    public void Guest_AgeProperty_CalculatesCorrectly()
    {
        var address = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var dob = new DateTime(2000, 1, 1);
        var guest = new Guest("Anna", dob, address, "12345678901", "1234567890");

        var today = DateTime.Today;
        var expectedAge = today.Year - dob.Year;
        if (dob.Date > today.AddYears(-expectedAge))
            expectedAge--;

        Assert.That(guest.Age, Is.EqualTo(expectedAge));
    }

    [Test]
    public void Guest_EditGuestInfo_UpdatesPropertiesCorrectly()
    {
        var address1 = new Address("Warsaw", "Wola", "Kaspszaka", 55);
        var address2 = new Address("Krakow", "Center", "MainStreet", 10);
        var dob1 = new DateTime(1990, 1, 1);
        var dob2 = new DateTime(1985, 5, 5);

        var guest = new Guest("Anna", dob1, address1, "12345678901", "1234567890");

        guest.EditGuestInfo("Maria", dob2, address2, "98765432109");

        Assert.That(guest.Name, Is.EqualTo("Maria"));
        Assert.That(guest.DateOfBirth, Is.EqualTo(dob2));
        Assert.That(guest.Address, Is.EqualTo(address2));
        Assert.That(guest.Pesel, Is.EqualTo("98765432109"));
    }
}