using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using DLL.Model;
using DLL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   
    public class DepartmentController : OurApplicationController
    {
       
        private IDepartmentService _departmentService;


        public DepartmentController(IDepartmentService departmentService)
        {
          
            _departmentService = departmentService;
        }
        // GET
        [HttpGet]
        public async  Task<ActionResult>GetAllDepartment()
        {
            return Ok(await _departmentService.GetAllDepartmentAsync());
        }

        [HttpGet]
        [Route("code")]
        public async  Task<ActionResult>GetADepartment(string code)
        {
            return Ok(await _departmentService.GetADepartmentAsync(code));
        }

        [HttpPost]
        public async Task<ActionResult>  AddADepartment(DepartmentInsertRequest aDepartment)
        {
         
            return Ok(await _departmentService.AddDepartmentAsync(aDepartment));
        }
        
    }
}