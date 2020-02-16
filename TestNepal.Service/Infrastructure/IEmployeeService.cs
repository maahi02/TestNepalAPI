using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Dtos;
using TestNepal.Entities;

namespace TestNepal.Service.Infrastructure
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAll();
        IEnumerable<EmployeeDto> GetAll(Expression<Func<Employee, bool>> where = null);
        IEnumerable<Employee> GetAll(params Expression<Func<Employee, object>>[] includeExpressions);
        IEnumerable<Employee> GetAll(Expression<Func<Employee, bool>> where = null, params Expression<Func<Employee, object>>[] includeExpressions);
        EmployeeDto Create(EmployeeDto model);
        EmployeeDto Update(EmployeeDto model, out string oldPicName);
        EmployeeDto GetById(int Id);
        Employee GetById(int Id, Expression<Func<Employee, bool>> where = null, params Expression<Func<Employee, object>>[] includeExpressions);
        void Delete(int id);
        void SaveChanges();

        Tuple<object, int> GetEmployeePagedData(EmployeePagedViewModel model);
        object GetEmployeeDataPrint(List<Int64> ids, bool isAll = false);
    }
}
