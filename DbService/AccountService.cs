using Aasaan_API.IServices;
using Aasaan_API.Models;
using DB_con;
using System.Data;
using System.Data.SqlClient;

namespace Aasaan_API.DbService
{
  public class AccountService : IAccount
  {
    private readonly ConnectionCls _connectionCls;
    private readonly IConfiguration _configuration;

    public AccountService(IConfiguration configuration)
    {
      _configuration = configuration;
      _connectionCls = new ConnectionCls(_configuration);
    }

    public DataTable ConvertDatareadertoDataTable(IDataReader dr)
    {
      DataTable dt = new DataTable();
      dt.Load(dr);
      return dt;
    }
    public ResponseRegistrationCLS SaveRegistrationData(RequestRegistrationCLS registrationCL)
    {
      try
      {
        _connectionCls.BeginTransaction();
        _connectionCls.clearParameter();
        RegistrationData(registrationCL, DBTrans.Insert);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_Insert_UsersRegistration", CommandType.StoredProcedure));
        _connectionCls.CommitTransaction();
        return ConvertToRegistrationData(dt);
      }
      catch (Exception ex)
      {
        throw new Exception("P2_sp_Insert_UsersRegistration : " + ex.Message);
      }
      finally 
      {
        _connectionCls.clearParameter();
      }
    }

    
    private void RegistrationData(RequestRegistrationCLS obj, DB_con.DBTrans trans)
    {
      try
      {
        _connectionCls.addParameter("@MobileNumber", obj.MobileNumber);
        _connectionCls.addParameter("@EmailID", obj.EmailID);  
        _connectionCls.addParameter("@DeviceID", obj.DeviceID);
        _connectionCls.addParameter("@SubscriptionExpiryDate", obj.SubscriptionExpiryDate);
        _connectionCls.addParameter("@Platform", obj.Platform);
        _connectionCls.addParameter("@AppVersion", obj.AppVersion);
        _connectionCls.addParameter("@AdminNotes", obj.AdminNotes);
        _connectionCls.addParameter("@AppCode", obj.AppCode);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public ResponseRegistrationCLS ConvertToRegistrationData(DataTable dt)
    {
      try
      {
        if (dt == null || dt.Rows.Count == 0)
        {
          return null;
        }

        DataRow row = dt.Rows[0];
        ResponseRegistrationCLS users = new ResponseRegistrationCLS();

        //DateTime createdata = Convert.ToDateTime(row["createdata"]?.ToString() ?? string.Empty);
        //DateTime lastapiDate = Convert.ToDateTime(row["LastAPICallDate"]?.ToString() ?? string.Empty);
        //DateTime expiryDate = Convert.ToDateTime(row["ExpiryDate"]?.ToString() ?? string.Empty);

        users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
        users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
        users.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
        users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]?.ToString() ?? string.Empty);
        users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
        users.SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]?.ToString() ?? string.Empty);
        users.Platform = row["Platform"]?.ToString() ?? string.Empty;
        users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
        users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]?.ToString() ?? string.Empty);
        users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
        users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;
        users.SubscriptionStatus = row["SubscriptionStatus"]?.ToString() ?? string.Empty;
        return users;
      }
      catch (Exception ex)
      {
        throw ex;
      }      
    }

    public LoginModel Login(string username, string password)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@Email", username);
        _connectionCls.addParameter("@Password", password);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_AdminLogin", CommandType.StoredProcedure));
        return ConvertToLoginList(dt);
      }
      catch (Exception ex)
      {
        throw;
      }                      
    }

    public LoginModel ConvertToLoginList(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];
      LoginModel users = new LoginModel();

      users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
      users.Email = row["Email"]?.ToString() ?? string.Empty;
      users.Password = row["Password"]?.ToString() ?? string.Empty;
      return users;
    }

    public ResponseUserModel LogInWithMobileAndDeviceId(string Mobile, string DeviceId)
    {
      try
      {
        _connectionCls.clearParameter();
        _connectionCls.addParameter("@MobileNumber", Mobile);
        _connectionCls.addParameter("@DeviceId", DeviceId);
        DataTable dt = ConvertDatareadertoDataTable(_connectionCls.ExecuteReader("P2_sp_CheckUserMobilenoAndDeviceIdexist", CommandType.StoredProcedure));
        return ConvertToUserLoginList(dt);
      }
      catch (Exception ex)
      {
        throw;
      }
      finally { _connectionCls.clearParameter(); }
    }

    public ResponseUserModel ConvertToUserLoginList(DataTable dt)
    {
      if (dt == null || dt.Rows.Count == 0)
      {
        return null;
      }

      DataRow row = dt.Rows[0];
      ResponseUserModel users = new ResponseUserModel();
      
      users.UserID = Convert.ToInt32(row["UserID"]?.ToString() ?? string.Empty);
      users.MobileNumber = row["MobileNumber"]?.ToString() ?? string.Empty;
      users.EmailID = row["EmailID"]?.ToString() ?? string.Empty;
      users.DateofCreation = Convert.ToDateTime(row["DateofCreation"]?.ToString() ?? string.Empty);
      users.DeviceID = row["DeviceID"]?.ToString() ?? string.Empty;
      users.SubscriptionExpiryDate = Convert.ToDateTime(row["SubscriptionExpiryDate"]?.ToString() ?? string.Empty);
      users.Platform = row["Platform"]?.ToString() ?? string.Empty;
      users.AppVersion = row["AppVersion"]?.ToString() ?? string.Empty;
      users.LastAPICallDate = Convert.ToDateTime(row["LastAPICallDate"]?.ToString() ?? string.Empty);
      users.AdminNotes = row["AdminNotes"]?.ToString() ?? string.Empty;
      users.AppCode = row["AppCode"]?.ToString() ?? string.Empty;
      users.SubscriptionStatus = row["SubscriptionStatus"]?.ToString() ?? string.Empty;

      return users;
    }
    public DateTime? GetNullableDate(object obj)
    {
      if (obj == null || obj == DBNull.Value)
      {
        return null;
      }

      if (DateTime.TryParse(obj.ToString(), out DateTime date))
      {
        return date;
      }

      return null;
    }
  }
}
