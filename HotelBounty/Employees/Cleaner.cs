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

    public Cleaner(string name, string surname, decimal bonus, HotelBlock hotelBlock, Employee? supervisor, Specialization specialization)
        : base(name, surname, bonus, hotelBlock, supervisor)
    {
        Specialization = specialization;
    }

    public Cleaner() { }
}