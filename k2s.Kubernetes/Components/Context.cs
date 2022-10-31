using k2s.Models;
using k2s.Models.k8s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Kube
{
    public partial class KubernetesService
    {
        public BaseResult<List<ContextModel>> GetContexts()
        {

            return BaseResult<List<ContextModel>>.NewSuccess(_mapper.Map<List<ContextModel>>(_config.Contexts.ToList()));


        }

        public BaseResult<string> GetCurrentContext()
        {

            return BaseResult<string>.NewSuccess(_config.CurrentContext);


        }


        public BaseResult SetContext(string name, bool save = false)
        {



            try
            {

                _config.CurrentContext = name;
                //Save


                if (save) return SaveKubeConfig(true);

                return BaseResult.NewSuccess();

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


        public BaseResult DeleteContext(string deleteCtx)
        {

            var toDelete = _config.Contexts.Where(x=>x.Name==deleteCtx).FirstOrDefault();

            if (toDelete!=null) { 
            _config.Contexts = _config.Contexts.Where(x => x.Name != deleteCtx);
            _config.Users = _config.Users.Where(x => x.Name != toDelete.ContextDetails.User);
            _config.Clusters = _config.Clusters.Where(x => x.Name != toDelete.ContextDetails.Cluster);

            return SaveKubeConfig(true);
            }

            return BaseResult.NewWarning("Context to delete has not been found");
        }

        
    }
}
