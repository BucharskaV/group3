using System.Collections.ObjectModel;

namespace HotelBounty.Rooms;

[Serializable]
public class Deluxe : Room
{
    private List<string> _minibarFilling = new List<string>();
    public ReadOnlyCollection<string> MiniBarFilling => _minibarFilling.AsReadOnly();
    
    public void SetMiniBarFilling(IEnumerable<string> filling)
    {
        if (filling == null)
            throw new ArgumentNullException(nameof(filling));
        var list = new List<string>();
        foreach (var f in filling)
        {
            if (string.IsNullOrWhiteSpace(f))
                throw new ArgumentException("The mini bar filling cannot be null, empty, or whitespace.");
            list.Add(f);
        }
        if (list.Count == 0)
            throw new ArgumentException("At least one mini bar filling must be added.");

        _minibarFilling = list;
    }

    public string? _terrace;

    public string? Terrace
    {
        get => _terrace;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50) throw new ArgumentException("Description of terrace availability cannot be longer than 50 characters");
            }
            _terrace = value;
        }
    }

    public string? _extraBad;

    public string? ExtraBad
    {
        get => _extraBad;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if(value.Length > 50) throw new ArgumentException("Description of extra bad availability cannot be longer than 50 characters");
            }
            _extraBad = value;
        }
    }

    public Deluxe(int occupancy, double price, string? climatization, string? isCleaned, string? isAvailable, string? terrace, string? extraBad)
    : base(occupancy, price, climatization, isCleaned, isAvailable)
    {
        Terrace = terrace;
        ExtraBad = extraBad;
    }

    public Deluxe() { }
    
    
    
}