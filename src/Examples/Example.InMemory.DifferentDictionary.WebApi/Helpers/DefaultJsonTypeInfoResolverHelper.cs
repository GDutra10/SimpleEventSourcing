using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Example.Domain.Events;

namespace Example.InMemory.DifferentDictionary.WebApi.Helpers;

public static class DefaultJsonTypeInfoResolverHelper
{
    public static DefaultJsonTypeInfoResolver GetEventResolver()
    {
        return new DefaultJsonTypeInfoResolver()
        {
            Modifiers =
            {
                static jsonTypeInfo =>
                {
                    if (jsonTypeInfo.Type == typeof(UserEvent))
                    {
                        jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                        {
                            TypeDiscriminatorPropertyName = "$type",
                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                            DerivedTypes =
                            {
                                new JsonDerivedType(typeof(UserCreateEvent), "userCreateEvent"),
                                new JsonDerivedType(typeof(UserUpdateEvent), "userUpdateEvent"),
                            }
                        };
                    }

                    if (jsonTypeInfo.Type == typeof(OrderEvent))
                    {
                        jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                        {
                            TypeDiscriminatorPropertyName = "$type",
                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                            DerivedTypes =
                            {
                                new JsonDerivedType(typeof(OrderCreateEvent), "orderCreateEvent"),
                            }
                        };
                    }
                }
            }
        };
    }
}
