namespace admin_api.Models
{
    public class Employee
    {
        // Properties representing the attributes of an employee.

        // An integer property to store the unique identifier for the employee.
        public int EmployeeId { get; set; }

        // A string property to store the name of the employee.
        public string EmployeeName { get; set; }

        // A string property to store the department of the employee.
        public string Department { get; set; }

        // A DateTime property to store the date of joining of the employee.
        public DateTime DateOfJoining { get; set; }

        // A string property to store the file name of the employee's photo.
        public string PhotoFileName { get; set; }
    }
}
