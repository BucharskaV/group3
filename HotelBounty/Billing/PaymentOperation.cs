using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using HotelBounty.Enums;

namespace HotelBounty.Billing;

[Serializable]
public class PaymentOperation
{
    private static List<PaymentOperation> _paymentList = new List<PaymentOperation>();
    private static int nextId = 1;

    public int Id { get; set; }

    public PaymentMethod Method { get; set; }

    public DateTime PaymentDate { get; set; }

    public decimal Amount { get; set; }

    public Bill Bill { get; set; }

    public PaymentOperation()
    {
        Id = nextId++;
        AddPayment(this);
    }

    public PaymentOperation(Bill bill, PaymentMethod method, decimal amount)
    {
        if (bill == null) throw new ArgumentNullException(nameof(bill));
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

        Id = nextId++;
        Bill = bill;
        Method = method;
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

    internal static void ClearExtent()
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