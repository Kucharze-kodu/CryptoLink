using CryptoLink.Domain.Common.Primitives;
using System.Text.Json.Serialization;


namespace CryptoLink.Domain.Aggregates.LinkExtendeds.ValueObcjects;

public sealed class Links : ValueObject
{

    public int Counter { get; private set; }

    [JsonConstructor]
    private Links()
    {
        Counter = 0;
    }

    public static Links Create() => new();

    public void Increment()
    {
        Counter++;
    }

    public void LoadCounter(int counter)
    {
        Counter = counter;
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Counter;
    }
}
