using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using HotelBounty.Bookings;

namespace HotelBounty.Billing;

[Serializable]
public class Bill
{
    private static List<Bill> _billList = new List<Bill>();
    private static int nextId = 1;

    public int Id { get; set; }

    public decimal TotalPrice { get; set; }

    public Booking Booking { get; set; }
    

    private const decimal TaxRate = 0.08m; // 8% for example

    public Bill()
    {
        Id = nextId++;
        AddBill(this);
    }

    public Bill(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));

        Id = nextId++;
        Booking = booking;
        GenerateBill();

        AddBill(this);
    }

    private static void AddBill(Bill bill)
    {
        if (bill == null) throw new ArgumentNullException(nameof(bill));
        _billList.Add(bill);
    }

    public void GenerateBill()
    {
        if (Booking == null) throw new InvalidOperationException("Booking must be set to generate bill");

        var nights = (Booking.CheckOutDate - Booking.CheckInDate).Days;
        if (nights <= 0) nights = 1;

        var basePrice = (decimal)Booking.Room.Price * nights;
        var tax = basePrice * TaxRate;

        TotalPrice = basePrice + tax;
    }

    public static ReadOnlyCollection<Bill> GetExtent()
    {
        return _billList.AsReadOnly();
    }

    internal static void ReplaceExtent(List<Bill> bills)
    {
        if (bills == null) throw new ArgumentNullException(nameof(bills));
        _billList = bills;
    }

    internal static void ClearExtent()
    {
        _billList.Clear();
    }

    internal static void FixIdCounter()
    {
        if (_billList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _billList.Max(b => b.Id);
            nextId = maxId + 1;
        }
    }
}