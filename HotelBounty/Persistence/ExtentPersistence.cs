using System.Xml;
using System.Xml.Serialization;
using HotelBounty.Billing;
using HotelBounty.Bookings;
using HotelBounty.ComplexAttributes;
using HotelBounty.Employees;
using HotelBounty.Enums;
using HotelBounty.Rooms;

namespace HotelBounty.Persistence;

public class ExtentPersistence
{
    public static void Save(string path = "hotels.xml")
    {
        var container = new ExtentContainer()
        {
            Employees = new List<Employee>(Employee.GetExtent()),
            Rooms = new List<Room>(Room.GetExtent()),
            HotelBlocks = new List<HotelBlock>(HotelBlock.GetExtent()),
            Addresses = new List<Address>(Address.GetExtent()),
            Hotels = new List<Hotel>(Hotel.GetExtent()),
            Bookings = new List<Booking>(Booking.GetExtent()),
            Guests = new List<Guest>(Guest.GetExtent()),
            Bills = new List<Bill>(Bill.GetExtent()),
            PaymentOperations = new List<PaymentOperation>(PaymentOperation.GetExtent()),
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
                ClearAllExtents();
                return false;
            }

            ClearAllExtents();

            var serializer = new XmlSerializer(typeof(ExtentContainer));
            
            using (var reader = new XmlTextReader(file))
            {
                ExtentContainer container;
                try
                {
                    container = (ExtentContainer)serializer.Deserialize(reader);
                }
                catch (InvalidCastException)
                {
                    ClearAllExtents();
                    return false;
                }
                catch (Exception)
                {
                    ClearAllExtents();
                    return false;
                }

                if (container == null)
                {
                    ClearAllExtents();
                    return false;
                }

                if (container.Employees != null)
                    Employee.ReplaceExtent(container.Employees);

                if (container.Rooms != null)
                    Room.ReplaceExtent(container.Rooms);

                if (container.HotelBlocks != null)
                    HotelBlock.ReplaceExtent(container.HotelBlocks);

                if (container.Addresses != null)
                    Address.ReplaceExtent(container.Addresses);

                if (container.Hotels != null)
                    Hotel.ReplaceExtent(container.Hotels);
                
                if (container.Guests != null)
                    Guest.ReplaceExtent(container.Guests);
                
                if (container.Bookings != null)
                    Booking.ReplaceExtent(container.Bookings);
                
                if (container.Bills != null)
                    Bill.ReplaceExtent(container.Bills);
                
                if (container.PaymentOperations != null)
                    PaymentOperation.ReplaceExtent(container.PaymentOperations);
                
                Employee.FixIdCounter();
                Room.FixIdCounter();
                HotelBlock.FixIdCounter();
                Address.FixIdCounter();
                Hotel.FixIdCounter();
                Guest.FixIdCounter();
                Booking.FixIdCounter();
                Bill.FixIdCounter();
                PaymentOperation.FixIdCounter();
            }

            return true;
        }
        finally
        {
            file?.Dispose();
        }
    }
    
    private static void ClearAllExtents()
    {
        Employee.ClearExtent();
        Room.ClearExtent();
        HotelBlock.ClearExtent();
        Address.ClearExtent();
        Hotel.ClearExtent();
        Guest.ClearExtent();
        Booking.ClearExtent();
        Bill.ClearExtent();
        PaymentOperation.ClearExtent();
    }
    
}