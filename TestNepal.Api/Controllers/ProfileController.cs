using TestNepal.Api.Helpers;
using TestNepal.Dtos;
using TestNepal.Entities;
using TestNepal.Repository.Infrastructure;
using TestNepal.Service.Infrastructure;
using TestNepal.API.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestNepal.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/profile")]
    public class ProfileController : BaseApiController
    {
        private IUserProfileRepository _userProfileRepository;
        private IProfileService _profileService;
        private IFileService _fileService;
        private IUserService _userService;
        public ProfileController(IProfileService profileService, IFileService fileService, IUserService userService, IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
            _profileService = profileService;
     
            _fileService = fileService;
            _userService = userService;
        }

        [HttpPost, Route("save")]
        public object saveProfile(Profile profile)
        {
            if (profile.IsPhotoChanged)
                profile.Photo = _fileService.SaveImage(ApplicationSettingVariables.ImageUploadPath, profile.Photo);
            else
                profile.Photo = "";
            if (_profileService.Save(profile))
                return Common.JsonOkObject(
                       new { msg = "Profile updated successully" }
                   );
            else
                return Common.JsonErrorObject("Some error occured. Try again later.");

        }

        [HttpPost, Route("saveprofileimage")]
        public object saveProfileImage(Profile profile)
        {
            if (profile.IsPhotoChanged)
            {
                profile.Photo = _fileService.SaveImage(ApplicationSettingVariables.ImageUploadPath, profile.Photo);
                if (_profileService.SaveProfileImage(profile.Photo, this.GetUserName()))
                    profile.Photo = String.IsNullOrEmpty(profile.Photo) == false ? ApplicationSettingVariables.WebsiteBaseUrl + ApplicationSettingVariables.ImageUploadPath + profile.Photo : "";
                    return Common.JsonOkObject(
                           new { Photo = profile.Photo, msg = "Uploaded Successully" }
                       );
            }
            return Common.JsonErrorObject("Some error occured. Try again later.");
        }

        [HttpPost, Route("uploadProfileImage")]
        public object UploadProfileImage()
        {
            Profile profile = new Profile();
            if (System.Web.HttpContext.Current.Request.Files.Count > 0)
            {
                profile.Photo = _fileService.SaveImage(ApplicationSettingVariables.ImageUploadPath, System.Web.HttpContext.Current.Request.Files[0]);

                if (string.IsNullOrEmpty(profile.Photo) == false)
                {
                    if (_profileService.SaveProfileImage(profile.Photo, this.GetUserName()))
                        return Common.JsonOkObject(
                               new { Photo = profile.Photo, msg = "Uploaded Successully" }
                           );
                }
            }
            return Common.JsonErrorObject("Some error occured. Try again later.");

        }

        [HttpGet, Route("getprofile")]
        public Profile GetProfile()
        {
           return _profileService.GetProfile(this.GetUserName());
        }
       

        [HttpPost, Route("deleteaddress")]
        public object DeleteAddress(int addressId)
        {
            if (_profileService.DeleteAddress(addressId))
                return Common.JsonOkObject(
                       new { msg = "Address Deleted Successully" }
                   );
            else
                return Common.JsonErrorObject("Some error occured. Try again later.");

        }
   
        [HttpGet, Route("getprofileimage")]
        public object GetProfileImage()
        {
            string ProfileImageUrl = _profileService.GetProfileImage(this.GetUserName());
            string base64Img =  _fileService.ConvertToBase64FromUrl(ApplicationSettingVariables.ImageUploadPath + ProfileImageUrl);
            return Common.JsonOkObject(base64Img);
        }

        [HttpPost, Route("changePassword")]
        public object ChangePassword(string oldPassword, string newPassword)
        {
            return _userService.ChangePassword(oldPassword, newPassword, this.GetUserName());
        }
    }
}