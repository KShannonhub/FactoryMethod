/*
Yes, the GetHistory method in the HistoryFactory class is an example of a Factory Method.

The Factory Method is a creational design pattern that provides an interface for creating objects, but allows subclasses to alter the type of objects that will be created. In this case, the GetHistory method creates and returns an instance of a class (either TruckHistory or DealerShipHistory) based on the HistoryType enum value passed to it.

The GetReturnHistory method is also a kind of factory method, as it uses the GetHistory factory method to create an object and then invokes the GetHistory method on that object to return a Dictionary<string, object>.
*/

// Define an interface 'IHistory'
public interface IHistory
{
    Dictionary<string, object> GetHistory();
}

// Implement 'IHistory' interface in 'Car' class
public class DealerShipHistory : IHistory
{
    public Dictionary<string, object> GetHistory()
    {
        // Use reflection to get the history of the class model
        var history = new DealerShip();
        return new PropertyHelper().GetPropertiesDictionary(history);
    }
}

public class PropertyHelper() {
    public Dictionary<string, object> GetPropertiesDictionary(object obj)
    {
        var type = obj.GetType();
        var properties = type.GetProperties();

        var propertiesDictionary = new Dictionary<string, object>();

        foreach (var property in properties)
        {
            if (Attribute.IsDefined(property, typeof(ReflectAttribute)))
            {
                propertiesDictionary.Add(property.Name, property.GetValue(obj));
            }
        }

        return propertiesDictionary;
    }
}
// Implement 'IHistory' interface in 'Truck' class
public class TruckHistory : IHistory
{
    public Dictionary<string, object> GetHistory()
    {
        // Use reflection to get the history of the class model
        var history = new Truck();
        // This will hydrate from a bl layer with a DAL call to the database

        return new PropertyHelper().GetPropertiesDictionary(history);
    }
}


// Define the attribute
[AttributeUsage(AttributeTargets.Property)]
public class ReflectAttribute : Attribute
{
}

public class Truck : Attribute
{
    [Reflect]
    public string VehicleType { get; set; }
    [Reflect]
    public DateTime Date { get; set; }
    public string Action { get; set; }
}

public class DealerShip : Attribute
{
    [Reflect]
    public string DealerName { get; set; }
    [Reflect]
    public DateTime Date { get; set; }
    public string Action { get; set; }
}

// Define an enum 'VehicleType'
public enum HistoryType
{
    Truck,
    DealerShip
}

// Create a factory class 'VehicleFactory' to create instances of 'IHistory'
public class HistoryFactory
{
    public static IHistory GetHistory(HistoryType historyType)
    {
        // Use a switch statement to create the appropriate vehicle based on the provided enum value
        switch (historyType)
        {
            case HistoryType.Truck:
                return new TruckHistory();
            case HistoryType.DealerShip:
                return new DealerShipHistory();
                default: throw new ArgumentException("Invalid history type");
        }
    }

    public static Dictionary<string, object> GetReturnHistory(HistoryType historyType)
    {
        // Get the history and return it
        return GetHistory(historyType).GetHistory();
    }
}

public class Client
{
    static void Main(string[] args)
    {
        HistoryFactory factory = new();

        // Use the factory to get the histories
        var dealerShipHistory = HistoryFactory.GetReturnHistory(HistoryType.DealerShip);
        var truckHistory = HistoryFactory.GetReturnHistory(HistoryType.Truck);
        Console.WriteLine(dealerShipHistory);
        Console.WriteLine(truckHistory);
    }
}

