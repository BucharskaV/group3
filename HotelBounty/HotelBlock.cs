using HotelBounty.Employees;

namespace HotelBounty;

[Serializable]
public class HotelBlock
{
    private List<Employee> _employees = new List<Employee>();

    private List<Employee> Employees
    {
        get { return _employees; }
        set { _employees = value; }
    }
}