using System.Xml.Serialization;
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
}