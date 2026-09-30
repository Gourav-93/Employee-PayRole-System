# Employee Management Payroll System
## Project Update

- Reviewed project structure and existing APIs.
- Checked backend modules and database integration.
- Verified the current project setup.

A backend-based **Employee Management and Payroll System** developed using **ASP.NET Core Web API, Entity Framework Core, MySQL, and JWT Authentication**.

The system is designed to manage employees, departments, attendance, payroll-related information, users/roles, and administrative operations through a structured and scalable architecture.

---

## 📌 Project Overview

The **Employee Management Payroll System** provides RESTful APIs for managing employee information and related organizational operations.

The project follows a layered architecture where responsibilities are separated into:

* Controllers
* DTOs
* Services
* Service Interfaces
* Repositories
* Repository Interfaces
* Models
* Database Context
* Authentication & Authorization

This separation makes the application easier to maintain, test, understand, and extend.

---

## 🚀 Key Features

### 👨‍💼 Employee Management

* Add new employees
* Get all employees
* Get employee by ID
* Update employee information
* Delete employee
* Manage employee department and position
* Manage employee status

### 🏢 Department Management

* Create departments
* View departments
* Update departments
* Delete departments
* Assign employees to departments

### 🕐 Attendance Management

* Manage employee attendance
* Track attendance status
* Manage attendance records
* HR/Admin attendance operations

### 💰 Payroll Management

* Manage salary-related employee information
* Maintain payroll-related records
* Support payroll calculation/management functionality
* Organize employee payroll information

### 🔐 Authentication & Authorization

* JWT-based authentication
* Login system
* Role-based authorization
* Protected API endpoints
* Secure access to administrative operations

### 👤 Role Management

Different users can have different permissions based on their role.

Example roles:

* Admin
* HR
* Employee

---

# 🛠️ Technologies Used

## Backend

* **C#**
* **ASP.NET Core Web API**
* **.NET**
* **Entity Framework Core**
* **LINQ**
* **REST API**

## Database

* **MySQL**

## Authentication

* **JWT (JSON Web Token)**
* **Role-Based Authorization**

## Architecture

* Layered Architecture
* Repository Pattern
* Service Layer
* DTO Pattern
* Dependency Injection

## Development Tools

* Visual Studio Code
* Postman
* Git
* GitHub
* MySQL
* Entity Framework Core Migrations

---

# 🏗️ Project Architecture

The project follows a layered architecture.

```text
                    CLIENT
                       │
                       ▼
                ┌─────────────┐
                │ Controller  │
                └──────┬──────┘
                       │
                       ▼
                ┌─────────────┐
                │    DTO      │
                └──────┬──────┘
                       │
                       ▼
                ┌─────────────┐
                │   Service   │
                └──────┬──────┘
                       │
                       ▼
              ┌──────────────────┐
              │ Service Interface│
              └────────┬─────────┘
                       │
                       ▼
                ┌─────────────┐
                │ Repository  │
                └──────┬──────┘
                       │
                       ▼
             ┌──────────────────┐
             │Repository Interface│
             └────────┬─────────┘
                      │
                      ▼
                ┌─────────────┐
                │  DbContext  │
                └──────┬──────┘
                       │
                       ▼
                ┌─────────────┐
                │   MySQL DB  │
                └─────────────┘
```

---

# 📂 Project Structure

```text
EmployeeManagementPayrollSystem
│
├── Controllers
│   ├── EmployeeController.cs
│   ├── DepartmentController.cs
│   ├── AttendanceController.cs
│   ├── PayrollController.cs
│   └── AuthController.cs
│
├── Data
│   └── AppDbContext.cs
│
├── DTOs
│   ├── EmployeeDto.cs
│   ├── DepartmentDto.cs
│   ├── AttendanceDto.cs
│   ├── PayrollDto.cs
│   └── LoginDto.cs
│
├── Models
│   ├── Employee.cs
│   ├── Department.cs
│   ├── Attendance.cs
│   ├── Payroll.cs
│   └── User.cs
│
├── Enums
│   ├── EmployeeStatus.cs
│   ├── AttendanceStatus.cs
│   └── Role.cs
│
├── Repositories
│   ├── EmployeeRepository.cs
│   ├── DepartmentRepository.cs
│   ├── AttendanceRepository.cs
│   └── Interfaces
│       ├── IEmployeeRepository.cs
│       ├── IDepartmentRepository.cs
│       └── IAttendanceRepository.cs
│
├── Services
│   ├── EmployeeService.cs
│   ├── DepartmentService.cs
│   ├── AttendanceService.cs
│   ├── PayrollService.cs
│   │
│   └── Interfaces
│       ├── IEmployeeService.cs
│       ├── IDepartmentService.cs
│       └── IAttendanceService.cs
│
├── Migrations
│
├── Program.cs
├── appsettings.json
└── EmployeeManagementPayrollSystem.csproj
```

