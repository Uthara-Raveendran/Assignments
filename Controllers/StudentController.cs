using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentGeo1.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        public static List<Student> _student = new List<Student>();
        public static List<Courses> _courses = new List<Courses>();

        // GET: api/Student
        [HttpGet("api/Student")]
        public IActionResult GetStudent()
        {
            return Ok(_student);
        }




        // GET: api/Student
        [HttpGet("api/Student/list")]
        public IActionResult GetCourse()
        {
            var courseList = _student.GroupBy(x => x.Course).Select (x=>new{x.Key, count = x.Count()});
            return Ok(courseList);
        }

        // GET: api/Student/5
        [HttpGet("api/Student/{id}")]
        public IActionResult GetStudentById(int id)
        {
            var checkId = _student.SingleOrDefault(x => x.Id == id);
            if (checkId==null)
            {
                return NotFound();
            }
            return Ok(checkId);
        }
        //POST Api for courses
        [HttpPost("api/Courses")]
        public IActionResult CreateCourse(Courses courses)
        {
            var lastCourse = _courses.OrderBy(x => x.courseId).LastOrDefault();
            int id = lastCourse == null ? 1 : lastCourse.courseId + 1;

            var courseToBeAdded = new Courses
            {
                courseId = id,
                courseName = courses.courseName

            };
            _courses.Add(courseToBeAdded);
            return Ok(courseToBeAdded.courseId);
        }



        // POST: api/Student
        [HttpPost("api/Student")]
        public IActionResult CreateStud(Student student)
        {
            int id;
            bool flag = false;
            var lastStudent = _student.OrderBy(x => x.Id).LastOrDefault();
            id = lastStudent == null ? 1 : lastStudent.Id + 1;
            foreach (var course in _courses)
            {
                if (student.Course == course.courseName)
                {
                    flag = true;
                }

            }
            if (flag == false)
            {
                return Conflict("Course is not in the list");
            }

            if (Convert.ToDateTime(student.Dob)>DateTime.Now)
            {
                return Conflict("Enter a valid date");
            }
            if (Convert.ToDateTime(student.EnrollmentDate) > DateTime.Now)
            {
                return Conflict("Enter a valid date");
            }

            var GiveStudentDetails = new Student
            {
                Id= id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Dob = student.Dob,
                Address = student.Address,
                PhoneNumber = student.PhoneNumber,
                Course = student.Course,
                EnrollmentDate = student.EnrollmentDate
            };
            _student.Add(GiveStudentDetails);
            return Ok(GiveStudentDetails.Id);
        }

        // PUT: api/Student/5
        [HttpPut("api/Student/{id}")]
        public IActionResult Put(int id,Student student)
        {
            var checkId = _student.SingleOrDefault(x => x.Id == id);
            if (checkId == null)
            {
                return NotFound();
            }
            checkId.FirstName = student.FirstName;
            checkId.LastName = student.LastName;
            checkId.Dob = student.Dob;
            checkId.Address = student.Address;
            checkId.PhoneNumber = student.PhoneNumber;
            checkId.Course = student.Course;
            checkId.EnrollmentDate = student.EnrollmentDate;
            return Ok(checkId);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("api/Student/{id}")]
        public IActionResult Delete(int id)
        {
            var checkId = _student.SingleOrDefault(x => x.Id == id);
            if (checkId == null)
            {
                return NotFound();
            }
            _student.Remove(checkId);

            return Ok();
            
        }
    }
}
