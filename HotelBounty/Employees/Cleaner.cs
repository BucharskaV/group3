using HotelBounty.Enums;

namespace HotelBounty.Employees;

[Serializable]
public class Cleaner : Employee
{
    private Specialization _specialization;

    public Specialization Specialization
    {
        get => _specialization;
        set
        {
            _specialization = value;
        }
    }

    public Cleaner(string name, string surname, decimal bonus, Specialization specialization)
        : base(name, surname, bonus)
    {
        Specialization = specialization;
    }

    public Cleaner() { }
}