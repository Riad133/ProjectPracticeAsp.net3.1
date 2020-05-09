using System.Threading.Tasks;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Utility.Helper;

namespace API.Controllers
{
    public class TestController : OurApplicationController
    {
        private readonly ITestService _testService;
        private readonly TaposRSA _taposRsa;

        public TestController(ITestService testService,TaposRSA taposRsa)
        {
            _testService = testService;
            _taposRsa = taposRsa;
        }
        // GET
        [HttpGet]
        public ActionResult Index()
        {
            //var user = User;
          //  await _testService.SaveData();
          _taposRsa.GenerateRsaKey("v1");
            return Ok("Hello");
        }
    }
}