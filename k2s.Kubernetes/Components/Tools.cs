using k2s.Models;
using k8s.KubeConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Kube
{
    public partial class KubernetesService
    {
        public BaseResult<MergeResult> MergeKubeConfig(string toBeMerged, bool force = false, bool verbose=false)
        {


            var ret = new MergeResult();

            try {
                K8SConfiguration kubeconfig = ReadKubeConfig(GetConfigPath());
                K8SConfiguration kubeconfigMerge = ReadKubeConfig(toBeMerged);


                foreach (var ctx in kubeconfigMerge.Contexts)
                {

                    var contextAlreadyExists = kubeconfig.Contexts.Any(x => x.Name.Equals(ctx.Name, StringComparison.OrdinalIgnoreCase));


                    if (!contextAlreadyExists || force)
                    {
                        if (contextAlreadyExists) {
                            ret.Modified.Add(ctx.Name);
                        } else {
                            ret.Added.Add(ctx.Name);
                        }

                        // Add Context
                        kubeconfig.Contexts = kubeconfig.Contexts.Where(x => !x.Name.Equals(ctx.Name, StringComparison.OrdinalIgnoreCase)).ToList();
                        kubeconfig.Contexts = kubeconfig.Contexts.Concat(new[] { ctx }).ToList();

                        //ret.details.Add(new MergeResultItemModel(Kind.context, ctx.Name, contextAlreadyExists ? EditAction.Modified : EditAction.Added));

                        var clusterAlreadyExists = kubeconfig.Clusters.Any(x => x.Name.Equals(ctx.ContextDetails.Cluster, StringComparison.OrdinalIgnoreCase));
                        if (clusterAlreadyExists)
                        {
                            kubeconfig.Clusters = kubeconfig.Clusters.Where(x => !x.Name.Equals(ctx.ContextDetails.Cluster, StringComparison.OrdinalIgnoreCase)).ToList();



                        }

                        // Add Cluster
                        var cluster = kubeconfigMerge.Clusters.FirstOrDefault(x => x.Name.Equals(ctx.ContextDetails.Cluster, StringComparison.OrdinalIgnoreCase));
                        kubeconfig.Clusters = kubeconfig.Clusters.Concat(new[] { cluster }).ToList();

                        // ret.details.Add(new MergeResultItemModel(Kind.cluster, ctx.ContextDetails.Cluster, clusterAlreadyExists ? EditAction.Modified : EditAction.Added));

                        // Add User

                        var userAlreadyExists = kubeconfig.Users.Any(x => x.Name.Equals(ctx.ContextDetails.User, StringComparison.OrdinalIgnoreCase));

                        if (userAlreadyExists)
                        {
                            kubeconfig.Users = kubeconfig.Users.Where(x => !x.Name.Equals(ctx.ContextDetails.User, StringComparison.OrdinalIgnoreCase)).ToList();

                        }

                        var user = kubeconfigMerge.Users.FirstOrDefault(x => x.Name.Equals(ctx.ContextDetails.User, StringComparison.OrdinalIgnoreCase));
                        kubeconfig.Users = kubeconfig.Users.Concat(new[] { user }).ToList();
                        //ret.details.Add(new MergeResultItemModel(Kind.user, ctx.ContextDetails.User, userAlreadyExists ? EditAction.Modified : EditAction.Added));






                    }



                }
                ret.Merged = kubeconfig;
            }
            catch (Exception e) {

                return BaseResult<MergeResult>.NewFatal(ret);

            }

            return BaseResult<MergeResult>.NewSuccess(ret);

        }

    }
}
