using System.Net;
using System.Net.Http.Json;
using Domain.Weapons;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Api.DTOs;
using Tests.Data.Weapon;
using Xunit;

namespace Api.Tests.Integration.Weapon;

public class WeaponControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/weapons";
    
    private readonly Domain.Weapons.Weapon _firstWeapon;
    private readonly Domain.Weapons.Weapon _secondWeapon;
    private readonly Domain.Weapons.Weapon _outOfStockWeapon;

    public WeaponControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _firstWeapon = WeaponData.FirstWeapon();
        _secondWeapon = WeaponData.SecondWeapon();
        _outOfStockWeapon = WeaponData.OutOfStockWeapon();
    }

    public async Task InitializeAsync()
    {
        await Context.Weapons.AddRangeAsync(_firstWeapon, _secondWeapon, _outOfStockWeapon);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.Weapons.RemoveRange(Context.Weapons);
        await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldGetAllWeapons()
    {
        // Act
        var response = await Client.GetAsync(BaseRoute);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var weapons = await response.ToResponseModel<List<WeaponResponse>>();
        weapons.Should().NotBeNull();
        weapons.Should().HaveCount(3);
    }

    [Fact]
    public async Task ShouldGetWeaponById()
    {
        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstWeapon.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var weapon = await response.ToResponseModel<WeaponResponse>();
        weapon.Should().NotBeNull();
        weapon!.Id.Should().Be(_firstWeapon.Id);
        weapon.Name.Should().Be(_firstWeapon.Name);
    }

    [Fact]
    public async Task ShouldCreateWeapon()
    {
        // Arrange
        var newWeapon = WeaponData.ThirdWeapon();
    
        var request = new CreateWeaponRequest(
            newWeapon.Name,
            newWeapon.Manufacturer,
            newWeapon.Model,
            newWeapon.Caliber,
            newWeapon.SerialNumber,
            newWeapon.Price,
            newWeapon.Category
        );

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdWeapon = await response.ToResponseModel<WeaponResponse>();
    
        createdWeapon.Should().NotBeNull();
        createdWeapon!.Name.Should().Be(request.Name);
        createdWeapon.Manufacturer.Should().Be(request.Manufacturer);
        createdWeapon.Model.Should().Be(request.Model);
        createdWeapon.Caliber.Should().Be(request.Caliber);
        createdWeapon.SerialNumber.Should().Be(request.SerialNumber);
        createdWeapon.Price.Should().Be(request.Price);
        createdWeapon.Category.Should().Be(request.Category);
        createdWeapon.Status.Should().Be(WeaponStatus.InStock);
    }


    [Fact]
    public async Task ShouldUpdateWeapon()
    {
        // Arrange
        var request = new UpdateWeaponRequest(
            "M4A1 Updated",
            "Colt Updated",
            "M4A1 Carbine Updated",
            "5.56x45mm NATO",
            1299.99m
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstWeapon.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedWeapon = await Context.Weapons
            .AsNoTracking()
            .FirstAsync(w => w.Id == _firstWeapon.Id);
    
        updatedWeapon.Name.Should().Be("M4A1 Updated");
        updatedWeapon.Manufacturer.Should().Be("Colt Updated");
    }

    [Fact]
    public async Task ShouldChangeWeaponStatus()
    {
        // Arrange
        var request = new ChangeWeaponStatusRequest(WeaponStatus.Reserved);

        // Act
        var response = await Client.PatchAsync(
            $"{BaseRoute}/{_firstWeapon.Id}/status",
            JsonContent.Create(request));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedWeapon = await Context.Weapons
            .AsNoTracking()
            .FirstAsync(w => w.Id == _firstWeapon.Id);
    
        updatedWeapon.Status.Should().Be(WeaponStatus.Reserved);
    }
    
    [Fact]
    public async Task ShouldDeleteWeapon()
    {
        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_secondWeapon.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var deletedWeapon = await Context.Weapons
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == _secondWeapon.Id);
    
        deletedWeapon.Should().BeNull();
    }

}
