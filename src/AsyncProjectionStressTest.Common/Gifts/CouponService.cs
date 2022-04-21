using System.Text;
using RT.Comb;

namespace AsyncProjectionStressTest.Common.Gifts;

public sealed class CouponService
{
    private readonly ICombProvider _combProvider;

    public CouponService(ICombProvider combProvider)
    {
        _combProvider = combProvider;
    }

    public Guid? OpenTicketIfReady(string couponCode)
    {
        return Encoding.UTF8.GetBytes(couponCode)
            .Sum(it => it) % 2 == 0
            ? _combProvider.Create()
            : null;
    }
}