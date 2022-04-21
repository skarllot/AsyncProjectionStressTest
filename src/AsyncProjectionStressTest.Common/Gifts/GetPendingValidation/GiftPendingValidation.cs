using AsyncProjectionStressTest.Common.Gifts.Give;
using AsyncProjectionStressTest.Common.Gifts.ValidateCoupon;

namespace AsyncProjectionStressTest.Common.Gifts.GetPendingValidation;

public class GiftPendingValidation
{
    public Guid GiftId { get; set; }
    public int Points { get; set; }
    public string Coupon { get; set; } = string.Empty;
    public Guid? CouponValidationTicket { get; private set; }
    public bool IsCouponTicketOpen { get; private set; }

    public void Apply(GiftGiven @event)
    {
        GiftId = @event.GiftId;
        Points = @event.Points;
        Coupon = @event.Coupon;
    }

    public void Apply(GiftCouponTicketOpened @event)
    {
        CouponValidationTicket = @event.CouponTicket;
        IsCouponTicketOpen = true;
    }

    public bool ShouldDelete(GiftGiven @event) => !@event.NeedValidation;
}