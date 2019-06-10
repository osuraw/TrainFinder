

using AutoMapper;
using pro_web_a.DTOs;
using pro_web_a.Models;

namespace pro_web_a
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Route, RouteDto>();
            
        }
    }
}