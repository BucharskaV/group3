using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using HotelBounty.Enums;

namespace HotelBounty.Employees;

[Serializable]
public class Employee
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

    private EmployeeRole _roles;

    public EmployeeRole Roles
    {
        get => _roles;
        set => _roles = value;
    }

    private bool HasRole(EmployeeRole role) => Roles.HasFlag(role);
    
    //cleaner attributes
    private Specialization? _specialization;

    [XmlIgnore]
    public Specialization? Specialization
    {
        get => _specialization;
        set
        {
            EnsureRole(EmployeeRole.Cleaner);
            _specialization = value;
        }
    }
    
    //receptionist attributes
    private string? _databaseKey;

    [XmlIgnore]
    public string? DatabaseKey
    {
        get => _databaseKey;
        set
        {
            EnsureRole(EmployeeRole.Receptionist);
            
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("The key of the database cannot be empty");
            if(value.Length > 20)
                throw new ArgumentException("The key of the database cannot be longer than 20 characters");
            if(value.Length < 5) 
                throw new ArgumentException("The key of the database cannot be shorter than 5 characters");
            if(!Regex.IsMatch(value, @"^[A-Za-z0-9]+$"))
                throw new ArgumentException("The key of the database should only contain alphanumeric characters");
            _databaseKey = value;
        }
    }
    
    private List<string>? _languages;
    [XmlIgnore]
    public IReadOnlyCollection<string> Languages
    {
        get => _languages?.AsReadOnly();
    }
    public void SetLanguages(IEnumerable<string> languages)
    {
        EnsureRole(EmployeeRole.Receptionist);
        
        if (languages == null)
            throw new ArgumentNullException(nameof(languages));

        var list = new List<string>();

        foreach (var language in languages)
        {
            if (string.IsNullOrWhiteSpace(language))
                throw new ArgumentException("The language cannot be null, empty, or whitespace.");
            list.Add(language);
        }

        if (list.Count == 0)
            throw new ArgumentException("At least one language must be added.");

        _languages = list;
    }

    public void AddLanguage(string language)
    {
        
        EnsureRole(EmployeeRole.Receptionist);
        if (string.IsNullOrWhiteSpace(language))
            throw new ArgumentException("The language cannot be null, empty, or whitespace.");

        if (!Languages.Contains(language))
            _languages.Add(language);
        else
            throw new ArgumentException("The language already added.");
    }

    public void RemoveLanguage(string language)
    {
        EnsureRole(EmployeeRole.Receptionist);
        
        if (string.IsNullOrWhiteSpace(language))
            throw new ArgumentException("The language cannot be null, empty, or whitespace.");

        if (Languages.Count - 1 == 0)
            throw new InvalidOperationException("An employee must have at least one language.");

        _languages.Remove(language);
    }
    
    //security guard attribute
    private string? _securityCode;

    [XmlIgnore]
    public string? SecurityCode
    {
        get =>_securityCode;
        set
        {
            EnsureRole(EmployeeRole.SecurityGuard);
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException("The security code cannot be empty");
            if(value.Length > 20)
                throw new ArgumentException("The security code cannot be longer than 20 characters");
            if(value.Length < 5) 
                throw new ArgumentException("The security code cannot be shorter than 5 characters");
            if(!Regex.IsMatch(value, @"^[A-Za-z0-9]+$"))
                throw new ArgumentException("The security code only contains alphanumeric characters");
            _securityCode = value;
        }
    }
    
    private string? _accessToWeapons;

    [XmlIgnore]
    public string? AccessToWeapons
    {
        get =>_accessToWeapons;
        set
        {
            EnsureRole(EmployeeRole.SecurityGuard);
            
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                    throw new ArgumentException("Description of access to weapons cannot be longer than 50 characters");
            }
            _accessToWeapons = value; 
        }
    }

    public void AddRole(EmployeeRole role)
    {
        if(HasRole(role))
            throw new InvalidOperationException("This role already assigned to the employee");
        
        Roles |= role;
        
        if(role == EmployeeRole.Cleaner)
            _specialization = Enums.Specialization.DEFAULT;

        if (role == EmployeeRole.Receptionist)
        {
            _databaseKey = string.Empty;
            _languages = new List<string>();
        }

        if (role == EmployeeRole.SecurityGuard)
        {
            _securityCode = string.Empty;
            _accessToWeapons = string.Empty;
        }
    }
    
    public void RemoveRole(EmployeeRole role)
    {
        if(!HasRole(role))
            throw new InvalidOperationException("This role is not assigned to the employee");
        
        Roles &= ~role;
        
        if(role == EmployeeRole.Cleaner)
            _specialization = null;

        if (role == EmployeeRole.Receptionist)
        {
            _databaseKey = null;
            _languages = null;
        }

        if (role == EmployeeRole.SecurityGuard)
        {
            _securityCode = null;
            _accessToWeapons = null;
        }
    }

    private void EnsureRole(EmployeeRole role)
    {
        if (!HasRole(role))
        {
            throw new InvalidOperationException($"Operation is not permitted for this employee, employee is not a {role}");
        }
    }

    public Employee()
    {
        Id = nextId++;
        AddEmployee(this);
    }

    public Employee(string name, string surname, decimal bonus, HotelBlock hotelBlock, EmployeeRole rolesToAdd, Employee? supervisor = null)
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

        Roles = EmployeeRole.None;

        if (rolesToAdd == EmployeeRole.None)
        {
            throw new ArgumentException("Employee must have at least one role");
        }

        foreach (EmployeeRole  role in Enum.GetValues(typeof(EmployeeRole)))
        {
            if (role != EmployeeRole.None && rolesToAdd.HasFlag(role))
            {
                AddRole(role);
            }
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