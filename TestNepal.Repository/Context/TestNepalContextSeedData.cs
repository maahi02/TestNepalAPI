using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestNepal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using TestNepal.Dtos;
using static TestNepal.Entities.Enums;

namespace TestNepal.Context
{
    public class TestNepalContextSeedData
    {
        private TestNepalContext _context;

        public TestNepalContextSeedData(TestNepalContext context)
        {
            _context = context;
        }

        public async void SeedDefaultDatas()
        {
            if (_context.Clients.Count() == 0)
            {
                _context.Clients.AddRange(BuildClientsList());
                await _context.SaveChangesAsync();
            }

            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "CUSTOMER"))
                await roleStore.CreateAsync(new IdentityRole { Name = "CUSTOMER" });

            if (!_context.Roles.Any(r => r.Name == "AUTHER"))
                await roleStore.CreateAsync(new IdentityRole { Name = "AUTHER" });

            if (!_context.Roles.Any(r => r.Name == "ADMINISTRATOR"))
                await roleStore.CreateAsync(new IdentityRole { Name = "ADMINISTRATOR" });

            //Adminstrator
            var user = new ApplicationUser
            {
                UserName = "admin@TestNepal.com",
                Email = "admin@TestNepal.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserProfile = new UserProfile()
                {
                    FirstName = "admin",
                    UserName = "admin@TestNepal.com"
                }
            };
            if (!_context.Users.Any(a => a.UserName == "admin@TestNepal.com"))
            {
                var password = new PasswordHasher();
                var hashed = password.HashPassword("abc123#");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(_context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "ADMINISTRATOR");
            }

            Guid adminUserId = Guid.Empty;
        }

        private string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }

        private List<Client> BuildClientsList()
        {
            List<Client> ClientsList = new List<Client>
            {
                new Client
                {
                    Id = "TeachNepalWebApp",
                    Secret = GetHash("TeachNepal@webapp"),
                    Name="TeachNepal front-end Application",
                    ApplicationType =  Entities.ApplicationType.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 144000,
                    AllowedOrigin = "*"
                },
                new Client
                {
                    Id = "TeachNepalMobileApp",
                    Secret= GetHash("TeachNepal@mobileapp"),
                    Name = "TeachNepal Mobile App",
                    ApplicationType =Entities.ApplicationType.NativeConfidential,
                    Active = true,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                }
            };
            return ClientsList;
        }

       

       
       
       
      



    }
}