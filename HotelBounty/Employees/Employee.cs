using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace HotelBounty.Employees;

[Serializable]
[XmlInclude(typeof(Cleaner))]
[XmlInclude(typeof(Receptionist))]
[XmlInclude(typeof(SecurityGuard))]
public abstract class Employee
{
    private static List<Employee> _employeesList = new List<Employee>();

    private static int nextId = 1;

    private static decimal _minSalary = 1000;

    public static decimal MinSalary
    {
        get => _minSalary;
        set
        {
            if(value < 0) throw new ArgumentOutOfRangeException("The minimum salary cannot be less than zero.");
            _minSalary = value;
        }
    }
    public int Id { get; private set; }

    public string _name;
    public string Name
    {
        get => _name;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }
    public string _surname;
    public string Surname
    {
        get => _surname;
        set
        {
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("Surname cannot be empty");
            _surname = value;
        }
    }

    public decimal _bonus;

    public decimal Bonus
    {
        get => _bonus;
        set
        {
            if(value < 0) throw new ArgumentOutOfRangeException("The bonus cannot be less than zero.");
            _bonus = value;
        }
    }

    public decimal Salary
    {
        get
        {
            return MinSalary + Bonus;
        }
    }
    
    private Employee? _supervisor;
    public Employee? Supervisor
    {
        get => _supervisor;
        set
        {
            if (value == this)
                throw new ArgumentException("An employee cannot be their own supervisor.");
            _supervisor = value;
        }
    }
    
    private HotelBlock _hotelBlock;

    public HotelBlock HotelBlock
    {
        get => _hotelBlock;
        set
        {
            if(value == null) throw new ArgumentNullException("The hotel block cannot be null.");
            _hotelBlock = value;
        }
    }
    
    public Employee() {}

    public Employee(string name, string surname, decimal bonus, Employee? supervisor)
    {
        Id = nextId++;
        Name = name;
        Surname = surname;
        Bonus = bonus;
        Supervisor = supervisor;

        AddEmployee(this);
    }

    public static ReadOnlyCollection<Employee> GetExtent()
    {
        return _employeesList.AsReadOnly();
    }

    private static void AddEmployee(Employee e)
    {
        if(e == null) throw new ArgumentException("Employee cannot be null");
        _employeesList.Add(e);
    }

    internal static void ReplaceExtent(List<Employee> employees)
    {
        if (employees == null) throw new ArgumentNullException(nameof(employees));
        _employeesList = employees;
    }

    internal static void ClearExtent()
    {
        _employeesList.Clear();
    }
    
    internal static void FixIdCounter()
    {
        if (_employeesList.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            var maxId = _employeesList.Max(g => g.Id);
            nextId = maxId + 1;
        }
    }
 
}