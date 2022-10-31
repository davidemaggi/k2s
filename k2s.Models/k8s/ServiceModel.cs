using System;
using System.Collections.Generic;
using System.Text;

namespace k2s.Models.k8s
{
    public class ServiceModel
    {
        public string Name { get; set; }
        public List<PortModel> Ports { get; set; } = new List<PortModel>();

    }
}
