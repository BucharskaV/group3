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
        set
        {
            if (!Enum.IsDefined(typeof(PaymentMethod), value))
                throw new ArgumentException("Invalid payment method.", nameof(value));

            if (_amount > 0 && _paymentMethod != value)
                throw new InvalidOperationException("Payment method cannot be changed after payment amount is set.");

            _paymentMethod = value;
        }
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
    [XmlIgnore]
    public Bill Bill => _bill;

    public void SetBill(Bill bill, bool internalCall = false)
    {
        if (bill == null) throw new ArgumentNullException("The bill cannot be null.");
        if (_bill == bill) throw new InvalidOperationException("Bill is already set.");
        
        if (_bill != null && _bill != bill)
            throw new InvalidOperationException("Bill cannot be changed once it has been set.");
        
        
        _bill = bill;
        
        if (!internalCall)
        {
            _bill.AddPaymentOperation(this, true);
        }
    }
    
    public void UnsetBill(bool internalCall = false)
    {
        if (_bill != null)
        {
            var oldBill = _bill;
            _bill = null;
            if (!internalCall) oldBill.RemovePaymentOperation(this, true);
        }
    }
    
    private Booking _booking;
    [XmlIgnore]
    public Booking Booking => _booking;

    public void SetBooking(Booking booking, bool internalCall = false)
    {
        if (booking == null) throw new ArgumentNullException("The booking cannot be null.");
        if (_booking == booking) throw new InvalidOperationException("Booking is already set.");
        
        if (_booking != null && _booking != booking)
            throw new InvalidOperationException("Booking cannot be changed once it has been set.");
        
        _booking = booking;
        
        if (!internalCall)
        {
            _booking.AddPaymentOperation(this, true);
        }
    }
    
    public void UnsetBooking(bool internalCall = false)
    {
        if (_booking != null)
        {
            var oldBooking = _booking;
            _booking = null;
            if (!internalCall) oldBooking.RemovePaymentOperation(this, true);
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
        
        SetBooking(booking);
        SetBill(bill);
        
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