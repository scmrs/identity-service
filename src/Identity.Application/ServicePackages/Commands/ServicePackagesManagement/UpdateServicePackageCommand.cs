﻿using FluentValidation;

namespace Identity.Application.ServicePackages.Commands.ServicePackagesManagement
{
    public record UpdateServicePackageCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int DurationDays,
        string AssociatedRole,
        string Status
    ) : ICommand<ServicePackageDto>;

    public class UpdateServicePackageValidator : AbstractValidator<UpdateServicePackageCommand>
    {
        public UpdateServicePackageValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.DurationDays).GreaterThan(0);
            RuleFor(x => x.Status).NotEmpty();
        }
    }
}