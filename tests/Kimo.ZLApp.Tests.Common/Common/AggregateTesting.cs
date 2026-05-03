using System.Reflection;
using Kimo.ZLApp.Domain.Common;
using Shouldly;

namespace Kimo.ZLApp.Tests.Common.Common;

public static class AggregateTesting
{
    public static void VerifyOrmCreationAndId<TAggregate, TKey>(string idPropertyName, TKey idTestValue)
        where TAggregate : IAggregateRoot
    {
        var privateDefaultCtor = FindAggregateConstructor<TAggregate>();
        var aggregate = (TAggregate)privateDefaultCtor.Invoke(null);

        VerifyIdProperty(idTestValue, idPropertyName, aggregate);
    }

    private static void VerifyIdProperty<TAggregate, TKey>(
        TKey idTestValue,
        string? idPropertyName,
        TAggregate aggregate) where TAggregate : IAggregateRoot
    {
        var idProperty = typeof(TAggregate).GetProperty(idPropertyName!);
        idProperty.ShouldNotBeNull("Id property should exist on the aggregate");

        idProperty.SetValue(aggregate, idTestValue);

        var idValue = idProperty.GetValue(aggregate);
        idValue.ShouldBe(idTestValue);
    }

    private static ConstructorInfo FindAggregateConstructor<TAggregate>() where TAggregate : IAggregateRoot
    {
        var privateDefaultCtor = typeof(TAggregate).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            Type.EmptyTypes,
            null
        );

        privateDefaultCtor.ShouldNotBeNull("Aggregate should have a private default constructor");

        return privateDefaultCtor;
    }
}
