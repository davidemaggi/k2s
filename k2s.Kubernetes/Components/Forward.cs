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


                var webSocket = GetClient(ctx).WebSocketNamespacedPodPortForwardAsync(pod.Content.Metadata.Name, ns, new int[] { int.Parse(tmpPort) }, "v4.channel.k8s.io").Result;
                var demux = new StreamDemuxer(webSocket, StreamType.PortForward);
                demux.Start();

                var stream = demux.GetStream((byte?)0, (byte?)0);

                IPAddress ipAddress = IPAddress.Loopback;
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, localPort);
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(100);

                Socket handler = null;
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

                 accept.Wait();
                 copy.Wait();
                if (handler != null)
                {
                    handler.Close();
                }
                listener.Close();
            }
            catch (Exception e) {

                Console.WriteLine("");
            
            }
        }
        public async Task PortForwardService(string ctx, string ns, string svc, string svcPort, int localPort) {

            var tmpPort=svcPort.Split(" ")[2];

            var pods = await GetRawPodsForService(ctx,ns,svc);
            


            PortForwardPod(ctx, ns,pods.Content.First().Name(), tmpPort, localPort);


        }


    }
}
