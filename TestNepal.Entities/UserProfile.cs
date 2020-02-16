using TestNepal.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestNepal.Entities
{
    public class UserProfile : ICreated, IUpdated, ISecuredByTenant, ISecuredByUser
    {
        [Key]
        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
     
        public virtual ApplicationUser User { get; set; }
        public Guid TenantId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedById { get; set; }
        public Guid UserId { get; set; }

        public Guid? PickedByFranchiseeId { get; set; }
        public String MYOBDebtorId { get; set; }
        public String MYOBError { get; set; }
        public String MYOBBranchId { get; set; }

        public bool IsPushSetup { get; set; }
        public bool StratcleanupPush { get; set; }

        public double Rating { get; set; }
        public bool IsMobile { get; set; }

        public string FilePath { get; set; }
        public string ThumbnailFilePath { get; set; }
       
    }
}
