using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Entities.Common;

namespace TestNepal.Entities
{
    public class Employee : ICreated, IUpdated
    {
        [Key]
        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public decimal? Salary { get; set; }
        public string Designation { get; set; }
        public string Photo { get; set; }
        public DateTime? ImportedDate { get; set; }

        public Guid CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
