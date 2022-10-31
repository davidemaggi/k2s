using k2s.Models.k8s;
using k2s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using k8s;
using k8s.KubeConfigModels;
using System.Reflection.Emit;
using AutoMapper.Configuration.Annotations;
using k8s.Models;

namespace k2s.Kube
{
    public partial class KubernetesService
    {

        public async Task<BaseResult<List<PodModel>>> GetPods(string ctx, string ns)
        {

            try
            {
                var tmp = await GetRawPods(ctx, ns);

                return BaseResult<List<PodModel>>.NewSuccess(_mapper.Map<List<PodModel>>(tmp.Content));
            }
            catch (Exception ex)
            {


                return BaseResult<List<PodModel>>.NewError(new List<PodModel>(), "Error getting pods");

            }
        }

        public async Task<BaseResult<List<V1Pod>>> GetRawPods(string ctx, string ns)
        {


            try
            {

                var podList = await GetClient(ctx).CoreV1.ListNamespacedPodAsync(ns);


                return BaseResult<List<V1Pod>>.NewSuccess(podList.Items.ToList());
            }
            catch (Exception ex)
            {


                return BaseResult<List<V1Pod>>.NewError(new List<V1Pod>(), "Error getting pods");

            }



        }

        public async Task<BaseResult<V1Pod>> GetRawPod(string ctx, string ns, string pod)
        {


            try
            {

                var podList = await GetClient(ctx).CoreV1.ListNamespacedPodAsync(ns);


                return BaseResult<V1Pod>.NewSuccess(podList.Items.Where(x => x.Name() == pod).FirstOrDefault());
            }
            catch (Exception ex)
            {


                return BaseResult<V1Pod>.NewError(null, "Error getting services");

            }



        }

        public async Task<BaseResult<List<V1Pod>>> GetRawPods(string ctx, string ns, IDictionary<string,string> selector)
        {


            try
            {
                var labels = new List<string>();

                foreach (var key in selector)
                {
                    labels.Add(key.Key + "=" + key.Value);
                }

                var labelStr = string.Join(",", labels.ToArray());



                var podList = await GetClient(ctx).CoreV1.ListNamespacedPodAsync(namespaceParameter:ns, labelSelector:labelStr);


                return BaseResult<List<V1Pod>>.NewSuccess(podList.Items.ToList());
            }
            catch (Exception ex)
            {


                return BaseResult<List<V1Pod>>.NewError(null, "Error getting services");

            }



        }

        public async Task<BaseResult<List<V1Pod>>> GetRawPodsForService(string ctx, string ns, string svc) {

            try { 
            var service =  GetRawService(ctx, ns,svc).Result;


            if (service.isOk() && service.HasContent()) {


                var pods =  GetRawPods(ctx, ns, service.Content.Spec.Selector).Result;

                return BaseResult<List<V1Pod>>.NewSuccess(pods.Content);

            }

                return BaseResult<List<V1Pod>>.NewError(null, "No Pod found for service");


            }
            catch (Exception ex)
            {


                return BaseResult<List<V1Pod>>.NewError(null, "Error getting pods for service");

            }





}


    }
}
