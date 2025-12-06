using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using HotelBounty.Enums;

namespace HotelBounty.Rooms;

[Serializable]
public class Deluxe : Room
{
    public List<string> MiniBarFilling { get; private set; } = new List<string>();

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
        
        MiniBarFilling = list;
        
    }

    private bool _terrace;

    public bool Terrace
    {
        get => _terrace;
        set
        {
            _terrace = value;
        }
    }

    private bool _extraBad;

    public bool ExtraBad
    {
        get => _extraBad;
        set
        {
            _extraBad = value;
        }
    }

    public Deluxe(int roomNumber, Hotel hotel, Occupancy occupancy, double price, bool climatization, bool isCleaned, bool isAvailable, bool terrace, bool extraBad)
        : base(roomNumber,hotel, occupancy, price, climatization, isCleaned, isAvailable)
    {
        Terrace = terrace;
        ExtraBad = extraBad;
    }
    
    public Deluxe() { }
}
