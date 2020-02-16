using System;
using System.Collections.Generic;

namespace TestNepal.Dtos
{
    public class Profile
    {
        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsPhotoChanged { get; set; }
        public double Rating { get; set; }
        public bool IsMobile { get; set; }
        public UserAddress UserAddress { get; set; }
    }

    public class UserAddress
    {
        public int Id { get; set; }

        public string Address { get; set; }
        public string BuildingName { get; set; }
        public string Floor { get; set; }
        public string StreetNumber { get; set; }
        public string StreetAlpha { get; set; }
        public string Street { get; set; }

        public string Suburb { get; set; }

        public string City { get; set; }
        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string GeoLat { get; set; }

        public string GeoLng { get; set; }
        public Int64? SiteId { get; set; }
        public bool IsDefault { get; set; }
    }
}