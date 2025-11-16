using System.Xml;
using System.Xml.Serialization;
using HotelBounty.Employees;

namespace HotelBounty.Persistence;

public class ExtentPersistence
{
    public static void Save(string path = "hotels.xml")
    {
        var container = new ExtentContainer()
        {
            Employees = new List<Employee>(Employee.GetExtent())
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
        catch (Exception)
        {
            throw new Exception("Failed to save extent");
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
                return false;
            }

            Employee.ClearExtent();

            var serializer = new XmlSerializer(typeof(ExtentContainer));
            using (var reader = new XmlTextReader(file))
            {
                ExtentContainer container;
                try
                {
                    container = (ExtentContainer)serializer.Deserialize(reader);
                    Employee.FixIdCounter(); 
                }
                catch (InvalidCastException)
                {
                    Employee.ClearExtent();
                    return false;
                }
                catch (Exception)
                {
                    Employee.ClearExtent();
                    return false;
                }

                if (container == null)
                {
                    Employee.ClearExtent();
                    return false;
                }

                if (container.Employees != null)
                {
                    Employee.ReplaceExtent(container.Employees);
                }
                else
                {
                    Employee.ClearExtent();
                }
            }

            return true;
        }
        finally
        {
            file?.Dispose();
        }
    }
}