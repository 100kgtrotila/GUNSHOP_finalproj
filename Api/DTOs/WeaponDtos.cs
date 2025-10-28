using Domain.Weapons;

namespace Api.DTOs;

public record CreateWeaponRequest(
    string Name,
    string Manufacturer,
    string Model,
    string Caliber,
    string SerialNumber,
    decimal Price,
    WeaponCategory Category
);

public record UpdateWeaponRequest(
    string Name,
    string Manufacturer,
    string Model,
    string Caliber,
    decimal Price
);

public record ChangeWeaponStatusRequest(
    WeaponStatus Status
);

public record WeaponResponse(
    Guid Id,
    string Name,
    string Manufacturer,
    string Model,
    string Caliber,
    string SerialNumber,
    decimal Price,
    WeaponStatus Status,
    WeaponCategory Category,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);