using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Formats.Asn1;
using System.Net.Http.Json;
using System.Linq.Expressions;

namespace StudentApiClient
{
    class Program
    {
        // Create a single static HttpClient instance to reuse for all requests
        static readonly HttpClient httpclient = new HttpClient();

        // Main async entry point of the console app
        static async Task Main(string[] args)
        {
            // Set the base URL for all HTTP requests
            httpclient.BaseAddress = new Uri("https://localhost:7152/api/Students/");

            // Call method to get all students from API
            await GetAllStudents();

            // Call method to get only passed students (grade >= 50)
            await GetPassedStudents();

            // Call method to get average grade of all students
            await GetAverageGrade();

            // Call method to get details of student with ID 3
            await GetStudentById(3);

            // Create a new student object
            var student = new Student { Name = "Mohamed Mostafa", Age = 44, Grade = 38 };
            // Add this new student via API
            await AddStudent(student);

            // Delete student with ID 4
            await DeleteStudent(4);

            // Update student with ID 2 with new data
            await UpdateStudent(2, new Student { Name = "Salma", Age = 22, Grade = 90 });
        }

        // Fetch all students from "All" endpoint
        static async Task GetAllStudents()
        {
            try
            {
                Console.WriteLine("\n--------------------------------");
                Console.WriteLine("\nFetching all students...\n");

                // Make GET request to api/Students/All and deserialize JSON to list of Student
                var students = await httpclient.GetFromJsonAsync<List<Student>>("All");

                // If students list is not null, print each student's details
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Print error message if something goes wrong
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Fetch students who passed (grade >= 50)
        static async Task GetPassedStudents()
        {
            try
            {
                Console.WriteLine("\n--------------------------------");
                Console.WriteLine("\nFetching Passed students...\n");

                // Make GET request to api/Students/Passed and deserialize to list
                var students = await httpclient.GetFromJsonAsync<List<Student>>("Passed");

                // If not null, print details of passed students
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Print error if exception occurs
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Fetch average grade from API
        static async Task GetAverageGrade()
        {
            try
            {
                Console.WriteLine("\n--------------------------------");
                Console.WriteLine("\nFetching Average Grades...\n");

                // GET request to api/Students/AverageGrade returning a double
                var averageGrade = await httpclient.GetFromJsonAsync<double>("AverageGrade");

                // Print the average grade received
                Console.WriteLine($"Average Grade: {averageGrade}");
            }
            catch (Exception ex)
            {
                // Print exception message if any error happens
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Fetch student details by their ID
        static async Task GetStudentById(int id)
        {
            try
            {
                Console.WriteLine("\n--------------------------------");
                Console.WriteLine($"\nFetching Student With Id: {id}\n");

                // GET request to api/Students/{id} to get specific student
                var student = await httpclient.GetFromJsonAsync<Student>($"{id}");

                // If found, print details; otherwise print not found message
                if (student != null)
                {
                    Console.WriteLine($"Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                }
                else
                {
                    Console.WriteLine("Student Not Found..");
                }
            }
            catch (Exception ex)
            {
                // Print error if exception happens
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Add a new student by sending POST request with JSON body
        static async Task AddStudent(Student newStudent)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nAdding a new student...\n");

                // POST JSON data to api/Students (empty relative path means base URL)
                var response = await httpclient.PostAsJsonAsync("", newStudent);

                // If success, read returned student from response and print details
                if (response.IsSuccessStatusCode)
                {
                    var addedStudent = await response.Content.ReadFromJsonAsync<Student>();
                    Console.WriteLine($"Added Student - ID: {addedStudent.Id}, Name: {addedStudent.Name}, Age: {addedStudent.Age}, Grade: {addedStudent.Grade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // If bad request (validation failed), notify user
                    Console.WriteLine("Bad Request: Invalid student data.");
                }
            }
            catch (Exception ex)
            {
                // Print any exception message
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Delete a student by ID
        static async Task DeleteStudent(int id)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nDeleting student with ID {id}...\n");

                // Send DELETE request to api/Students/{id}
                var response = await httpclient.DeleteAsync($"{id}");

                // Check response and print result
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Student with ID {id} has been deleted.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                // Catch any exceptions and print
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Update existing student by ID with new data
        static async Task UpdateStudent(int id, Student updatedStudent)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nUpdating student with ID {id}...\n");

                // Send PUT request with JSON body to api/Students/{id}
                var response = await httpclient.PutAsJsonAsync($"{id}", updatedStudent);

                // If success, read updated student from response and print details
                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Student>();
                    Console.WriteLine($"Updated Student: ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine("Failed to update student: Invalid data.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                // Print exception message if error occurs
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    // Student class represents the data structure of a student
    public class Student
    {
        public int Id { get; set; }      // Student unique ID
        public string Name { get; set; } // Student's name
        public int Age { get; set; }     // Student's age
        public int Grade { get; set; }   // Student's grade
    }
}
