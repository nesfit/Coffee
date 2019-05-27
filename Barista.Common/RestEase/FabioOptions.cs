namespace Barista.Common.RestEase
{
    public class FabioOptions : IFabioOptions
    {
        public string Url { get; set; }
        public int RequestRetries { get; set; }
    }
}
