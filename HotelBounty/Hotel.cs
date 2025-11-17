using HotelBounty.Rooms;

namespace HotelBounty;

public class Hotel
{
    private List<HotelBlock> _blocks = new List<HotelBlock>();
    
    private List<HotelBlock> HotelBlocks
    {
        get { return _blocks; }
        set { _blocks = value; }
    }
    
    private List<Room> _rooms = new List<Room>();

    private List<Room> Rooms
    {
        get { return _rooms; }
        set { _rooms = value; }
    }
}