using System;
using Agency.Data.Entities;
using Agency.Web.ViewModels;
using System.Data;
using AutoMapper;

namespace Agency.Web
{
    public class AutoMapperMappings : Profile
    {
        public AutoMapperMappings()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
            //CreateMap<Role, RoleViewModel>().ReverseMap();

        }
    }
}

