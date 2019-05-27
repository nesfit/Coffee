using System;
using System.Collections.Generic;
using System.Text;

namespace Barista.Common.RestEase
{
    // (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/

    public class RestEaseOptions
    {
        public IEnumerable<Service> Services { get; set; }
        public string LoadBalancer { get; set; }

        public class Service
        {
            public string Name { get; set; }
            public string Scheme { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
        }
    }
}
