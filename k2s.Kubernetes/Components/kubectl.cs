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



        public BaseResult RunKubectlCommand(List<string> parameters) {

            if (FoundKubectl())
            {



                try {

                    parameters.Add("--kubeconfig");
                    parameters.Add(GetConfigPath());

                    Process ExternalProcess = new Process();
                ExternalProcess.StartInfo.FileName = "kubectl";
                ExternalProcess.StartInfo.Arguments = string.Join(" ", parameters);
                ExternalProcess.StartInfo.CreateNoWindow = false;
                ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                ExternalProcess.Start();
                ExternalProcess.WaitForExit();

                return BaseResult.NewSuccess("Executed Command");
                }
                catch {

                    return BaseResult.NewError("Error Running Command");


                }
            }
            else {

                return BaseResult.NewError("kubectl cannot be found");


            }



        }


        public bool FoundKubectl()
        {

            try
            {

                Process ExternalProcess = new Process();
                ExternalProcess.StartInfo.FileName = "kubectl";
                ExternalProcess.StartInfo.Arguments = "version --short";
                ExternalProcess.StartInfo.CreateNoWindow = false;
                ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                ExternalProcess.StartInfo.RedirectStandardOutput = true;
                ExternalProcess.StartInfo.RedirectStandardError = true;
                ExternalProcess.Start();
                ExternalProcess.WaitForExit();
                return true;

            }
            catch {

                return false;
            
            }



        }



    }
}
