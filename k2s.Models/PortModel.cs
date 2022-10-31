using System;
using System.Collections.Generic;
using System.Text;

namespace k2s.Models
{
    public class PortModel
    {

        public string Protocol { get; set; } = "TCP";
        public string ExternalPort { get; set; }
        public string InternalPort { get; set; }

    }
}
