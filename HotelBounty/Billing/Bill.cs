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

    private decimal _totalPrice;
    public decimal TotalPrice
    {
        get => _totalPrice;
        set
        {
            if (value < 0) throw new ArgumentException("Total price cannot be negative.");
            _totalPrice = value;
        }
    }
    
    
    private HashSet<PaymentOperation> _paymentOperations = new HashSet<PaymentOperation>();
    
    [XmlIgnore]
    public IReadOnlyCollection<PaymentOperation> PaymentOperations => _paymentOperations.ToList().AsReadOnly();

    public void AddPaymentOperation(PaymentOperation operation, bool internalCall = false)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (_paymentOperations.Contains(operation)) throw new InvalidOperationException("Payment operation already in bill.");

        _paymentOperations.Add(operation);

        if (!internalCall)
        {
            operation.SetBill(this, true);
        }
    }
    
    public void RemovePaymentOperation(PaymentOperation operation, bool internalCall = false)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        
        if (!_paymentOperations.Contains(operation))
            throw new InvalidOperationException("Payment operation not found."); // Исключение

        _paymentOperations.Remove(operation);
            
        if (!internalCall)
        {
            operation.UnsetBill(true);
        }
    }
    
    
    
  

    private const decimal TaxRate = 0.08m;

    public Bill()
    {
        Id = nextId++;
        AddBill(this);
    }

    public Bill(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));

        Id = nextId++;
        
        var nights = (booking.CheckOutDate - booking.CheckInDate).Days;
        if (nights <= 0) nights = 1;
        var basePrice = (decimal)booking.Room.Price * nights;
        TotalPrice = basePrice + basePrice * TaxRate;

        AddBill(this);
    }
    
    public Bill(decimal price)
    {
        Id = nextId++;
        TotalPrice = price;
        AddBill(this);
    }

    private static void AddBill(Bill bill)
    {
        if (bill == null) throw new ArgumentNullException(nameof(bill));
        _billList.Add(bill);
    }
    

    public static ReadOnlyCollection<Bill> GetExtent() => _billList.AsReadOnly();
    internal static void ReplaceExtent(List<Bill> bills) => _billList = bills ?? throw new ArgumentNullException(nameof(bills));
    public static void ClearExtent() => _billList.Clear();
    internal static void FixIdCounter()
    {
        if (_billList.Count == 0) nextId = 1;
        else nextId = _billList.Max(b => b.Id) + 1;
    }
}