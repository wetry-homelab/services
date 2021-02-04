using Infrastructure.Contracts.Request;
using FluentValidation;

namespace Application.Validators
{
    public class ClusterUpdateRequestValidator : AbstractValidator<ClusterUpdateRequest>
    {
        public ClusterUpdateRequestValidator()
        {
            RuleFor(r => r.Node)
                .GreaterThan(2)
                .WithMessage("#MIN_NODE#")
                .LessThan(8)
                .WithMessage("#MAX_NODE#");

            RuleFor(r => r.Cpu)
                .GreaterThan(1)
                .WithMessage("#MIN_CPU#")
                .LessThan(8)
                .WithMessage("#MAX_CPU#");

            RuleFor(r => r.Memory)
                .GreaterThan(512)
                .WithMessage("#MIN_MEMORY#")
                .LessThan(16384)
                .WithMessage("#MAX_MEMORY#");

            RuleFor(r => r.Storage)
                .LessThan(10)
                .WithMessage("#MIN_STORAGE#")
                .GreaterThan(200)
                .WithMessage("#MAX_STORAGE#");
        }
    }

    public class ClusterCreateRequestValidator : AbstractValidator<ClusterCreateRequest>
    {
        public ClusterCreateRequestValidator()
        {
            RuleFor(r => r.Name)
                    .NotEmpty()
                    .WithMessage("#FIELD_REQUIED#")
                    .NotNull()
                    .WithMessage("#FIELD_REQUIED#")
                    .MinimumLength(4)
                    .WithMessage("#FIELD_MIN_LENGTH#")
                    .MaximumLength(128)
                    .WithMessage("#FIELD_MAX_LENGTH#");

            RuleFor(r => r.Node)
                .GreaterThan(2)
                .WithMessage("#MIN_NODE#")
                .LessThan(8)
                .WithMessage("#MAX_NODE#");

            RuleFor(r => r.Cpu)
                .GreaterThan(1)
                .WithMessage("#MIN_CPU#")
                .LessThan(8)
                .WithMessage("#MAX_CPU#");

            RuleFor(r => r.Memory)
                .GreaterThan(512)
                .WithMessage("#MIN_MEMORY#")
                .LessThan(16384)
                .WithMessage("#MAX_MEMORY#");

            RuleFor(r => r.Storage)
                .GreaterThan(10)
                .WithMessage("#MIN_STORAGE#")
                .LessThan(200)
                .WithMessage("#MAX_STORAGE#");

            RuleFor(r => r.SshKey)
                .NotEmpty()
                .WithMessage("#FIELD_REQUIED#")
                .NotNull()
                .WithMessage("#FIELD_REQUIED#");

            RuleFor(r => r.Ip)
                .NotEmpty()
                .WithMessage("#FIELD_REQUIED#")
                .NotNull()
                .WithMessage("#FIELD_REQUIED#");

            RuleFor(r => r.Gateway)
                .NotEmpty()
                .WithMessage("#FIELD_REQUIED#")
                .NotNull()
                .WithMessage("#FIELD_REQUIED#");
        }
    }
}
