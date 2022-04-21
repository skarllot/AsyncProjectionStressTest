namespace AsyncProjectionStressTest.Common.Gifts.ValidateCoupon;

public sealed record GiftCouponTicketOpened(
    Guid GiftId,
    Guid CouponTicket);