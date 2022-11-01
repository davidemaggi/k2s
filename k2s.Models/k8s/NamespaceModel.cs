using System;
using System.Collections.Generic;
using System.Text;

namespace k2s.Models.k8s
{
    public class NamespaceModel
    {
        public string Name { get; set; }
        public bool IsCurrent { get; set; } = false;

    }
}
