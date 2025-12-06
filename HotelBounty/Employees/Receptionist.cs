using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace HotelBounty.Employees;

[Serializable]
public class Receptionist : Employee
{
    private string _databaseKey;

    public string DatabaseKey
    {
        get => _databaseKey;
        set
        {
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
    
    public List<string> Languages { get; private set; } = new List<string>();
    public void SetLanguages(IEnumerable<string> languages)
    {
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

        Languages = list;
    }

    public void AddLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            throw new ArgumentException("The language cannot be null, empty, or whitespace.");

        if (!Languages.Contains(language))
            Languages.Add(language);
        else
            throw new ArgumentException("The language already added.");
    }

    public void RemoveLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            throw new ArgumentException("The language cannot be null, empty, or whitespace.");

        if (Languages.Count - 1 == 0)
            throw new InvalidOperationException("An employee must have at least one language.");

        Languages.Remove(language);
    }

    
    public Receptionist(string name, string surname, decimal bonus, HotelBlock hotelBlock, Employee? supervisor, string databaseKey)
        : base(name, surname, bonus, hotelBlock, supervisor)
    {
        DatabaseKey = databaseKey;
    }

    public Receptionist() { }

}