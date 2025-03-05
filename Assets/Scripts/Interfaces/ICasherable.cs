public interface ICachable<TKey>
{
    TKey GetKey { get; }
}