using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Dtos
{
    public class EmployeeDto
    {
        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public decimal? Salary { get; set; }
        public string Designation { get; set; }
        public string Photo { get; set; }
        public DateTime? ImportedDate { get; set; }

    }

    public class EmployeePagedViewModel : PaginationViewModel
    {
        public string Gender { get; set; }
        public decimal? StartSalary { get; set; }
        public decimal? EndSalary { get; set; }
        public DateTime? DobStart { get; set; }
        public DateTime? DobEnd { get; set; }
    }

    public class EmployeePrintGetViewModel
    {
        public List<Int64> ids { get; set; }
        public bool isAll { get; set; }
    }
}
