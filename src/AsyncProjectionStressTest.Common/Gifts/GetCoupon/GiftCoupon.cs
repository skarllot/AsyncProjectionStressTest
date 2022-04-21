using AsyncProjectionStressTest.Common.Gifts.Give;

namespace AsyncProjectionStressTest.Common.Gifts.GetCoupon;

public class GiftCoupon
{
    public Guid GiftId { get; set; }
    public string Coupon { get; set; } = string.Empty;

    public static GiftCoupon Create(GiftGiven @event)
    {
        return new GiftCoupon
        {
            GiftId = @event.GiftId,
            Coupon = @event.Coupon
        };
    }
}