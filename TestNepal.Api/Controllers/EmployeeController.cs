using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TestNepal.Api.Controllers;
using TestNepal.Dtos;
using TestNepal.Service.Infrastructure;

namespace WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/employee")]
    public class EmployeeController : BaseApiController
    {

        private IEmployeeService _employeeService;
        private IFileService _fileService;
        public EmployeeController(
            IFileService fileService,
            IEmployeeService employeeService
            )
        {
            _employeeService = employeeService;
            _fileService = fileService;
        }


        [HttpGet]
        [Route("GetEmployeeAll")]
        public HttpResponseMessage GetEmployeeAll([FromUri] TestNepal.Dtos.EmployeePagedViewModel model)
        {
            try
            {
                var data = _employeeService.GetEmployeePagedData(model);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = data.Item1, count = data.Item2, msg = "Employee created successfully!", status = true });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { msg = ex.Message, status = false });
            }
        }

        [HttpGet]
        [Route("EmployeeGetById")]
        public HttpResponseMessage EmployeeGetById([FromUri] int Id)
        {
            try
            {
                var data = _employeeService.GetById(Id);
                try
                {
                    if (!String.IsNullOrEmpty(data.Photo))
                    {
                        data.Photo = ApplicationSettingVariables.WebsiteBaseUrl + ApplicationSettingVariables.ImageUploadPath + data.Photo;
                    }
                }
                catch (Exception ex) { }
                return Request.CreateResponse(HttpStatusCode.OK, new { status = true, data = data });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { msg = ex.Message, status = false });
            }
        }

        [HttpGet]
        [Route("EmployeeGetPrint")]
        public HttpResponseMessage EmployeeGetPrint([FromUri]EmployeePrintGetViewModel model)
        {
            try
            {
                var data = _employeeService.GetEmployeeDataPrint(model.ids, model.isAll);
                return Request.CreateResponse(HttpStatusCode.OK, new { status = true, data = data });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { msg = ex.Message, status = false });
            }
        }


        [HttpPost]
        [Route("SaveEmployee")]
        public HttpResponseMessage SaveEmployee()
        {
            try
            {
                string imageName = null;
                var httpRequest = HttpContext.Current.Request;
                //Upload Image
                var postedFile = httpRequest.Files["Image"];
                var isPhoto = false;
                //Create custom filename
                if (postedFile != null && (!String.IsNullOrEmpty(postedFile.FileName)))
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                    var filePath = HttpContext.Current.Server.MapPath("~/Content/EmpImage/" + imageName);
                    var ImageCaption = httpRequest["ImageCaption"];
                    isPhoto = true;
                    postedFile.SaveAs(filePath);
                }

                TestNepal.Dtos.EmployeeDto empDto = new TestNepal.Dtos.EmployeeDto();
                empDto.Id = Convert.ToInt64(httpRequest["Id"]);
                empDto.DateOfBirth = Convert.ToDateTime(httpRequest["DateOfBirth"]);
                empDto.FullName = httpRequest["FullName"];
                empDto.Salary = Convert.ToDecimal(httpRequest["Salary"]);
                empDto.Designation = httpRequest["Designation"];
                empDto.Gender = httpRequest["Gender"];
                empDto.Photo = isPhoto == true ? imageName : "";
                if (empDto.Id > 0)
                {
                    string oldPhoto = "";
                    _employeeService.Update(empDto, out oldPhoto);
                    if (oldPhoto != "" && isPhoto == true)
                    {
                        try
                        {
                            _fileService.DeleteImage(ApplicationSettingVariables.ImageUploadPath, oldPhoto);
                        }
                        catch (Exception ex) { }
                    }
                }
                else
                {
                    _employeeService.Create(empDto);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { data = empDto, msg = "Employee saved successfully!" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new { }, msg = ex.Message });
            }
        }


        [HttpGet]
        [Route("SyncExcelData")]
        public HttpResponseMessage SyncExcelData()
        {

            List<string> skipedRow = new List<string>();
            List<string> errorMessage = new List<string>();

            try
            {
                string filename = "SampleEmployeeData.xlsx";
                string pathToExcelFile = HttpContext.Current.Server.MapPath("~/Content/ExcelFile/" + filename);
                var connectionString = "";
                if (filename.EndsWith(".xls"))
                {
                    connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                }
                else if (filename.EndsWith(".xlsx"))
                {
                    connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                }

                var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                var ds = new DataSet();
                adapter.Fill(ds, "ExcelTable");
                DataTable dtable = ds.Tables["ExcelTable"];
                string sheetName = "Sheet1";
                var excelFile = new ExcelQueryFactory(pathToExcelFile);
                var employeeDtoList = from a in excelFile.Worksheet<EmployeeDto>(sheetName) select a;
                int index = 2;
                foreach (var a in employeeDtoList)
                {
                    try
                    {
                        if (a.Gender != "" && a.FullName != "" && a.DateOfBirth != null)
                        {

                            EmployeeDto Em = new EmployeeDto();
                            Em.FullName = a.FullName;
                            Em.DateOfBirth = a.DateOfBirth;
                            Em.Gender = a.Gender;
                            Em.Salary = a.Salary;
                            Em.Designation = a.Designation;
                            Em.ImportedDate = DateTime.Now;
                            _employeeService.Create(Em);
                        }
                        else
                        {
                            skipedRow.Add(index.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage.Add(ex.Message);
                    }
                    index++;
                }
            }
            catch (Exception ex)
            {
                errorMessage.Add(ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { msg = "success", data = skipedRow, errorMessage = errorMessage });
        }

    }
}




