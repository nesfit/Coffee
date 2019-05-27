namespace Barista.Common.Consul
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public class ConsulOptions
    {
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string Service { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public bool PingEnabled { get; set; }
        public string PingEndpoint { get; set; }
        public int PingInterval { get; set; }
        public int RemoveAfterInterval { get; set; }
        public int RequestRetries { get; set; }
        public bool SkipLocalhostDockerDnsReplace { get; set; }
        public string[] UrlPrefixes { get; set; }
    }
}