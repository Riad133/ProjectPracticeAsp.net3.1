using System.Collections.Generic;
using DLL.Model;

namespace BLL.Response
{
    public class StudentCourseEnrollResponse
    {
        public Student Student { get; set; }
        public List<Course> Courses { get; set; }
             
    }
}