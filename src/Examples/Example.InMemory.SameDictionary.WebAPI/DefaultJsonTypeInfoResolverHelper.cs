using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Example.Domain.Events;
using SimpleEventSourcing;

namespace Example.InMemory.SameDictionary.WebAPI;

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
                    if (jsonTypeInfo.Type == typeof(Event))
                    {
                        jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                        {
                            TypeDiscriminatorPropertyName = "$type",
                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                            DerivedTypes =
                            {
                                new JsonDerivedType(typeof(UserCreateEvent), "userCreateEvent"),
                                new JsonDerivedType(typeof(UserUpdateEvent), "userUpdateEvent"),
                                new JsonDerivedType(typeof(OrderCreateEvent), "orderCreateEvent"),
                            }
                        };
                    }
                }
            }
        };
    }
}
