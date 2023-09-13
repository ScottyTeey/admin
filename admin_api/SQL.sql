CREATE TABLE dbo.Department (
    DepartmentId INT IDENTITY(1, 1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE
);

--creating employees table
CREATE TABLE dbo.Employee (
    EmployeeId INT IDENTITY(1, 1) PRIMARY KEY,
    EmployeeName NVARCHAR(100) NOT NULL,
    DepartmentId INT NOT NULL,
    DateOfJoining DATETIME,
    PhotoFileName NVARCHAR(100)
);

ALTER TABLE dbo.Employee
ADD CONSTRAINT FK_Employee_Department FOREIGN KEY (DepartmentId) REFERENCES dbo.Department(DepartmentId);

--inserting records
-- Insert departments
INSERT INTO dbo.Department (DepartmentName)
VALUES ('Infrastructure'), ('Production Support'), ('Developers');

-- Insert employees
INSERT INTO dbo.Employee (EmployeeName, DepartmentId, DateOfJoining, PhotoFileName)
VALUES ('Thabelo', 3, GETDATE(), 'anonymous.jpg');


--querying the information
-- Select all departments
SELECT * FROM dbo.Department;
SELECT * FROM Employee

-- Select all employees with department names
SELECT e.EmployeeId, e.EmployeeName, d.DepartmentName, e.DateOfJoining, e.PhotoFileName
FROM dbo.Employee e
JOIN dbo.Department d ON e.DepartmentId = d.DepartmentId;
