namespace HotelBounty.Employees;

[Flags]
[Serializable]
public enum EmployeeRole
{
    None = 0,
    Cleaner = 1,
    Receptionist = 2,
    SecurityGuard = 4
}