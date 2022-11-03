using k2s.Models;
using k2s.Models.k8s;
using k8s.KubeConfigModels;
using k8s.Models;

namespace k2s.Kube
{
    public interface IKubernetesService
    {

        BaseResult SetOverrideFile(string file);

        BaseResult<List<ContextModel>> GetContexts();
        Task<BaseResult<List<NamespaceModel>>> GetNamespaces(string ctx = null);
        string GetConfigPath();
        BaseResult<string> GetCurrentContext();

        BaseResult SetContext(string name, bool save = false);
        BaseResult SetNameSpace(string context, string name, bool save = false);

        BaseResult<string> GetCurrentNameSpace(string context);


        BaseResult SaveKubeConfig(bool backup = true);


        BaseResult<MergeResult> MergeKubeConfig(string toBeMerged, bool force = false, bool verbose = false);

        BaseResult SetConfig(K8SConfiguration newConf, bool withSave = false);
        BaseResult RenameContext(string renameCtx, string newName);
        BaseResult DeleteContext(string deleteCtx, bool save=true);
        BaseResult DeleteContexts(List<string> deleteCtxs);
        Task<BaseResult<List<PodModel>>> GetPods(string ctx, string ns);
        Task<BaseResult<List<ServiceModel>>> GetServices(string fwdCtx, string fwdNs);

        Task<BaseResult<List<V1Pod>>> GetRawPodsForService(string ctx, string ns, string svc);

        Task<BaseResult> PortForwardPod(string ctx, string ns, string podName, string svcPort, int localPort);
        Task<BaseResult> PortForwardService(string ctx, string ns, string svc, string svcPort, int localPort);

    }
}