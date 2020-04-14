using System.Threading.Tasks;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestController : OurApplicationController
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        // GET
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = User;
            await _testService.SaveData();
            return Ok("Hello");
        }
    }
}