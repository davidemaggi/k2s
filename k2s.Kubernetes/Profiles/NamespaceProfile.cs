using AutoMapper;
using k2s.Models.k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Kube.Profiles
{
    public class NamespaceProfile : Profile
    {
        public NamespaceProfile()
        {
            CreateMap<V1Namespace, NamespaceModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name()))
                ;
        }
    }
}
