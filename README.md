# EmpMan - Employee Management System

A Blazor WebAssembly application with an integrated ASP.NET Core Web API for managing employee data.

## Features
- Employee CRUD operations (Create, Read, Update, Delete) using Web API.
- SQL Server with stored procedures for managing employee data.
- Swagger UI for testing API endpoints.

## Technologies
- **Blazor WebAssembly**
- **ASP.NET Core Web API** 
- **Entity Framework Core** 
- **SQL Server** 
- **Swagger**

## Project Files and Resources
You can view the video output, screenshots, and download the SQL file for the project at the following link:

[Project Files and Resources](https://bit.ly/carlos-test-files)


## Setup

### 1. Clone the Repository

git [https://github.com/CarlosNatanauan/EmpMan-Blazor.git](https://github.com/CarlosNatanauan/EmpMan-Blazor.git)

### 2. Configure Database
Update the connection string in `appsettings.json` to match your SQL Server setup:

```json
"ConnectionStrings": {
  "EmpManDBContext": "Server=YOUR_SERVER_NAME\\SQLEXPRESS;Database=EmpMan;Trusted_Connection=True;"
}
```

### 3. Configure API Base URL
Update the ApiBaseUrl in appsettings.json to match your local development environment:

```json
"ApiBaseUrl": "http://localhost:PORT_NUMBER"
```

### 4. Install Dependencies
Open Package Manager Console in Visual Studio and run:

```bash
dotnet restore
```

### 5. Set Up the Database
Run the following command in the Package Manager Console to apply migrations:

```bash
Update-Database
```

### 6. Run the Stored Procedures
After the database is set up, run the DatabaseScripts/stored_procedures.sql file using SQL Server Management Studio (SSMS) or another SQL tool:
Open SSMS.
Open the stored_procedures.sql file.
Run the script to create the stored procedures.

### 7. Run the Application
Start the app using Visual Studio or by running:

```bash
dotnet run
```

### 8. Test the API with Swagger

```bash
https://localhost:PORT_NUMBER/swagger
```


### Stored Procedures
Stored procedures for employee management are located in the DatabaseScripts/stored_procedures.sql file.
To set them up, run the SQL script using SQL Server Management Studio (SSMS) or any other SQL client.
