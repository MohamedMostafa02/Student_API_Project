# Student API

A simple ASP.NET Core Web API to manage students, including retrieving student lists, filtering passed students, calculating average grades, and performing CRUD operations (Create, Read, Update, Delete) on student data.

✅ Features
- Get all students
- Get passed students (grade >= 50)
- Get average grade of all students
- Get student by ID
- Add a new student
- Update an existing student
- Delete a student by ID

🧠 How It Works
- The API exposes several endpoints under the base route /api/Students.
- Uses in-memory data simulation (StudentDataSimultion.StudentList) as the data source.
- Supports RESTful HTTP methods: GET, POST, PUT, DELETE.
- Validates input data and returns appropriate HTTP status codes:
  200 OK for successful requests,
  201 Created when a new student is added,
  400 Bad Request for invalid data or parameters,
  404 Not Found when a student or resource is not found.

📡 API Endpoints

HTTP Method | Route                  | Description
------------|------------------------|----------------------------------
GET         | /api/Students/All       | Retrieve all students
GET         | /api/Students/Passed    | Retrieve students with grade ≥ 50
GET         | /api/Students/AverageGrade | Get average grade of all students
GET         | /api/Students/{id}      | Get student by ID
POST        | /api/Students           | Add a new student
PUT         | /api/Students/{id}      | Update an existing student
DELETE      | /api/Students/{id}      | Delete a student by ID

▶️ How to Run
1. Clone the repository or add the StudentsController class to your ASP.NET Core Web API project.
2. Ensure you have a StudentDataSimultion class with a StudentList in-memory collection.
3. Build and run the project.
4. Access the endpoints using tools like Postman or your HTTP client.
5. Example base URL: https://localhost:{port}/api/Students

👤 Author
Mohamed Mostafa
