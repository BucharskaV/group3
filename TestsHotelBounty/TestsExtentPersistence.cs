using NUnit.Framework;
using HotelBounty.Bookings;
using HotelBounty.Rooms;
using HotelBounty.Enums;
using HotelBounty;
using HotelBounty.ComplexAttributes;
using HotelBounty.Billing;
using HotelBounty.Persistence; 
using System;
using System.Linq;
using System.IO;

namespace TestsHotelBounty
{
    [TestFixture]
    public class TestsExtentPersistence
    {
        [SetUp]
        public void Setup()
        {
            Booking.ClearExtent();
            Guest.ClearExtent();
            Room.ClearExtent();
            Hotel.ClearExtent();
            Address.ClearExtent();
            Bill.ClearExtent();
        }

        [Test]
        public void SaveAndLoad_ShouldPreserveDataCount()
        {
            var address = new Address("City", "District", "Street", 1);
            var hotel = new Hotel("Hotel Bounty", "Warsaw", "123456789", 5);
            var room = new Standard(101, hotel, Occupancy.DOUBLE, 100, true, true, true);
            var guest = new Guest("Test Guest", DateTime.Now.AddYears(-25), address, "12345678901", "1234567890");
            
            var booking = new Booking(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), guest, room);
            var bill = new Bill(booking);
            
            Assert.AreEqual(1, Booking.GetExtent().Count);
            Assert.AreEqual(1, Guest.GetExtent().Count);
            
            string testFileName = "test_extent.xml";
            ExtentPersistence.Save(testFileName);

            
            Booking.ClearExtent();
            Guest.ClearExtent();
            Room.ClearExtent();
            Hotel.ClearExtent();
            Bill.ClearExtent();

            Assert.AreEqual(0, Booking.GetExtent().Count); 
            
            bool loaded = ExtentPersistence.Load(testFileName);
            
            Assert.IsTrue(loaded, "Extent should load successfully");
            
            Assert.AreEqual(1, Hotel.GetExtent().Count, "Hotel count mismatch");
            Assert.AreEqual(1, Room.GetExtent().Count, "Room count mismatch");
            Assert.AreEqual(1, Guest.GetExtent().Count, "Guest count mismatch");
            Assert.AreEqual(1, Booking.GetExtent().Count, "Booking count mismatch");
            Assert.AreEqual(1, Bill.GetExtent().Count, "Bill count mismatch");
            
            var loadedBooking = Booking.GetExtent().First();
            Assert.IsNotNull(loadedBooking);
            
            if (File.Exists(testFileName)) File.Delete(testFileName);
        }
    }
}