using Claims.Domain.Contracts.Exceptions;
using Claims.Services;
using Claims.WebApi.Contracts;
using FluentValidation;

namespace Claims.WebApi.Validators
{
    public class ClaimRequestValidator : AbstractValidator<CreateClaimRequest>
    {
        private readonly ICoversService _coversService;

        public ClaimRequestValidator(ICoversService coversService)
        {
            _coversService = coversService;

            RuleFor(request => request.DamageCost)
                .LessThanOrEqualTo(100_000)
                .WithMessage("DamageCost cannot exceed 100.000");

            RuleFor(request => request)
                .MustAsync(async (request, _) => await CoverExists(request))
                .WithMessage("Specified cover doesn't exist by identifier");

            RuleFor(request => request)
                .MustAsync(async (request, _) => await IsInCoverPeriod(request))
                .WithMessage("Created date must be within the period of the related Cover");
        }

        private async Task<bool> CoverExists(CreateClaimRequest request)
        {
            try
            {
                await _coversService.GetCoverAsync(request.CoverId);
                return true;
            }
            catch (CoverNotFoundException)
            {
                return false;
            }
        }


        private async Task<bool> IsInCoverPeriod(CreateClaimRequest request)
        {
            try
            {
                var cover = await _coversService.GetCoverAsync(request.CoverId);
                return request.Created >= cover.StartDate && request.Created <= cover.EndDate;
            }
            catch (CoverNotFoundException)
            {
                return true;
            }
        }
    }
}