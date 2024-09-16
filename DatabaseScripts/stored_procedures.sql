-- stored_procedures.sql
-- This script creates the necessary stored procedures for the Employee Management Application.

USE EmpMan; -- Replace with your database name
GO

-- Stored Procedure for Retrieving an Employee by ID
CREATE PROCEDURE spGetEmployeeById
    @Id INT
AS
BEGIN
    SELECT Id, Fname, Lname, Email
    FROM Employee
    WHERE Id = @Id;
END
GO

-- Stored Procedure for Retrieving All Employees
CREATE PROCEDURE spGetAllEmployees
AS
BEGIN
    SELECT Id, Fname, Lname, Email
    FROM Employee;
END
GO

-- Stored Procedure for Creating a New Employee
CREATE PROCEDURE spCreateEmployee
    @Fname NVARCHAR(50),
    @Lname NVARCHAR(50),
    @Email NVARCHAR(100),
    @NewEmployeeId INT OUTPUT
AS
BEGIN
    INSERT INTO Employee (Fname, Lname, Email)
    VALUES (@Fname, @Lname, @Email);

    SET @NewEmployeeId = SCOPE_IDENTITY();
END
GO

-- Stored Procedure for Updating an Existing Employee
CREATE PROCEDURE spUpdateEmployee
    @Id INT,
    @Fname NVARCHAR(50),
    @Lname NVARCHAR(50),
    @Email NVARCHAR(100)
AS
BEGIN
    UPDATE Employee
    SET Fname = @Fname,
        Lname = @Lname,
        Email = @Email
    WHERE Id = @Id;
END
GO

-- Stored Procedure for Deleting an Employee
CREATE PROCEDURE spDeleteEmployee
    @Id INT
AS
BEGIN
    DELETE FROM Employee
    WHERE Id = @Id;
END
GO
