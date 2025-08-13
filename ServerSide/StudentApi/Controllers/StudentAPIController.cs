using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.DataSimulation;
using StudentApi.Model;
using System.Collections.Generic;
using System.Linq;

namespace StudentApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Students")]                  // Set the base route for this controller
    [ApiController]                         // Enables API-specific behaviors (e.g. automatic model validation)
    public class StudentsController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]   // GET api/Students/All, named route GetAllStudents

        [ProducesResponseType(StatusCodes.Status200OK)]      // Possible 200 OK response
        [ProducesResponseType(StatusCodes.Status404NotFound)]// Possible 404 Not Found response
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            if (StudentDataSimultion.StudentList.Count == 0)  // If no students exist
            {
                return NotFound("No Students Found");         // Return 404 Not Found with message
            }
            return Ok(StudentDataSimultion.StudentList);      // Return 200 OK with student list
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]     // GET api/Students/Passed, named route GetPassedStudents

        [ProducesResponseType(StatusCodes.Status200OK)]      // Possible 200 OK response
        [ProducesResponseType(StatusCodes.Status404NotFound)]// Possible 404 Not Found response
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            var passedStudents = StudentDataSimultion.StudentList.Where(student => student.Grade >= 50); // Filter students with grade >= 50

            if (passedStudents.Count() == 0)                   // If no passed students found
            {
                return NotFound("Not Students Passed.");       // Return 404 Not Found with message
            }
            return Ok(passedStudents);                          // Return 200 OK with passed students list
        }

        [HttpGet("AverageGrade", Name = "GetAverageGrade")]  // GET api/Students/AverageGrade, named route GetAverageGrade

        [ProducesResponseType(StatusCodes.Status200OK)]      // Possible 200 OK response
        [ProducesResponseType(StatusCodes.Status404NotFound)]// Possible 404 Not Found response
        public ActionResult<double> GetAverageGrade()
        {
            if (StudentDataSimultion.StudentList.Count == 0)  // If no students exist
            {
                return NotFound("Not Students found.");        // Return 404 Not Found with message
            }
            var averageGrade = StudentDataSimultion.StudentList.Average(student => student.Grade);  // Calculate average grade
            return Ok(averageGrade);                           // Return 200 OK with average grade
        }

        [HttpGet("{id}", Name = "GetStudentById")]           // GET api/Students/{id}, named route GetStudentById
        [ProducesResponseType(StatusCodes.Status200OK)]      // Possible 200 OK response
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// Possible 400 Bad Request response
        [ProducesResponseType(StatusCodes.Status404NotFound)]// Possible 404 Not Found response
        public ActionResult<Student> GetStudentById(int id)
        {
            if (id < 1)                                        // Validate that id is positive
            {
                return BadRequest($"Not accepted ID: {id}");  // Return 400 Bad Request if id invalid
            }

            var student = StudentDataSimultion.StudentList.FirstOrDefault(s => s.Id == id); // Find student by id

            if (student == null)                               // If student not found
            {
                return NotFound($"Student with ID {id} Not Found.."); // Return 404 Not Found
            }

            return Ok(student);                                // Return 200 OK with found student
        }

        [HttpPost(Name = "AddStudent")]                       // POST api/Students, named route AddStudent
        [ProducesResponseType(StatusCodes.Status201Created)] // Possible 201 Created response
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// Possible 400 Bad Request response
        public ActionResult<Student> AddStudent(Student newStudent)
        {
            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name) || newStudent.Age < 0 || newStudent.Grade < 0)  // Validate input
            {
                return BadRequest("Invalid student data.");   // Return 400 Bad Request if validation fails
            }

            // Assign new Id: max current Id + 1, or 1 if list empty
            newStudent.Id = StudentDataSimultion.StudentList.Count > 0 ? StudentDataSimultion.StudentList.Max(s => s.Id) + 1 : 1;
            StudentDataSimultion.StudentList.Add(newStudent);  // Add new student to list

            // Return 201 Created with location header pointing to GetStudentById route
            return CreatedAtRoute("GetStudentById", new { id = newStudent.Id }, newStudent);
        }

        [HttpDelete("{id}", Name = "DeleteStudent")]         // DELETE api/Students/{id}, named route DeleteStudent
        [ProducesResponseType(StatusCodes.Status200OK)]      // Possible 200 OK response
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// Possible 400 Bad Request response
        [ProducesResponseType(StatusCodes.Status404NotFound)]// Possible 404 Not Found response
        public ActionResult DeleteStudent(int id)
        {
            if (id < 1)                                        // Validate id is positive
            {
                return BadRequest($"Not Accepted Id {id}");   // Return 400 Bad Request if invalid
            }

            var student = StudentDataSimultion.StudentList.FirstOrDefault(s => s.Id == id); // Find student by id
            if (student == null)                               // If student not found
            {
                return NotFound($"Student With ID {id} not found."); // Return 404 Not Found
            }

            StudentDataSimultion.StudentList.Remove(student); // Remove student from list
            return Ok($"Student With Id {id} has been deleted."); // Return 200 OK with confirmation
        }

        [HttpPut("{id}", Name = "UpdateStudent")]             // PUT api/Students/{id}, named route UpdateStudent
        [ProducesResponseType(StatusCodes.Status200OK)]       // Possible 200 OK response
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// Possible 400 Bad Request response
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Possible 404 Not Found response
        public ActionResult<Student> UpdateStudent(int id, Student updatedStudent)
        {
            // Validate id and student data
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");    // Return 400 Bad Request on invalid input
            }

            var student = StudentDataSimultion.StudentList.FirstOrDefault(s => s.Id == id); // Find existing student by id
            if (student == null)                               // If student not found
            {
                return NotFound($"Student with ID {id} not found."); // Return 404 Not Found
            }

            // Update existing student's properties
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;

            return Ok(student);                                // Return 200 OK with updated student
        }
    }
}
