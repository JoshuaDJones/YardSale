using Core.YardSale.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.YardSale
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IConfiguration _configuration;
        public DatabaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DataTable GetDT(string storedProcName, List<object>? parameters = null, string connectionStringName = "")
        {
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString(connectionStringName));
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(storedProcName, sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlCommandBuilder.DeriveParameters(cmd);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(storedProcName, sqlConnection);
            sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (cmd.Parameters.Count > 1)
            {
                for (int i = 1; i < cmd.Parameters.Count; i++)
                {
                    cmd.Parameters[i].Value = parameters[i - 1];
                }
            }

            sqlAdapter.SelectCommand = cmd;
            sqlAdapter.Fill(dt);
            sqlConnection.Close();
            sqlConnection.Dispose();
            return dt;
        }

        public int GetRetVal(string storedProcName, List<object>? parameters = null, string connectionStringName = "DatabaseString")
        {
            int retVal;

            SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString(connectionStringName));
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand(storedProcName, sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlCommandBuilder.DeriveParameters(cmd);

            if (cmd.Parameters.Count > 1)
            {
                for (int i = 1; i < cmd.Parameters.Count; i++)
                {
                    cmd.Parameters[i].Value = parameters[i - 1];
                }
            }

            retVal = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return retVal;
        }
    }
}
