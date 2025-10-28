using Application.Weapons.Commands;
using FluentValidation;

namespace Application.Weapons.Validators;

public class UpdateWeaponCommandValidator : AbstractValidator<UpdateWeaponCommand>
{
    public UpdateWeaponCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Caliber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}