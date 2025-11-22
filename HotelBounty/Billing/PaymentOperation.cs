using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using HotelBounty.Bookings;
using HotelBounty.Enums;

namespace HotelBounty.Billing;

[Serializable]
public class PaymentOperation
{
    private static List<PaymentOperation> _paymentList = new List<PaymentOperation>();
    private static int nextId = 1;
    public int Id { get; set; }
    
    private PaymentMethod _paymentMethod;

    public PaymentMethod PaymentMethod
    {
        get => _paymentMethod;
        set => _paymentMethod = value;
    }

    private DateTime _paymentDate;
    public DateTime PaymentDate {
        get => _paymentDate;
        set => _paymentDate = value;
    }

    private decimal _amount;

    public decimal Amount
    {
        get => _amount;
        set
        {
            if(value < 0) throw new ArgumentOutOfRangeException("The payment amount cannot be less than zero.");
            _amount = value;
        }
    }

    private Bill _bill;
    public Bill Bill
    {
        get => _bill;
        set
        {
            if(value == null) throw new ArgumentNullException("The bill cannot be null.");
            _bill = value;
        }
    }
    
    private Booking _booking;
    public Booking Booking
    {
        get => _booking;
        set
        {
            if(value == null) throw new ArgumentNullException("The booking cannot be null.");
            _booking = value;
        }
    }

    public PaymentOperation()
    {
        Id = nextId++;
        AddPayment(this);
    }

    public PaymentOperation(Bill bill, Booking booking, PaymentMethod method, decimal amount)
    {
        Id = nextId++;
        Booking = booking;
        Bill = bill;
        PaymentMethod = method;
        Amount = amount;
        PaymentDate = DateTime.Now;

        AddPayment(this);
    }

    private static void AddPayment(PaymentOperation payment)
    {
        if (payment == null) throw new ArgumentNullException(nameof(payment));
        _paymentList.Add(payment);
    }


    public void Prepay(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Amount = amount;
        PaymentDate = DateTime.Now;
    }


    public void Pay()
    {
        if (Bill == null) throw new InvalidOperationException("Bill must be set to pay");
        Amount = Bill.TotalPrice;
        PaymentDate = DateTime.Now;
    }

    public static ReadOnlyCollection<PaymentOperation> GetExtent()
    {
        return _paymentList.AsReadOnly();
    }

    internal static void ReplaceExtent(List<PaymentOperation> payments)
    {
        if (payments == null) throw new ArgumentNullException(nameof(payments));
        _paymentList = payments;
    }

    public static void ClearExtent()
    {
        _paymentList.Clear();
    }

    internal static void FixIdCounter()
    {
        if (_paymentList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _paymentList.Max(p => p.Id);
            nextId = maxId + 1;
        }
    }
}