> Folder names and files can vary depending on the final version of the project.

---

# 🧩 Main Components

## 1. Models

Models represent the actual database entities.

For example:

```text
Employee
Department
Attendance
Payroll
User
```

These classes contain the properties that represent database columns.

---

## 2. DTOs

DTO stands for **Data Transfer Object**.

DTOs are used to transfer required data between the client and API without directly exposing the complete database model.

```text
Client
   ↓
Controller
   ↓
DTO
   ↓
Service
```

Benefits:

* Controls which data is exposed
* Separates API data from database models
* Improves maintainability
* Helps with request/response validation

---

## 3. Repository

The Repository layer handles database-related operations.

Examples:

```text
GetAll()
GetById()
Add()
Update()
Delete()
```

The repository communicates with `AppDbContext` and Entity Framework Core.

---

## 4. Repository Interface

Interfaces define what operations a repository should provide.

Example:

```csharp
public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
}
```

The interface helps maintain loose coupling between different layers.

---

## 5. Service Layer

The Service layer contains the application's **business logic**.

Example:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Database
```

The controller does not directly communicate with the database.

---

## 6. Service Interface

The service interface defines the operations that the service provides.

Example:

```csharp
IEmployeeService
```

The actual implementation is provided by:

```csharp
EmployeeService
```

This follows the principle of **loose coupling** and makes the application easier to test and maintain.

---

# 🗄️ Database

The project uses **MySQL** as the relational database.

Entity Framework Core is used to communicate with MySQL.

```text
ASP.NET Core
      ↓
Entity Framework Core
      ↓
MySQL
```

---

# 🔄 Entity Framework Core Migrations

Migrations are used to keep the database structure synchronized with the application's models.

Typical commands:

```powershell
dotnet ef migrations add InitialCreate
```

Apply the migration:

```powershell
dotnet ef database update
```

### Migration Methods

Entity Framework migrations mainly contain:

```csharp
Up()
Down()
```

### Up()

Applies the database changes.

### Down()

Reverts the database changes.

---

# 🔐 JWT Authentication

The application uses **JSON Web Token (JWT)** for authentication.

Authentication flow:

```text
User
 │
 ▼
Login
 │
 ▼
AuthController
 │
 ▼
Validate Credentials
 │
 ▼
Generate JWT Token
 │
 ▼
Client
 │
 ▼
Send Token with API Request
 │
 ▼
JWT Authentication Middleware
 │
 ▼
Authorized Controller
```

The token is generally sent using:

```text
Authorization: Bearer <token>
```

---

## API Endpoints

### Authentication
- POST /api/auth/register
- POST /api/auth/login

### Employee
- GET /api/employee
- GET /api/employee/{id}
- POST /api/employee
- PUT /api/employee/{id}
- DELETE /api/employee/{id}

# 🛡️ Authorization

Authorization controls which users are allowed to access particular operations.

For example:

```text
Admin
 ├── Employee Management
 ├── Department Management
 ├── Attendance Management
 └── Payroll Management

HR
 ├── Employee Management
 └── Attendance Management

Employee
 └── Own Employee/Attendance Information
```

Actual permissions depend on the implemented authorization rules.

---

# 🔌 Dependency Injection

The application uses ASP.NET Core's built-in **Dependency Injection** system.

Services and repositories are registered in:

```text
Program.cs
```

Example:

```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
```

This allows ASP.NET Core to automatically provide required dependencies.

---

# 🌐 REST API

The application exposes RESTful API endpoints.

Typical employee endpoints include:

| Method | Endpoint             | Purpose            |
| ------ | -------------------- | ------------------ |
| GET    | `/api/employee`      | Get all employees  |
| GET    | `/api/employee/{id}` | Get employee by ID |
| POST   | `/api/employee`      | Add employee       |
| PUT    | `/api/employee/{id}` | Update employee    |
| DELETE | `/api/employee/{id}` | Delete employee    |

Other controllers provide similar APIs for departments, attendance, payroll, and authentication.

---

# 🔄 CRUD Operations

CRUD stands for:

```text
C → Create
R → Read
U → Update
D → Delete
```

Example employee workflow:

```text
POST    → Create Employee
GET     → Read Employee
PUT     → Update Employee
DELETE  → Delete Employee
```

---

# 🔁 Complete Request Flow

When a client sends an API request:

```text
Client
  │
  ▼
