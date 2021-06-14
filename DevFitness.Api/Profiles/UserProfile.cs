using AutoMapper;
using DevFitness.Api.Core.Entities;
using DevFitness.Api.Models.InputModels;
using DevFitness.Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevFitness.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<CreateUserInputModel, User>();
        }
    }
}
