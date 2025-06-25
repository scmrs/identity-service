﻿using FluentValidation;

namespace Identity.Application.ServicePackages.Commands.ServicePackagesManagement
{
    public record CreateServicePackageCommand(
        string Name,
        string Description,
        decimal Price,
        int DurationDays,
        string AssociatedRole,
        string Status = "active"
    ) : ICommand<ServicePackageDto>;

    public class CreateServicePackageValidator : AbstractValidator<CreateServicePackageCommand>
    {
        public CreateServicePackageValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.DurationDays).GreaterThan(0);
        }
    }
}