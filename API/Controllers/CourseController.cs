﻿using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CourseController:OurApplicationController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpPost]
        public async Task<ActionResult>  AddADepartment(CourseInsertRequest course)
        {
         
            return Ok(await _courseService.AddCourseAsync(course));
        }
        [HttpGet]
        public async  Task<ActionResult>GetAllCourses()
        {
            return Ok(await _courseService.GetAllCourseAsync());
        }
        [HttpGet]
        [Route("code")]
        public async  Task<ActionResult>GetCourse(string code)
        {
            return Ok(await _courseService.GetACourseAsync(code));
        }
        [HttpPut("{code}")]
        
        
        public async  Task<ActionResult>UpdateCourse(string code,CourseUpdateRequest request)
        {
            return Ok(await _courseService.UpdateAsync(code,request));
        } 
    }
}