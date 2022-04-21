namespace AsyncProjectionStressTest.Common.Gifts.Give;

public record GiftGiveRequest(
    Guid UserId,
    int Points,
    string Coupon,
    bool NeedValidation,
    DateTimeOffset Timestamp);