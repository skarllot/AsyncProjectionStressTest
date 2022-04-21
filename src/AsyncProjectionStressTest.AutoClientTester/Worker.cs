using System.Net.Http.Json;
using AsyncProjectionStressTest.Common.Gifts.Give;

namespace AsyncProjectionStressTest.AutoClientTester;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = new HttpClient();
        var random = Random.Shared;

        while (!stoppingToken.IsCancellationRequested)
        {
            var requestList = new List<GiftGiveRequest>(1000);
            while (requestList.Count < requestList.Capacity)
            {
                var request = new GiftGiveRequest(
                    Guid.NewGuid(),
                    random.Next(1, 1000),
                    Guid.NewGuid().ToString("N"),
                    random.Next(0, 1000) % 2 == 0,
                    DateTimeOffset.Now);

                requestList.Add(request);
            }

            try
            {
                foreach (var request in requestList)
                {
                    await client.PostAsJsonAsync("http://api/Gifts", request, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to post");
            }
        }
    }
}