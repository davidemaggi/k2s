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
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<V1Service, ServiceModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name()))
                                .ForMember(dest => dest.Ports, opt => opt.MapFrom(src => GetPortsFrom(src.Spec.Ports)))

                ;
        }


        private List<PortModel> GetPortsFrom(IList<V1ServicePort> ports)
        {

            var ret = new List<PortModel>();

            foreach (var port in ports)
            {

                

                        ret.Add(new PortModel() { InternalPort = ""+port.TargetPort.Value.ToString(), ExternalPort = port.Port.ToString(), Protocol = port.Protocol });

                    
                

            }

            return ret;
        }

    }
}
