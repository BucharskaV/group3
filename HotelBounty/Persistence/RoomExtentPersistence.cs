using System.Xml;
using System.Xml.Serialization;
using HotelBounty.Rooms;

namespace HotelBounty.Persistence;

public static class RoomExtentPersistence
{
    public static void Save(string path = "hotels.xml")
    {
        var container = new RoomExtentContainer()
        {
            Rooms = new List<Room>(Room.GetExtent())
        };

        try
        {
            var file = File.CreateText(path);
            var serializer = new XmlSerializer(typeof(RoomExtentContainer));

            using var writer = new XmlTextWriter(file)
            {
                Formatting = Formatting.Indented
            };

            serializer.Serialize(writer, container);
        }
        catch
        {
            throw new Exception("Failed to save room extent");
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
                Room.ClearExtent();
                return false;
            }

            Room.ClearExtent();

            var serializer = new XmlSerializer(typeof(RoomExtentContainer));

            using var reader = new XmlTextReader(file);
            RoomExtentContainer container;

            try
            {
                container = (RoomExtentContainer)serializer.Deserialize(reader);
            }
            catch
            {
                Room.ClearExtent();
                return false;
            }

            if (container?.Rooms != null)
                Room.ReplaceExtent(container.Rooms);
            else
                Room.ClearExtent();

            return true;
        }
        finally
        {
            file?.Dispose();
        }
    }
}
