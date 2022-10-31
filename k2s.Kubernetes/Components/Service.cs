using k2s.Models;
using k2s.Models.k8s;
using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Kube
{
    public partial class KubernetesService
    {
        public async Task<BaseResult<List<ServiceModel>>> GetServices(string ctx, string ns)
        {


            try
            {
                var tmp = await GetRawServices(ctx, ns);


                return BaseResult<List<ServiceModel>>.NewSuccess(_mapper.Map<List<ServiceModel>>(tmp.Content));
            }
            catch (Exception ex)
            {


                return BaseResult<List<ServiceModel>>.NewError(new List<ServiceModel>(), "Error getting services");

            }



        }

        public async Task<BaseResult<List<V1Service>>> GetRawServices(string ctx, string ns)
        {


            try
            {

                var svcList = await GetClient(ctx).CoreV1.ListNamespacedServiceAsync(ns);


                return BaseResult<List<V1Service>>.NewSuccess(svcList.Items.ToList());
            }
            catch (Exception ex)
            {


                return BaseResult<List<V1Service>>.NewError(new List<V1Service>(), "Error getting services");

            }



        }


        public async Task<BaseResult<V1Service>> GetRawService(string ctx, string ns, string svc)
        {


            try
            {

                var svcList = GetClient(ctx).CoreV1.ListNamespacedServiceAsync(ns).Result;

                var lll = svcList.Items.Where(x => x.Name() == svc).FirstOrDefault();

                return BaseResult<V1Service>.NewSuccess(lll);
            }
            catch (Exception ex)
            {


                return BaseResult<V1Service>.NewError(null, "Error getting services");

            }



        }


    }
}
