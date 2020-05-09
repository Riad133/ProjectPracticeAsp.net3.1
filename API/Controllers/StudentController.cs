using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using DLL.Model;
using DLL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  
    public class StudentController : OurApplicationController
    {
       
        private IStudentService _studentService;

        public StudentController( IStudentService studentService)
        {
            _studentService = studentService;
            
        }
        // GET
        [HttpGet]
        [Authorize(Roles = "teacher")]
        public async  Task<ActionResult> GetAll()
        {
           
            return Ok(await  _studentService.GetAllStudentAsync());
        }
       

        [HttpGet]
        [Authorize(Roles = "teacher")]
        [Route("{rollNo}")]
        public async Task<ActionResult>GetSingleStudent(string rollNo)
        {
            return Ok(await _studentService.GetAStudentAsync(rollNo));
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
       
        public async  Task<ActionResult> InsertStudent(StudentInsertRequest aStudent)
        {
            return Ok(await _studentService.AddStudentAsync(aStudent));
        }
       [HttpPut("{rollNo}")]
       [Authorize(Roles = "staff")]
        
        public async  Task<ActionResult>UpdateStudent(string rollNo,StudentUpdateRequest aStudent)
        {
            return Ok(await _studentService.UpdateStudentAsync(rollNo,aStudent));
        }
/*
        [HttpDelete("{email}")]
        public ActionResult DeleteStudent(string email)
        {
            _studentService.DeleteStudent(email);
            return Ok();
        }
    
*/
    
    }
}