using AsyncProjectionStressTest.Common.Core;
using NodaTime.Extensions;
using RT.Comb;

namespace AsyncProjectionStressTest.Common.Gifts.Give;

public class GiftGiveUseCase
{
    private readonly IRepository<Gift> _repository;
    private readonly ICombProvider _combProvider;
    private readonly CouponService _couponService;

    public GiftGiveUseCase(IRepository<Gift> repository, ICombProvider combProvider, CouponService couponService)
    {
        _repository = repository;
        _combProvider = combProvider;
        _couponService = couponService;
    }

    public async Task<GiftGiveResponse> Execute(GiftGiveRequest request, CancellationToken cancellationToken)
    {
        var gift = Gift.Give(
            _combProvider.Create(request.Timestamp.UtcDateTime),
            request.UserId,
            request.Points,
            request.Coupon,
            request.NeedValidation,
            request.Timestamp.ToInstant());

        await _repository.Store(gift, cancellationToken);

        if (gift.NeedValidation)
        {
            var openTicketResult = _couponService.OpenTicketIfReady(gift.Coupon);
            gift.ValidateCoupon(openTicketResult);
        }

        await _repository.Store(gift, cancellationToken);

        return new GiftGiveResponse(gift.Id, gift.CouponValidationTicket);
    }
}