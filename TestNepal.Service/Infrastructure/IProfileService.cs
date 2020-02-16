using TestNepal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Service.Infrastructure
{
    public interface IProfileService
    {
        Boolean Save(Profile profile);
        Boolean SaveProfileImage(string Photo, string username);
        Profile GetProfile(String Username);
        string GetProfileImage(String Username);
        Boolean DeleteAddress(int addressId);
    }
}
