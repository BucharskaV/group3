using System.Xml.Serialization;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;
using HotelBounty.Rooms;

namespace HotelBounty.Persistence;

[Serializable]
[XmlRoot("ExtentContainer")]
public class ExtentContainer
{
    [XmlArray("Employees")]
    [XmlArrayItem("Employee")]
    public List<Employee> Employees { get; set; } = new List<Employee>();
    
    [XmlArray("Rooms")]
    [XmlArrayItem("Room")]
    public List<Room> Rooms { get; set; } = new List<Room>();
    
    [XmlArray("HotelBlocks")]
    [XmlArrayItem("HotelBlock")]
    public List<HotelBlock> HotelBlocks { get; set; } = new List<HotelBlock>();
    
    [XmlArray("Addresses")]
    [XmlArrayItem("Address")]
    public List<Address> Addresses { get; set; } = new List<Address>();
    
    [XmlArray("Hotels")]
    [XmlArrayItem("Hotel")]
    public List<Hotel> Hotels { get; set; } = new List<Hotel>();
    
    [XmlArray("Bookings")]
    [XmlArrayItem("Booking")]
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    
    [XmlArray("Guests")]
    [XmlArrayItem("Guest")]
    public List<Guest> Guests { get; set; } = new List<Guest>();
    
    [XmlArray("Bills")]
    [XmlArrayItem("Bill")]
    public List<Bill> Bills { get; set; } = new List<Bill>();
    
    [XmlArray("PaymentOperations")]
    [XmlArrayItem("PaymentOperation")]
    public List<PaymentOperation> PaymentOperations { get; set; } = new List<PaymentOperation>();
}