Controller
  │
  ▼
DTO
  │
  ▼
Service
  │
  ▼
Repository
  │
  ▼
AppDbContext
  │
  ▼
Entity Framework Core
  │
  ▼
MySQL Database
```

Response travels back in the opposite direction:

```text
MySQL
  ↓
Entity Framework Core
  ↓
Repository
  ↓
Service
  ↓
Controller
  ↓
DTO
  ↓
Client
```

---

# ⚙️ Installation & Setup

## 1. Clone the Repository

```bash
git clone https://github.com/Gourav-93/EmployeeManagementPayrollSystem.git
```

```bash
cd EmployeeManagementPayrollSystem
```

---

## 2. Configure MySQL

Make sure MySQL is installed and running.

Update the connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=EmployeeManagementPayrollSystem;user=root;password=YOUR_PASSWORD;"
  }
}
```

Replace:

```text
YOUR_PASSWORD
```

with your MySQL password.

---

# 3. Restore Dependencies

Run:

```powershell
dotnet restore
```

---

# 4. Apply Database Migration

Run:

```powershell
dotnet ef database update
```

If migrations have not been created yet:

```powershell
dotnet ef migrations add InitialCreate
```

Then:

```powershell
dotnet ef database update
```

---

# 5. Run the Application

Run:

```powershell
dotnet run
```

The API will start on the configured localhost URL.

---

# 🧪 Testing APIs

The APIs can be tested using:

* Postman
* Swagger
* Browser for GET requests

Recommended testing flow:

```text
1. Register/Login
       ↓
2. Get JWT Token
       ↓
3. Add Bearer Token
       ↓
4. Test Protected APIs
       ↓
5. Perform CRUD Operations
```

---

# 📋 Example Employee Request

### POST

```http
POST /api/employee
```

Example request body:

```json
{
  "name": "Rahul Sharma",
  "email": "rahul@example.com",
  "departmentId": 1,
  "position": "Software Developer"
}
```

The exact request properties depend on the DTO implemented in the project.

---

# 📊 Project Concepts Demonstrated

This project demonstrates practical knowledge of:

* C#
* ASP.NET Core Web API
* REST API
* HTTP methods
* CRUD operations
* Entity Framework Core
* MySQL
* LINQ
* Async/Await
* Dependency Injection
* DTOs
* Repository Pattern
* Service Layer
* Interfaces
* JWT Authentication
* Role-Based Authorization
* Entity Relationships
* Database Migrations
* API Testing with Postman
* Git & GitHub

---

# 🎯 Project Objective

The main objective of this project is to build a structured employee management backend that demonstrates how a real-world application can be divided into multiple layers.

The project focuses on:

* Clean separation of responsibilities
* Secure authentication
* Database management
* Business logic separation
* RESTful API development
* Maintainable code structure
* Scalable backend architecture

---

# 🔮 Future Scope

The system can be extended with:

* Advanced payroll calculation
* Salary slips
* Leave management
* Employee performance management
* Email notifications
* File/document management
* Dashboard and analytics
* Automated monthly payroll
* Export reports to PDF/Excel
* Audit logs
* Cloud deployment
* Frontend using React, Angular, or Blazor

---

# 👨‍💻 Developer

**Gourav Khore**

### Skills Used

```text
C#
ASP.NET Core
.NET
Entity Framework Core
MySQL
REST API
JWT
Git
GitHub
```

---

# ⭐ Conclusion

The **Employee Management Payroll System** is a practical ASP.NET Core Web API project demonstrating a layered backend architecture with Entity Framework Core, MySQL, JWT authentication, DTOs, repositories, services, and RESTful APIs.

The project provides a foundation that can be further extended into a complete enterprise-level employee and payroll management application.

---

## 📜 License

This project is developed for **learning, internship, and portfolio purposes**.
