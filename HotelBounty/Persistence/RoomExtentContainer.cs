using System.Xml.Serialization;
using HotelBounty.Rooms;

namespace HotelBounty.Persistence;

[Serializable]
[XmlRoot("RoomExtentContainer")]
public class RoomExtentContainer
{
    [XmlArray("Rooms")]
    [XmlArrayItem("Room")]
    public List<Room> Rooms { get; set; } = new List<Room>();
}