using Application.Weapons.Commands;
using FluentValidation;

namespace Application.Weapons.Validators;

public class CreateWeaponCommandValidator : AbstractValidator<CreateWeaponCommand>
{
    public CreateWeaponCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Manufacturer).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Caliber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Category).IsInEnum();
    }
}