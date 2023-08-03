using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using k2s.Models;

namespace k2s.Kube
{
    public partial class KubernetesService
    {

        public  async Task<BaseResult> ShellConnectPod(string ctx, string ns, string podName) {

            try
            {

                var tmpPort = podPort.Split(" ")[0];

                var pod = GetRawPod(ctx, ns, podName).Result;

               return RunKubectlCommand(new List<string>() { "port-forward", $"pods/{podName}", $"{localPort}:{tmpPort}", $"-n {ns}" });

            }
            catch (Exception e) {

                return BaseResult.NewError("Error Connecting to Shell");

            }
        }
    }
}
