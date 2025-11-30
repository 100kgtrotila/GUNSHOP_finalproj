using Domain.Weapons;

namespace Tests.Data.Weapon;

public static class WeaponData
{
    public static Domain.Weapons.Weapon FirstWeapon() => 
        Domain.Weapons.Weapon.New("Glock 17", "Glock", "17 Gen 5", "9mm", "GLK-001", 599.99m, WeaponCategory.Pistol);

    public static Domain.Weapons.Weapon SecondWeapon() => 
        Domain.Weapons.Weapon.New("Beretta 92", "Beretta", "92FS", "9mm", "BRT-002", 649.99m, WeaponCategory.Pistol);
    
    public static Domain.Weapons.Weapon ThirdWeapon() => 
        Domain.Weapons.Weapon.New("AR-15", "Smith & Wesson", "M&P15", "5.56x45mm", "SW-AR15-001", 1899.99m, WeaponCategory.Rifle);

    public static Domain.Weapons.Weapon OutOfStockWeapon()
    {
        var weapon = Domain.Weapons.Weapon.New("AK-47", "Kalashnikov", "AKM", "7.62x39mm", "AK-003", 899.99m, WeaponCategory.Rifle);
        weapon.ChangeStatus(WeaponStatus.Sold);
        return weapon;
    }
}