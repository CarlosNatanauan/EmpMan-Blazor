using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmpMan.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmpMan.Data
{
    public class EmpManDBContext : DbContext
    {
        public EmpManDBContext(DbContextOptions<EmpManDBContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; } = default!;

        // Retrieve an employee by ID using a stored procedure
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            var employee = await Employee
                .FromSqlRaw("EXEC spGetEmployeeById @Id", idParam)
                .ToListAsync(); // Retrieve as a list first

            return employee.FirstOrDefault(); // Return the first result or null
        }

        // Retrieve all employees using a stored procedure
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var employees = await Employee
                .FromSqlRaw("EXEC spGetAllEmployees")
                .ToListAsync(); // Retrieve as a list

            return employees;
        }

        // Create a new employee using a stored procedure and return the new ID
        public async Task<int> CreateEmployeeAsync(string fname, string lname, string email)
        {
            var conn = Database.GetDbConnection();  // Get the database connection
            await conn.OpenAsync();  // Ensure the connection is open

            using (var command = conn.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spCreateEmployee";

                command.Parameters.Add(new SqlParameter("@Fname", fname));
                command.Parameters.Add(new SqlParameter("@Lname", lname));
                command.Parameters.Add(new SqlParameter("@Email", email));

                var outputParam = new SqlParameter
                {
                    ParameterName = "@NewEmployeeId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputParam);

                await command.ExecuteNonQueryAsync();  // Execute the stored procedure

                return (int)outputParam.Value;  // Retrieve the output parameter value
            }
        }


        // Update an existing employee using a stored procedure
        public async Task UpdateEmployeeAsync(int id, string fname, string lname, string email)
        {
            var idParam = new SqlParameter("@Id", id);
            var fnameParam = new SqlParameter("@Fname", fname);
            var lnameParam = new SqlParameter("@Lname", lname);
            var emailParam = new SqlParameter("@Email", email);

            try
            {
                await Database.ExecuteSqlRawAsync("EXEC spUpdateEmployee @Id, @Fname, @Lname, @Email",
                    idParam, fnameParam, lnameParam, emailParam);
            }
            catch (Exception ex)
            {
                // Log or handle exception
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
            }
        }


        // Delete an employee using a stored procedure
        public async Task DeleteEmployeeAsync(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            await Database.ExecuteSqlRawAsync("EXEC spDeleteEmployee @Id", idParam);
        }
    }
}
