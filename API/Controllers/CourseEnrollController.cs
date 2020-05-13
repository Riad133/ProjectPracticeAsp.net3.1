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
    }
}