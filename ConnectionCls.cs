using Microsoft.Extensions.Configuration;  // Import the correct namespace
using System.Data.SqlClient;
using System.Data;
using System;

namespace DB_con
{
    public enum DBTrans
    {
        Insert,
        Update
    }

    public class ConnectionCls
    {
        private readonly IConfiguration _configuration; // Private readonly field
        private readonly string _connectionString;
        private SqlConnection conn = null;
        private SqlCommand cmd = null;
        private SqlDataReader dr = null;
        private SqlTransaction trans;

        public ConnectionCls(IConfiguration configuration) // Constructor injection
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("sqlconstr"); // Retrieve here
            createConnection();
            setconnection();
        }

        // Public property is no longer needed.
        //public string ConnectionString { get; set; }  // Remove this


        public void createConnection()
        {
            try
            {
                conn = new SqlConnection(_connectionString); // Use the injected connection string
                cmd = new SqlCommand();
                cmd.CommandTimeout = 60000 * 30;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void createConnection(Boolean mybool)
        {
            try
            {
                conn = new SqlConnection(_connectionString); // Use injected connection string
                cmd = new SqlCommand();
                cmd.CommandTimeout = 60000 * 30;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setCommandTimeout(Int32 minutes)
        {
            try
            {
                cmd.CommandTimeout = 60000 * 30;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setconnection()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void clearParameter()
        {
            try
            {
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void addParameter(string name, object value)
        {
            try
            {
                cmd.Parameters.AddWithValue(name, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void addParameter(string name, long value, DBTrans trans) // Changed enum name
        {
            try
            {
                if (trans == DBTrans.Insert)
                {
                    cmd.Parameters.AddWithValue(name, value);
                    cmd.Parameters[name].Direction = ParameterDirection.Output;
                }
                else if (trans == DBTrans.Update)
                    cmd.Parameters.AddWithValue(name, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void addParameter(string name, string value, DBTrans trans) //Changed Enum Name
        {
            try
            {
                if (trans == DBTrans.Insert)
                {
                    cmd.Parameters.AddWithValue(name, value);
                    //cmd.Parameters[name].Direction = ParameterDirection.Output;
                }
                else if (trans == DBTrans.Update)
                    cmd.Parameters.AddWithValue(name, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object getValue(string name, CommandType storedProcedure)
        {
            object parameter = null;
            try
            {
                parameter = cmd.Parameters[name].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return parameter;
        }

        public void ExecuteNoneQuery(string commandText, CommandType commandType)
        {
            try
            {
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                setconnection();
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RollbackTransaction();
                if (ex.Number == 1205 || ex.Message.ToLower().Contains("was deadlocked on lock resources") || ex.Message.ToLower().Contains("the size property has an invalid size"))
                {
                    ExecuteNoneQuery(commandText, commandType);
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex1)
            {
                RollbackTransaction();
                if (!ex1.Message.ToLower().Contains("was deadlocked on lock resources") && ex1.Message.ToLower().Contains("the size property has an invalid size"))
                {
                    ExecuteNoneQuery(commandText, commandType);
                }
                else
                {
                    throw ex1;
                }
            }
        }

        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            try
            {
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                setconnection();
                cmd.Connection = conn;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                RollbackTransaction();
                if (ex.Number == 1205 || ex.Message.ToLower().Contains("was deadlocked on lock resources") || ex.Message.ToLower().Contains("the size property has an invalid size"))
                {
                    return ExecuteReader(commandText, commandType);
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex1)
            {
                RollbackTransaction();
                if (!ex1.Message.ToLower().Contains("was deadlocked on lock resources") && ex1.Message.ToLower().Contains("the size property has an invalid size"))
                {
                    return ExecuteReader(commandText, commandType);
                }
                else
                {
                    throw ex1;
                }
            }
        }

        public void BeginTransaction()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    cmd.Transaction = trans;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                if (trans != null)
                {
                    trans.Commit();
                    conn.Close();
                    trans = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                if (trans != null)
                {
                    trans.Rollback();
                    conn.Close();
                    trans = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            try
            {
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                setconnection();
                cmd.Connection = conn;

                object result = cmd.ExecuteScalar();
                return result;
            }
            catch (SqlException ex)
            {
                RollbackTransaction();
                if (ex.Number == 1205 || ex.Message.ToLower().Contains("was deadlocked on lock resources") || ex.Message.ToLower().Contains("the size property has an invalid size"))
                {
                    //System.Threading.Thread.Sleep(1000);
                    return ExecuteScalar(commandText, commandType);
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex1)
            {
                RollbackTransaction();
                if (!ex1.Message.ToLower().Contains("was deadlocked on lock resources") && ex1.Message.ToLower().Contains("the size property has an invalid size"))
                {
                    //System.Threading.Thread.Sleep(1000);
                    return ExecuteScalar(commandText, commandType);
                }
                else
                {
                    throw ex1;
                }
            }
        }            
    }
}