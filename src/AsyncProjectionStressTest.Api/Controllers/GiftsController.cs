using AsyncProjectionStressTest.Common.Gifts.Give;
using Microsoft.AspNetCore.Mvc;

namespace AsyncProjectionStressTest.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GiftsController : ControllerBase
{
    private readonly GiftGiveUseCase _giveUseCase;

    public GiftsController(GiftGiveUseCase giveUseCase)
    {
        _giveUseCase = giveUseCase;
    }

    [HttpPost]
    public Task<GiftGiveResponse> Give(GiftGiveRequest request, CancellationToken cancellationToken)
    {
        return _giveUseCase.Execute(request, cancellationToken);
    }
}