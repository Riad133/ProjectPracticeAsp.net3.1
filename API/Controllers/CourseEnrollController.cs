using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CourseEnrollController : OurApplicationController
    {
        private readonly ICourseEnrollService _courseEnrollService;

        public CourseEnrollController(ICourseEnrollService courseEnrollService)
        {
            _courseEnrollService = courseEnrollService;
        }
        [HttpPost]
        public async Task<ActionResult>  EnrollCourse(CourseEnrollRequest aCourseEnrollRequest)
        {
         
            return Ok(await _courseEnrollService.AddCourseEnrollAsync(aCourseEnrollRequest));

        }

        [HttpGet]
        [Route("rollNo")]
        public async Task<ActionResult> StudentEnrollReport(string rollno)
        {
            return  Ok(await _courseEnrollService.GetAStudentReport(rollno));
        }
        [HttpGet]
       
        public async Task<ActionResult> AllStudentEnrollReport()
        {
            return  Ok(await _courseEnrollService.GetAllStudentReport());
        }
    }
}