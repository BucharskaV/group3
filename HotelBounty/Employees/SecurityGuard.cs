using System.Text.RegularExpressions;

namespace HotelBounty.Employees;

[Serializable]
public class SecurityGuard : Employee
{
    private string _securityCode;

    public string SecurityCode
    {
        get => _securityCode;
        set
        {
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

    public string? AccessToWeapons
    {
        get => _accessToWeapons;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                    throw new ArgumentException("Description of access to weapons cannot be longer than 50 characters");
            }
            _accessToWeapons = value; 
        }
    }

    public SecurityGuard(string name, string surname, decimal bonus, HotelBlock hotelBlock, Employee? supervisor, string securityCode, string? accessToWeapons)
        : base(name, surname, bonus, hotelBlock, supervisor)
    {
        SecurityCode = securityCode;
        AccessToWeapons = accessToWeapons;
    }

    public SecurityGuard() { }

}