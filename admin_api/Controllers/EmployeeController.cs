using admin_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System;

namespace admin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // Constructor that injects IConfiguration and IWebHostEnvironment for configuration and file handling.
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // API method to get data from the "Employee" table (HTTP GET).
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string query = @"SELECT * FROM Employee";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myCon.Close();
                    }
                }

                // Return the result as JSON with a 200 OK status code.
                return Ok(new { status = 200, data = table });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        // API method to insert a new employee record (HTTP POST).
        [HttpPost]
        public IActionResult Post(Employee emp)
        {
            try
            {
                string query = @"INSERT INTO Employee VALUES(@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue(@"EmployeeName", emp.EmployeeName);
                        myCommand.Parameters.AddWithValue(@"Department", emp.Department);

                        // Convert the DateOfJoining to the appropriate format (assuming it's a DateTime).
                        myCommand.Parameters.AddWithValue(@"DateOfJoining", emp.DateOfJoining);

                        myCommand.Parameters.AddWithValue(@"PhotoFileName", emp.PhotoFileName);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myCon.Close();
                    }
                }

                // Return a success message with a 200 OK status code.
                return Ok(new { status = 200, message = "Employee record added successfully" });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        // API method to update an employee record (HTTP PUT).
        [HttpPut]
        public IActionResult Put(Employee emp)
        {
            try
            {
                string query = @"UPDATE Employee SET EmployeeName = @EmployeeName, Department = @Department,
                                DateOfJoining = @DateOfJoining,
                                PhotoFileName = @PhotoFileName
                                WHERE EmployeeId = @EmployeeId";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue(@"EmployeeId", emp.EmployeeId);
                        myCommand.Parameters.AddWithValue(@"EmployeeName", emp.EmployeeName);
                        myCommand.Parameters.AddWithValue(@"Department", emp.Department);
                        myCommand.Parameters.AddWithValue(@"DateOfJoining", emp.DateOfJoining);
                        myCommand.Parameters.AddWithValue(@"PhotoFileName", emp.PhotoFileName);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myCon.Close();
                    }
                }

                // Return a success message with a 200 OK status code.
                return Ok(new { status = 200, message = "Employee record updated successfully" });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        // API method to delete an employee record by EmployeeId (HTTP DELETE).
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            try
            {
                string query = @"DELETE FROM Employee WHERE EmployeeId = @EmployeeId";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue(@"EmployeeId", Id);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myCon.Close();
                    }
                }

                // Return a success message with a 200 OK status code.
                return Ok(new { status = 200, message = "Employee record deleted successfully" });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        // API method to handle file upload and save it to the server (HTTP POST).
        [Route("SaveFile")]
        [HttpPost]
        public IActionResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = Path.Combine(_env.ContentRootPath, "Photos", filename);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                // Return a success message with a 200 OK status code.
                return Ok(new { status = 200, message = "File uploaded successfully", fileName = filename });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }
    }
}
