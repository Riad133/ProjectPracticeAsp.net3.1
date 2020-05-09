using System.Threading.Tasks;
using BLL.Services;
using DLL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StudentDepartmentController : OurApplicationController
    {
        private readonly IStudentService _studentService;

        public StudentDepartmentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        // GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok( await _studentService.GetAllStudentDepartmentReportAsync());
        }
    }
}