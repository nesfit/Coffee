namespace Barista.Api.Authenticators
{
    public interface IMeansValueHasher
    {
        string Hash(string originalValue);
    }
}
