using k8s.KubeConfigModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace k2s.Models
{
    public  class MergeResult
    {
        public List<string> Added { get; set; }
        public List<string> Modified { get; set; }
        public List<string> Deleted { get; set; }
        public K8SConfiguration Merged { get; set; }

        public MergeResult() { 
        
            Added = new List<string>();
            Modified = new List<string>();
            Deleted = new List<string>();

        }

        public bool DoneSomething() => Added.Count + Modified.Count + Deleted.Count > 0;

    }
}
