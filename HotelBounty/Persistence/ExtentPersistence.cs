using System.Xml;
using System.Xml.Serialization;
using HotelBounty.Employees;
using HotelBounty.Rooms;

namespace HotelBounty.Persistence;

public class ExtentPersistence
{
    public static void Save(string path = "hotels.xml")
    {
        var container = new ExtentContainer()
        {
            Employees = new List<Employee>(Employee.GetExtent()),
            Rooms = new List<Room>(Room.GetExtent())
        };

        try
        {
            var file = File.CreateText(path);
            var serializer = new XmlSerializer(typeof(ExtentContainer));
            using (var writer = new XmlTextWriter(file))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, container);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Serialization failed: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    public static bool Load(string path = "hotels.xml")
    {
        StreamReader file = null;

        try
        {
            try
            {
                file = File.OpenText(path);
            }
            catch (FileNotFoundException)
            {
                Employee.ClearExtent();
                Room.ClearExtent();
                return false;
            }

            Employee.ClearExtent();
            Room.ClearExtent();

            var serializer = new XmlSerializer(typeof(ExtentContainer));
            
            using (var reader = new XmlTextReader(file))
            {
                ExtentContainer container;
                try
                {
                    container = (ExtentContainer)serializer.Deserialize(reader);
                    
                    Employee.FixIdCounter(); 
                    Room.FixIdCounter();
                }
                catch (InvalidCastException)
                {
                    Employee.ClearExtent();
                    Room.ClearExtent();
                    return false;
                }
                catch (Exception)
                {
                    Employee.ClearExtent();
                    Room.ClearExtent();
                    return false;
                }

                if (container == null)
                {
                    Employee.ClearExtent();
                    Room.ClearExtent();
                    return false;
                }

                if (container.Employees != null) 
                    Employee.ReplaceExtent(container.Employees);
                if (container.Rooms != null) 
                    Room.ReplaceExtent(container.Rooms);
            }

            return true;
        }
        finally
        {
            file?.Dispose();
        }
    }
}