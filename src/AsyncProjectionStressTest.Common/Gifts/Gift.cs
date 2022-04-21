using AsyncProjectionStressTest.Common.Core;
using AsyncProjectionStressTest.Common.Gifts.Give;
using AsyncProjectionStressTest.Common.Gifts.ValidateCoupon;
using NodaTime;

namespace AsyncProjectionStressTest.Common.Gifts;

public class Gift : Aggregate
{
    public Gift(GiftGiven @event)
        : base(@event.GiftId)
    {
        UserId = @event.UserId;
        Points = @event.Points;
        Coupon = @event.Coupon;
        NeedValidation = @event.NeedValidation;
        ReceivedTimestamp = @event.Timestamp;
        OnEventApplied(@event);
    }

    public Guid UserId { get; set; }
    public int Points { get; set; }
    public string Coupon { get; set; }
    public bool NeedValidation { get; set; }
    public Instant ReceivedTimestamp { get; set; }

    public Guid? CouponValidationTicket { get; private set; }
    public bool IsCouponTicketOpen { get; private set; }

    public static Gift Give(Guid id, Guid userId, int points, string coupon, bool needValidation, Instant timestamp)
    {
        return new Gift(new GiftGiven(id, userId, points, coupon, needValidation, timestamp));
    }

    public void ValidateCoupon(Guid? openTicketResult)
    {
        if (openTicketResult is not null)
            Apply(new GiftCouponTicketOpened(Id, openTicketResult.Value));
    }

    private void Apply(GiftCouponTicketOpened @event)
    {
        CouponValidationTicket = @event.CouponTicket;
        IsCouponTicketOpen = true;
        OnEventApplied(@event);
    }
}