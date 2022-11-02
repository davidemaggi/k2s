using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Kube
{
    public partial class KubernetesService
    {


        public async Task PortForwardPod(string ctx, string ns, string podName, string podPort, int localPort) {

            try
            {

                var tmpPort = podPort.Split(" ")[0];

                var pod = GetRawPod(ctx, ns, podName).Result;

                RunKubectlCommand(new List<string>() { "port-forward", $"pod/{podName}", $"{localPort}:{tmpPort}", $"-n {ns}" });

                //await Forward(GetClient(ctx),pod.Content, ns, Convert.ToInt32(tmpPort), localPort);
            }
            catch (Exception e) {

                Console.WriteLine("");
            
            }
        }
        public async Task PortForwardService(string ctx, string ns, string svc, string svcPort, int localPort) {

            var tmpPort=svcPort.Split(" ")[0];

            var pods = await GetRawPodsForService(ctx,ns,svc);


            RunKubectlCommand(new List<string>() { "port-forward", $"service/{svc}", $"{localPort}:{tmpPort}", $"-n {ns}" });

            //await PortForwardPod(ctx, ns,pods.Content.First().Name(), tmpPort, localPort);


        }


        private static async Task Forward(IKubernetes client, V1Pod pod,string ns, int port, int localport)
        {
            // Note this is single-threaded, it won't handle concurrent requests well...
            var webSocket = await client.WebSocketNamespacedPodPortForwardAsync(pod.Metadata.Name, ns, new int[] { port }, "v4.channel.k8s.io");
            var demux = new StreamDemuxer(webSocket, StreamType.PortForward);
            demux.Start();

            var stream = demux.GetStream((byte?)0, (byte?)0);

            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, localport);
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);

            Socket handler = null;

            // Note this will only accept a single connection
            var accept = Task.Run(() =>
            {
                while (true)
                {
                    handler = listener.Accept();
                    var bytes = new byte[4096];
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        stream.Write(bytes, 0, bytesRec);
                        if (bytesRec == 0 || Encoding.ASCII.GetString(bytes, 0, bytesRec).IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                }
            });

            var copy = Task.Run(() =>
            {
                var buff = new byte[4096];
                while (true)
                {
                    var read = stream.Read(buff, 0, 4096);
                    handler.Send(buff, read, 0);
                }
            });

            await accept;
            await copy;
            if (handler != null)
            {
                handler.Close();
            }
            listener.Close();
        }


    }
}
