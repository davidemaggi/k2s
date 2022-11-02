

using k2s.Models;
using k2s.Models.k8s;
using k8s;
using k8s.KubeConfigModels;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace k2s.Kube
{
    public partial class KubernetesService
    {
        
        public async Task<BaseResult<List<NamespaceModel>>> GetNamespaces(string ctx=null)
        {

            try
            {

                var namespaces = await GetClient(ctx).CoreV1.ListNamespaceAsync();

                
                //return BaseResult<List<NamespaceModel>>.NewSuccess(_mapper.Map<List<NamespaceModel>>(namespaces.Items));

                var ret = new List<NamespaceModel>();

                foreach (var ns in namespaces)
                {

                    var tmp = _mapper.Map<NamespaceModel>(ns);



                    if (tmp.Name == GetCurrentNameSpace(ctx).Content) { tmp.IsCurrent = true; }

                    ret.Add(tmp);
                }



                return BaseResult<List<NamespaceModel>>.NewSuccess(ret.OrderByDescending(o => o.IsCurrent).ToList(), $"Retrieved {ret.Count} namespace(s)");



            }



            catch (Exception ex)
            {

                
                return BaseResult<List<NamespaceModel>>.NewError(new List<NamespaceModel>(), "Error getting namespaces");

            }

           
        }
        public BaseResult<string> GetCurrentNameSpace(string context)
        {

            foreach (var ctx in _config.Contexts)
            {

                if (ctx.Name.Equals(context))
                {

                    
                    return BaseResult<string>.NewSuccess(ctx.ContextDetails.Namespace, $"Retrieved current namespace");

                }



            }




            return BaseResult<string>.NewWarning("NA");


        }

        public BaseResult SetNameSpace(string context, string name, bool save = false)
        {


            try
            {

                foreach (var ctx in _config.Contexts)
                {

                    if (ctx.Name.Equals(context))
                    {

                        ctx.ContextDetails.Namespace = name;

                    }



                }



                if (save) return SaveKubeConfig(true);

                return BaseResult.NewSuccess($"Namespace Set");

            }
            catch (Exception e)
            {
                return BaseResult.NewError("Error setting context");
            }



        }


    }
}
