using NodaTime;

namespace AsyncProjectionStressTest.Common.Gifts.Give;

public record GiftGiven(
    Guid GiftId,
    Guid UserId,
    int Points,
    string Coupon,
    bool NeedValidation,
    Instant Timestamp);