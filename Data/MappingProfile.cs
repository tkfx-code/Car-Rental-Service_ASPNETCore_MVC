using AutoMapper;
using MVC_Project.Model;
using MVC_Project.Models;

namespace MVC_Project.Data
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerViewModel,Customer>().ReverseMap();
        }
    }
}
