using System.Threading.Tasks;
using BLL.Services;
using DLL.MongoReport.Model;
using DLL.MongoReport.Repositories;
using Microsoft.AspNetCore.Mvc;
using Utility.Helper;

namespace API.Controllers
{
    public class TestController : OurApplicationController
    {
        private readonly ITestService _testService;
        private readonly TaposRSA _taposRsa;
        private readonly IDepartmentStudentMongoRepository _mongoRepository;

        public TestController(ITestService testService,TaposRSA taposRsa, IDepartmentStudentMongoRepository mongoRepository)
        {
            _testService = testService;
            _taposRsa = taposRsa;
            _mongoRepository = mongoRepository;
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
        [HttpPost]
        public ActionResult MongoTry()
        {
            var mongoReport = new DepartmentStudentReportMongo
            {
                StudentName = "Nazrul Islma RIad",
                StudentEmail = "riad.nazrul@gmail.com",
                DepartmentCode = "CSE",
                DepartmentName = "Computer Science"
                
            };
            _mongoRepository.Create(mongoReport);
            return Ok("Hello Mongo");
        }
    }
}