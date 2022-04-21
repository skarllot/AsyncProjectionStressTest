using AsyncProjectionStressTest.Common.Core;
using AsyncProjectionStressTest.Common.Gifts;
using AsyncProjectionStressTest.Common.Gifts.GetCoupon;
using AsyncProjectionStressTest.Common.Gifts.GetPendingValidation;
using AsyncProjectionStressTest.Common.Gifts.Give;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Marten.NodaTime;
using Marten.Schema;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RT.Comb;
using Weasel.Core;
using Weasel.Postgresql;

namespace AsyncProjectionStressTest.Common;

public static class CommonDependecyInjection
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services, bool enableProjectionDaemon)
    {
        services.AddMarten(provider => ConfigureMarten(provider, enableProjectionDaemon));

        return services
            .AddCore()
            .AddUseCases();
    }

    private static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services
            .AddSingleton(typeof(IRepository<>), typeof(MartenRepository<>))
            .AddSingleton(Provider.PostgreSql);
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddSingleton<GiftGiveUseCase>()
            .AddSingleton<CouponService>();
    }

    private static StoreOptions ConfigureMarten(IServiceProvider serviceProvider, bool enableProjectionDaemon)
    {
        var martenDbOptions = serviceProvider.GetRequiredService<IOptions<MartenDbOptions>>().Value;
        var options = new StoreOptions();

        options.Connection(martenDbOptions.ConnectionString ?? throw new InvalidOperationException());
        options.UseDefaultSerialization(EnumStorage.AsString, Casing.CamelCase);
        options.UseNodaTime();

        options.AutoCreateSchemaObjects = AutoCreate.All;
        options.DatabaseSchemaName = "documents";
        options.Events.DatabaseSchemaName = "stream";
        options.Events.MetadataConfig.CausationIdEnabled = true;
        options.Events.MetadataConfig.CorrelationIdEnabled = true;

        if (enableProjectionDaemon)
            options.Projections.AsyncMode = DaemonMode.HotCold;

        options.Schema.For<GiftCoupon>()
            .Identity(it => it.GiftId)
            .UniqueIndex(UniqueIndexType.Computed, it => it.Coupon)
            .DatabaseSchemaName("projection");

        options.Schema.For<GiftPendingValidation>()
            .Identity(it => it.GiftId)
            .DatabaseSchemaName("projection");

        options.Projections.SelfAggregate<GiftCoupon>(ProjectionLifecycle.Inline);
        options.Projections.SelfAggregate<GiftPendingValidation>(ProjectionLifecycle.Async);

        return options;
    }
}