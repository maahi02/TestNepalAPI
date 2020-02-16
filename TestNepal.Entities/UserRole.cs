namespace TestNepal.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public Int64 UserRoleId { get; set; }

        public int? UserId { get; set; }

        public int? RoleId { get; set; }
    }
}
