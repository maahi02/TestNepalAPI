using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TestNepal.API.Mappers
{
    /// <summary>
    /// Enable one way or
    /// two way binding
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.UserProfile, Dtos.Profile>();


            //---one to many mapping TECHNIQUE
            CreateMap<Entities.Employee, Dtos.EmployeeDto>();
            CreateMap<Dtos.EmployeeDto, Entities.Employee>();

        }
    }
}