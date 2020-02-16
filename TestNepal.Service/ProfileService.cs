using AutoMapper;
using TestNepal.Dtos;
using TestNepal.Entities;
using TestNepal.Repository.Common;
using TestNepal.Repository.Infrastructure;
using TestNepal.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TestNepal.Service
{
    public class ProfileService : IProfileService
    {
        private IUserProfileRepository _userProfileRepository;
       
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public ProfileService(IUserProfileRepository userProfileRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
           // _profileAddressRepository = profileAddressRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Boolean Save(Dtos.Profile profile)
        {
            UserProfile userProfile = new UserProfile();
            userProfile = _userProfileRepository.GetById(profile.Id);
            //userProfile.Email = profile.Email;
            userProfile.Phone = profile.Phone;
            userProfile.Photo = string.IsNullOrEmpty(profile.Photo) ? userProfile.Photo : profile.Photo;
            userProfile.FirstName = profile.FirstName;
            userProfile.LastName = profile.LastName;

            _userProfileRepository.Update(userProfile);
            _userProfileRepository.SaveChanges();
            return true;
        }
        public Boolean SaveProfileImage(string Photo, string username)
        {
            UserProfile userProfile = new UserProfile();
            userProfile = _userProfileRepository.Get(a => a.UserName == username);
            userProfile.Photo = Photo;
            _userProfileRepository.Update(userProfile);
            _userProfileRepository.SaveChanges();
            return true;
        }
        public Dtos.Profile GetProfile(String Username)
        {
            UserProfile userProfile = _userProfileRepository.Get(a => a.UserName == Username);
            if(userProfile != null)
            {
                userProfile.Photo = String.IsNullOrEmpty(userProfile.Photo) == false ? ApplicationSettingVariables.WebsiteBaseUrl + ApplicationSettingVariables.ImageUploadPath + userProfile.Photo : "";
            }
            return _mapper.Map<Dtos.Profile>(userProfile);
        }

        public string GetProfileImage(String Username)
        {
            UserProfile userProfile = _userProfileRepository.Get(a => a.UserName == Username);
            return userProfile.Photo;
        }
       

        public Boolean DeleteAddress(int addressId)
        {
            //if (_jobAddressRepository.GetMany(a => a.ProfileAddressId == addressId).Count() == 0)
            //{
            //    ProfileAddress profileAddress = _profileAddressRepository.GetById(addressId);
            //    if (profileAddress.SiteId.HasValue)
            //    {
            //        // Delete Site
            //        Site site = _siteRepository.GetById(profileAddress.SiteId.Value);
            //        _siteRepository.Delete(site);
            //        _siteRepository.SaveChanges();
            //    }
            //    _profileAddressRepository.Delete(profileAddress);
            //    _profileAddressRepository.SaveChanges();
            //    return true;
            //}
            return false;
        }

      
    }
}
