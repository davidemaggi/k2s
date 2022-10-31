using AutoMapper;
using k2s.Models;
using k2s.Models.k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Kube.Profiles
{
    public class PodProfile : Profile
    {
        public PodProfile()
        {
            CreateMap<V1Pod, PodModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name()))
                .ForMember(dest => dest.Ports, opt => opt.MapFrom(src => GetPortsFrom(src.Spec.Containers)))
                ;
        }



        private List<PortModel> GetPortsFrom(IList<V1Container> containers) {

            var ret = new List<PortModel>();

            foreach (var container in containers) {

                if (container.Ports!=null) { 
                foreach (var port in container.Ports) {

                    ret.Add(new PortModel() { ExternalPort=port.ContainerPort.ToString(), Protocol=port.Protocol });

                }
                }

            }

            return ret;
        }



    }
}
