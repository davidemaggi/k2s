using k2s.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Kube
{
    public partial class KubernetesService
    {



        public void RunKubectlCommand(List<string> parameters) {



            Process ExternalProcess = new Process();
            ExternalProcess.StartInfo.FileName = "kubectl";
            ExternalProcess.StartInfo.Arguments = string.Join(" ",parameters);
            ExternalProcess.StartInfo.CreateNoWindow = false;
            ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            ExternalProcess.Start();
            ExternalProcess.WaitForExit();





        }



    }
}
