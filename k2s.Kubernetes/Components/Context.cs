using k2s.Models;
using k2s.Models.k8s;
using k8s.KubeConfigModels;
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
        public BaseResult<List<ContextModel>> GetContexts()
        {

            var ret = new List<ContextModel>(); 

            foreach (var context in _config.Contexts) {

                var tmp = _mapper.Map<ContextModel>(context);

                if (tmp.Name==_config.CurrentContext) { tmp.IsCurrent = true; }

                ret.Add(tmp);
            }



            return BaseResult<List<ContextModel>>.NewSuccess(ret.OrderByDescending(o => o.IsCurrent).ToList(),$"Retrieved {ret.Count} contexts");
        }

        public BaseResult<string> GetCurrentContext()
        {

            return BaseResult<string>.NewSuccess(_config.CurrentContext, $"Retrieved Current Context");


        }


        public BaseResult SetContext(string name, bool save = false)
        {



            try
            {

                _config.CurrentContext = name;
                //Save


                if (save) return SaveKubeConfig(true);

                return BaseResult.NewSuccess($"Context Set");

            }
            catch (Exception e)
            {
                return BaseResult.NewError("Error setting context");
            }

        }
        public BaseResult RenameContext(string renameCtx, string newName) {

            foreach (var ctx in _config.Contexts) {

                if (ctx.Name==renameCtx) { 
                ctx.Name= newName;
                    break;
                }

            }

           return SaveKubeConfig(true);

           
        
        }

        public BaseResult DeleteContexts(List<string> deleteCtxs)
        {

            foreach (string ctx in deleteCtxs) {

                var tmpRes = DeleteContext(ctx,false);

                if (!tmpRes.isOk()) {

                    return BaseResult.NewError($"Error deleting context {ctx}");
                
                }
            
            }
            return SaveKubeConfig(true);


        }


        public BaseResult DeleteContext(string deleteCtx,  bool save=true)
        {

            var toDelete = _config.Contexts.Where(x=>x.Name==deleteCtx).FirstOrDefault();

            if (toDelete!=null) { 

            
                if (ShouldDeleteUser(_config.Contexts, toDelete.ContextDetails.User)) { 
                _config.Users = _config.Users.Where(x => x.Name != toDelete.ContextDetails.User).ToList();
                }
                if (ShouldDeleteCluster(_config.Contexts, toDelete.ContextDetails.Cluster))
                {
                    _config.Clusters = _config.Clusters.Where(x => x.Name != toDelete.ContextDetails.Cluster).ToList();
                }

                _config.Contexts = _config.Contexts.Where(x => x.Name != deleteCtx).ToList();

                if (save)
                {
                    return SaveKubeConfig(true);
                }
                else {

                    return BaseResult.NewSuccess("Deleted, waiting for save");

                }

            }

            return BaseResult.NewWarning("Context to delete has not been found");
        }

        private bool ShouldDeleteCluster(IEnumerable<Context> contexts, string cluster)
        {
            

            var nCluster = contexts.Where(x => x.ContextDetails.Cluster == cluster).Count();

            return nCluster == 1;

        }

        private bool ShouldDeleteUser(IEnumerable<Context> contexts, string user)
        {
            var nUser = contexts.Where(x => x.ContextDetails.User == user).Count();

            return nUser == 1;
        }
    }
}
