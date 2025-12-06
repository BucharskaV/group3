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
    public int Id { get; set; }

    private string _name;
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
    private string _surname;
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

    private decimal _bonus;
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
    
    private HotelBlock _hotelBlock;
    public HotelBlock HotelBlock => _hotelBlock;

    internal void AssignHotelBlock(HotelBlock hotelBlock)
    {
        if(hotelBlock == null) throw new ArgumentNullException("The hotel block cannot be null.");
        if(_hotelBlock != null && _hotelBlock != hotelBlock) _hotelBlock.RemoveEmployee(this);
        
        _hotelBlock = hotelBlock;
    }

    internal void UnassignHotelBlock()
    {
        _hotelBlock = null;
    }

    public void ChangeHotelBlock(HotelBlock newHotelBlock)
    {
        if(newHotelBlock == null) throw new ArgumentNullException("The hotel block cannot be null.");
        
        if(_hotelBlock == newHotelBlock)
            throw new InvalidOperationException("Employee already works in this block.");
        
        if(_hotelBlock != null) _hotelBlock.RemoveEmployee(this);
        
        newHotelBlock.AddEmployee(this);
    }

    private Employee? _supervisor;
    private readonly HashSet<Employee> _supervisees = new HashSet<Employee>();
    public IReadOnlyCollection<Employee> Supervisees => _supervisees;
    public Employee? Supervisor => _supervisor;

    public void SetSupervisor(Employee? newSupervisor)
    {
        if (newSupervisor == null) throw new ArgumentNullException("The supervisor cannot be null.");
        if(newSupervisor == this)
            throw new InvalidOperationException("The employee cannot supervise itself.");
        
        if(_supervisor == newSupervisor)
            throw new InvalidOperationException("The new supervisor is the same as the current.");

        if (_supervisor != null)
            _supervisor._supervisees.Remove(this);
        
        _supervisor = newSupervisor;

        if(!newSupervisor._supervisees.Add(this))
            throw new InvalidOperationException("The supervisor already has this supervisee.");
    }

    public void RemoveSupervisor()
    {
        if(_supervisor == null) throw new InvalidOperationException("The supervisor is not set.");
        var oldSupervisor = _supervisor;
        _supervisor = null;
        oldSupervisor._supervisees.Remove(this);
    }

    public void AddSupervisee(Employee e)
    {
        if(e == null) throw new ArgumentNullException("The employee cannot be null.");
        if(e == this) throw new InvalidOperationException("The employees cannot supervise themselves.");
        if(_supervisees.Contains(e)) throw new InvalidOperationException("The employee is already a supervisee.");
        _supervisees.Add(e);
        e.SetSupervisor(this);
    }

    public void RemoveSupervisee(Employee e)
    {
        if(e == null) throw new ArgumentNullException("The employee cannot be null.");
        if(!_supervisees.Contains(e))
            throw new InvalidOperationException("Not supervised by this employee.");
        _supervisees.Remove(e);
        if(e._supervisor == this)
            e._supervisor = null;
    }

    public Employee()
    {
        Id = nextId++;
        AddEmployee(this);
    }

    public Employee(string name, string surname, decimal bonus, HotelBlock hotelBlock, Employee? supervisor = null)
    {
        Id = nextId++;
        Name = name;
        Surname = surname;
        Bonus = bonus;
        
        hotelBlock.AddEmployee(this);
        if (supervisor != null)
        {
            SetSupervisor(supervisor);
        }

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

    public static void ClearExtent()
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