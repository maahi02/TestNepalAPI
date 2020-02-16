using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestNepal.Dtos;
using TestNepal.Entities;
using TestNepal.Repository.Common;
using TestNepal.Repository.Infrastructure;
using TestNepal.Service.Infrastructure;

namespace TestNepal.Service
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;
        IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetAll()
        {
            IEnumerable<Employee> dqlists = _employeeRepository.GetAll().ToList();
            return _mapper.Map<List<Dtos.EmployeeDto>>(dqlists);
        }

        public IEnumerable<EmployeeDto> GetAll(Expression<Func<Employee, bool>> where = null)
        {
            var lists = _employeeRepository.GetMany(where).ToList();
            return _mapper.Map<List<Dtos.EmployeeDto>>(lists);
        }
        public IEnumerable<Employee> GetAll(params Expression<Func<Employee, object>>[] includeExpressions)
        {
            return _employeeRepository.GetAll(includeExpressions);
        }
        public IEnumerable<Employee> GetAll(Expression<Func<Employee, bool>> where = null, params Expression<Func<Employee, object>>[] includeExpressions)
        {
            return _employeeRepository.GetMany(where, includeExpressions);
        }

        public EmployeeDto Create(EmployeeDto model)
        {
            Employee _model = _mapper.Map<Employee>(model);
            _employeeRepository.Add(_model);
            _employeeRepository.SaveChanges();
            return _mapper.Map<EmployeeDto>(_model);
        }
        public EmployeeDto Update(EmployeeDto model, out string oldPicName)
        {
            Employee em = _employeeRepository.GetById(model.Id);
            oldPicName = em.Photo;
            em.DateOfBirth = model.DateOfBirth;
            em.Designation = model.Designation;
            em.FullName = model.FullName;
            em.Gender = model.Gender;
            em.Salary = model.Salary;
            if (!String.IsNullOrEmpty(model.Photo))
            {
                em.Photo = model.Photo;
            }
            _employeeRepository.Update(em);
            _employeeRepository.SaveChanges();
            return _mapper.Map<EmployeeDto>(em);
        }

        public EmployeeDto GetById(int Id)
        {
            Employee model;
            if (Id == 0)
            {
                model = new Entities.Employee();
            }
            else
            {
                model = _employeeRepository.GetById(Id);
            }
            return _mapper.Map<EmployeeDto>(model);
        }
        public Employee GetById(int Id, Expression<Func<Employee, bool>> where = null, params Expression<Func<Employee, object>>[] includeExpressions)
        {
            return _employeeRepository.GetById(Id, where, includeExpressions);
        }

        public void Delete(int id)
        {
            _employeeRepository.Delete(f => f.Id == id);
            _employeeRepository.SaveChanges();
        }
        public void SaveChanges()
        {
            _employeeRepository.SaveChanges();
        }

        public Tuple<object, int> GetEmployeePagedData(EmployeePagedViewModel model)
        {
            int count = 0;
            if (model.Page == 0) model.Page = 1;
            dynamic data = null;
            List<Employee> listEmp = null;
            try
            {
                var skip = (model.Page - 1) * model.PageSize;
                var iQueryableData = _employeeRepository.GetAll();

                if (!String.IsNullOrEmpty(model.Gender))
                {
                    iQueryableData = iQueryableData.Where(x => x.Gender == model.Gender);
                }
                if (model.StartSalary > 0 && model.EndSalary > 0)
                {
                    iQueryableData = iQueryableData.Where(x => x.Salary >= model.StartSalary && x.Salary <= model.EndSalary);
                }
                else if (model.StartSalary > 0)
                {
                    iQueryableData = iQueryableData.Where(x => x.Salary >= model.StartSalary);
                }
                else if (model.EndSalary > 0)
                {
                    iQueryableData = iQueryableData.Where(x => x.Salary <= model.EndSalary);
                }

                if (model.DobStart != null && model.DobEnd != null)
                {
                    iQueryableData = iQueryableData.Where(x => x.DateOfBirth >= model.DobStart && x.DateOfBirth <= model.DobEnd);
                }
                else if (model.DobStart != null)
                {
                    iQueryableData = iQueryableData.Where(x => x.DateOfBirth >= model.DobStart);
                }
                else if (model.DobEnd != null)
                {
                    iQueryableData = iQueryableData.Where(x => x.DateOfBirth <= model.DobEnd);
                }

                if (!String.IsNullOrEmpty(model.SearchText))
                {
                    var _searchTextLower = model.SearchText.ToLower();
                    iQueryableData = iQueryableData.Where(x => x.FullName.ToLower().Contains(_searchTextLower) ||
                    x.Salary.ToString().Contains(_searchTextLower) ||
                    x.Designation.ToLower().Contains(_searchTextLower) ||
                    x.Gender.ToLower().Contains(_searchTextLower) ||
                    x.DateOfBirth.ToString().Contains(_searchTextLower));
                }

                switch (model.Sort)
                {
                    case "fullName":
                        iQueryableData = (model.Order == "asc") ? (iQueryableData.OrderBy(x => x.FullName)) : (iQueryableData.OrderByDescending(x => x.FullName));
                        break;

                    case "dateOfBirth":
                        iQueryableData = (model.Order == "asc") ? (iQueryableData.OrderBy(x => x.DateOfBirth)) : (iQueryableData.OrderByDescending(x => x.DateOfBirth));
                        break;

                    case "salary":
                        iQueryableData = (model.Order == "asc") ? (iQueryableData.OrderBy(x => x.Salary)) : (iQueryableData.OrderByDescending(x => x.Salary));
                        break;

                    case "designation":
                        iQueryableData = (model.Order == "asc") ? (iQueryableData.OrderBy(x => x.Designation)) : (iQueryableData.OrderByDescending(x => x.Designation));
                        break;

                    case "gender":
                        iQueryableData = (model.Order == "asc") ? (iQueryableData.OrderBy(x => x.Gender)) : (iQueryableData.OrderByDescending(x => x.Gender));
                        break;

                    case "id":
                        iQueryableData = (model.Order == "asc") ? (iQueryableData.OrderBy(x => x.Id)) : (iQueryableData.OrderByDescending(x => x.Id));
                        break;

                    default:
                        iQueryableData = iQueryableData.OrderByDescending(x => x.CreatedOn);
                        break;
                }

                count = iQueryableData.Count();

                //data = (from ms in iQueryableData
                //                select new
                //                {
                //                    ms
                //                }).Skip(skip).Take(model.PageSize).ToList();

                listEmp = iQueryableData.
                         Skip(skip).Take(model.PageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new Tuple<object, int>(listEmp, count);
        }


        public object GetEmployeeDataPrint(List<Int64> ids, bool isAll = false)
        {

            dynamic data = null;
            //var iQueryableData = _employeeRepository.GetAll();
            if (isAll)
            {
                data = (from ms in _employeeRepository.GetAll().OrderBy(x => x.CreatedOn)
                        select new
                        {
                            Id = ms.Id,
                            FullName = ms.FullName,
                            ms.DateOfBirth,
                            ms.Gender,
                            ms.Salary,
                            ms.Designation
                        }).ToList();
            }
            else
            {
                data = (from ms in _employeeRepository.GetMany(x => ids.Contains(x.Id))
                        select new
                        {
                            Id = ms.Id,
                            FullName = ms.FullName,
                            ms.DateOfBirth,
                            ms.Gender,
                            ms.Salary,
                            ms.Designation
                        }).ToList();
            }

            return data;
        }


    }
}
