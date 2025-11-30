namespace Domain.Weapons;

public class Weapon
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Manufacturer { get; private set; }
    public string Model { get; private set; }
    public string Caliber { get; private set; }
    public string SerialNumber { get; private set; }
    public decimal Price { get; private set; }
    public WeaponStatus Status { get; private set; }
    public WeaponCategory Category { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    private readonly List<ProductComment.ProductComment> _comments = new();
    public IReadOnlyCollection<ProductComment.ProductComment> Comments => _comments.AsReadOnly();

    private Weapon(
        Guid id,
        string name,
        string manufacturer,
        string model,
        string caliber,
        string serialNumber,
        decimal price,
        WeaponStatus status,
        WeaponCategory category,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        Manufacturer = manufacturer;
        Model = model;
        Caliber = caliber;
        SerialNumber = serialNumber;
        Price = price;
        Status = status;
        Category = category;
        CreatedAt = createdAt;
    }

    public static Weapon New(
        string name,
        string manufacturer,
        string model,
        string caliber,
        string serialNumber,
        decimal price,
        WeaponCategory category)
    {
        return new Weapon(
            Guid.NewGuid(),
            name,
            manufacturer,
            model,
            caliber,
            serialNumber,
            price,
            WeaponStatus.InStock,
            category,
            DateTime.UtcNow
        );
    }

    public void UpdateDetails(string name, string manufacturer, string model, string caliber, decimal price)
    {
        Name = name;
        Manufacturer = manufacturer;
        Model = model;
        Caliber = caliber;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(WeaponStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}
