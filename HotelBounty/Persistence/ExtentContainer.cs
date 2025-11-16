using System.Xml.Serialization;
using HotelBounty.Employees;

namespace HotelBounty.Persistence;

[Serializable]
[XmlRoot("ExtentContainer")]
public class ExtentContainer
{
    [XmlArray("Employees")]
    [XmlArrayItem("Employee")]
    public List<Employee> Employees { get; set; } = new List<Employee>();
}