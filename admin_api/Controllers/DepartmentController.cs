using admin_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace admin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        // Constructor that injects IConfiguration for configuration settings.
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // API method to get data from the "Department" table (HTTP GET).
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                string query = @"SELECT * FROM Department";

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

        // API method to insert a new department record (HTTP POST).
        [HttpPost]
        public IActionResult Post(Department dep)
        {
            try
            {
                string query = @"INSERT INTO Department VALUES(@DepartmentName)";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue(@"DepartmentName", dep.DepartmentName);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myCon.Close();
                    }
                }

                // Return a success message with a 200 OK status code.
                return Ok(new { status = 200, message = "Department record added successfully" });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        // API method to update a department record (HTTP PUT).
        [HttpPut]
        public IActionResult Put(Department dep)
        {
            try
            {
                string query = @"UPDATE Department SET DepartmentName = @DepartmentName WHERE DepartmentId = @DepartmentId";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue(@"DepartmentId", dep.DepartmentId);
                        myCommand.Parameters.AddWithValue(@"DepartmentName", dep.DepartmentName);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myCon.Close();
                    }
                }

                // Return a success message with a 200 OK status code.
                return Ok(new { status = 200, message = "Department record updated successfully" });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        // API method to delete a department record by DepartmentId (HTTP DELETE).
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            try
            {
                string query = @"DELETE FROM Department WHERE DepartmentId = @DepartmentId";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue(@"DepartmentId", Id);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myCon.Close();
                    }
                }

                // Return a success message with a 200 OK status code.
                return Ok(new { status = 200, message = "Department record deleted successfully" });
            }
            catch (Exception ex)
            {
                // Return an error message with a 500 Internal Server Error status code.
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }
    }
}
