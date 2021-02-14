using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BLL.Services;
using DLL.MongoReport.Model;
using DLL.MongoReport.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Utility.Helper;

namespace API.Controllers
{
    public class TestController : OurApplicationController
    {
        private readonly ITestService _testService;
        private readonly TaposRSA _taposRsa;
        private readonly IDepartmentStudentMongoRepository _mongoRepository;
        private readonly IAccountService _accountService;

        public TestController(ITestService testService,TaposRSA taposRsa, IDepartmentStudentMongoRepository mongoRepository,IAccountService accountService)
        {
            _testService = testService;
            _taposRsa = taposRsa;
            _mongoRepository = mongoRepository;
            _accountService = accountService;
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
            var myroles = new List<Role>
            {
                new Role
                {
                    Name = "Admin",
                    ID = "1234"
                }
            };
            var mongoReport = new DepartmentStudentReportMongo
            {
                StudentName = "Arifur Rahman",
                StudentEmail = "arif@gmail.com",
                DepartmentCode = "CSE",
                DepartmentName = "Computer Science",
                Roles = myroles
                
            };
            _mongoRepository.Create(mongoReport);
            return Ok("Hello Mongo");
        }
        
        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser()
        {
           await _testService.SaveData();
            return Ok("Hello Mongo");
        }
        
        [HttpGet("GetAllUsers")]
        public async  Task<IActionResult>GetAllUsers()
        {
          var result= await   _mongoRepository.GetAll(new DepartmentStudentReportMongo());
         
            return Ok(result);
        }
        [HttpGet("GetAUser")]
        public async  Task<IActionResult>GetAUser()
        {
            var result= await   _mongoRepository.GetFilter("Admin");
         
            return Ok(result);
        }
        [HttpGet("GetReports")]
        public async  Task<IActionResult>GetReports()
        {
           //  Document doc = new Document(PageSize.A5);
           //  FileStream file= new FileStream("helloworl.pdf", FileMode.Create);
           //  PdfWriter writer = PdfWriter.GetInstance(doc, file);
           //  doc.AddAuthor("Riad");
           //  doc.AddTitle("Hello World");
           //  doc.Open();
           //  doc.Add(new Phrase("Hello World"));
           //  writer.Close();
           //  doc.Close();
           //  file.Dispose();
           // // var result= await  _testService.Reports();
           // // var pdf = new FileStream("hello.pdf",FileMode.Open,FileAccess.Read);
           //  return File(file, "application/pdf","hello.pdf");
           var result =await _testService.Reports();
           return File(result, "application/pdf","hello.pdf");


        }
    }
    



}