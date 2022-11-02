using AutoMapper;
using k2s.Models;
using k2s.Models.k8s;
using k8s;
using k8s.KubeConfigModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace k2s.Kube
{
    public partial class KubernetesService: IKubernetesService
    {
        //private KubernetesClientConfiguration _config;
        
        readonly IMapper _mapper;

        
        private K8SConfiguration _config;

        private Dictionary<string,Kubernetes> _clients;
        private string _overridePath="";

        public KubernetesService(IMapper mapper) {
            _mapper = mapper;
            
            _clients = new Dictionary<string, Kubernetes>(); 
            SetupClients();

        }

        private void SetupClients() {

            _clients.Clear();

            _config = ReadKubeConfig(GetConfigPath());
            


            foreach (var ctx in _config.Contexts) {


                var tmpConf = KubernetesClientConfiguration.BuildConfigFromConfigFile(GetConfigPath(), ctx.Name);
                tmpConf.SkipTlsVerify = true;
                _clients.Add(ctx.Name, new Kubernetes(tmpConf));


            }

           



            
            
        }



        public Kubernetes GetClient(string? name) => string.IsNullOrEmpty(name) ? _clients[GetCurrentContext().Content] : _clients[name];

        public string GetConfigPath() => string.IsNullOrEmpty(_overridePath) ? Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".kube","config") : _overridePath;

        public K8SConfiguration ReadKubeConfig(string filePath)
        {



            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
           .WithNamingConvention(CamelCaseNamingConvention.Instance)
           .IgnoreUnmatchedProperties()
           .Build();

            K8SConfiguration kubeconfig = deserializer.Deserialize<K8SConfiguration>(File.ReadAllText(filePath));
            return kubeconfig;
        }


        public BaseResult SaveKubeConfig( bool backup = true)
        {
            try { 
            var path = GetConfigPath();
            if (backup)
            {
                File.Delete($"{path}.backup");
                File.Copy(path, $"{path}.backup");
            }


            var serializer = new SerializerBuilder()
   .WithNamingConvention(CamelCaseNamingConvention.Instance)
   .Build();

            var stringResult = serializer.Serialize(_config);

            File.WriteAllText(path, stringResult);

                SetupClients();

            return BaseResult.NewSuccess("Config file saved");
            }
            catch (Exception e) {

                return BaseResult.NewFatal("Error saving config file");

            }
        }

        public BaseResult SetConfig(K8SConfiguration newConf, bool withSave = false) { 
        
            _config = newConf;
            if (withSave) SaveKubeConfig();


            return BaseResult.NewSuccess($"Updated config");

        }

        public BaseResult SetOverrideFile(string file)
        {
            if (!string.IsNullOrEmpty(file)) { 
            if (File.Exists(file))
            {
                _overridePath = file;


                SetupClients();
            }
            else {

                return BaseResult.NewWarning($"Specified file doesn't exist, the default config will be used");

            }
            }

            return BaseResult.NewSuccess($"Using custom file");
        }
    }
}
